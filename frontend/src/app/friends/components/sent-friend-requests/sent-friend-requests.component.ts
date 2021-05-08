import { AfterViewInit, Component, OnDestroy, OnInit } from '@angular/core'
import { BehaviorSubject, Observable } from 'rxjs'
import { FriendRequestDto, FriendRequestsClient, FriendshipCreateDto, FriendshipsClient } from '../../../shared/clients'
import { PagerModel } from '../../../shared/models/pager.model'
import { map, switchMap, tap } from 'rxjs/operators'
import { defaultPageSize } from '../../../shared/constants'

@Component({
  selector: 'app-sent-friend-requests',
  templateUrl: './sent-friend-requests.component.html',
  styleUrls: ['./sent-friend-requests.component.scss']
})
export class SentFriendRequestsComponent implements AfterViewInit, OnDestroy {

  loadItems$ = new BehaviorSubject<number>(0)
  requests$: Observable<FriendRequestDto[]>
  pager$: Observable<PagerModel>

  constructor(
    private friendRequestsClient: FriendRequestsClient
  ) {
    const response$ = this.loadItems$.pipe(
      switchMap(pageIndex => this.friendRequestsClient.listReceivedRequests(pageIndex, defaultPageSize))
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
    this.loadItems$.next(0)
  }

  loadPage(pageIndex: number) {
    this.loadItems$.next(pageIndex)
  }

  ngOnDestroy() {
    this.loadItems$.complete()
  }

  cancelFriendRequest(requestId: string) {
    this.friendRequestsClient.deleteRequest(requestId).subscribe(() => {
      this.loadItems$.next(this.loadItems$.value)
    })
  }
}
