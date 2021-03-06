import { AfterViewInit, Component, OnDestroy } from '@angular/core'
import { merge, Observable, Subject } from 'rxjs'
import { FriendRequestDto, FriendRequestsClient, FriendshipCreateDto, FriendshipsClient } from '../../../shared/clients'
import { PagerModel } from '../../../shared/models/pager.model'
import { map, share, switchMap } from 'rxjs/operators'
import { defaultPageSize } from '../../../shared/constants'
import { HubService } from '../../../shared/services/hub.service'

@Component({
  selector: 'app-incoming-friend-requests',
  templateUrl: './incoming-friend-requests.component.html',
  styleUrls: ['./incoming-friend-requests.component.scss']
})
export class IncomingFriendRequestsComponent implements AfterViewInit, OnDestroy {
  currentPage = 0
  loadItems$ = new Subject<void>()
  requests$: Observable<FriendRequestDto[]>
  pager$: Observable<PagerModel>

  constructor(
    private friendRequestsClient: FriendRequestsClient,
    private friendshipsClient: FriendshipsClient,
    private hubService: HubService
  ) {
    const response$ = merge(
      this.loadItems$,
      hubService.friendRequestCreated$,
      hubService.friendRequestDeleted$
    ).pipe(
      switchMap(() => this.friendRequestsClient.listReceivedRequests(this.currentPage, defaultPageSize)),
      share()
    )

    this.requests$ = response$.pipe(
      map(x => x.data)
    )

    this.pager$ = response$.pipe(
      map(x => ({
        pageIndex: x.pageIndex,
        pageSize: x.pageSize,
        totalCount: x.totalCount,
        totalPages: Math.ceil(x.totalCount / x.pageSize)
      }))
    )
  }

  ngAfterViewInit() {
    this.loadItems$.next()
  }

  loadPage(pageIndex: number) {
    this.currentPage = pageIndex
    this.loadItems$.next()
  }

  ngOnDestroy() {
    this.loadItems$.complete()
  }

  acceptFriendRequest(requestId: string) {
    this.friendshipsClient.createFriendship(new FriendshipCreateDto({
      requestId
    })).subscribe(() => {
      this.loadItems$.next()
    })
  }

  rejectFriendRequest(requestId: string) {
    this.friendRequestsClient.deleteRequest(requestId).subscribe(() => {
      this.loadItems$.next()
    })
  }

}
