import { AfterViewInit, Component, OnDestroy, OnInit } from '@angular/core'
import { Subject, Observable } from 'rxjs'
import { FriendRequestDto, FriendRequestsClient, FriendshipCreateDto, FriendshipsClient } from '../../../shared/clients'
import { PagerModel } from '../../../shared/models/pager.model'
import { map, share, switchMap, tap } from 'rxjs/operators'
import { defaultPageSize } from '../../../shared/constants'

@Component({
  selector: 'app-sent-friend-requests',
  templateUrl: './sent-friend-requests.component.html',
  styleUrls: ['./sent-friend-requests.component.scss']
})
export class SentFriendRequestsComponent implements AfterViewInit, OnDestroy {
  currentPage = 0
  loadItems$ = new Subject<number>()
  requests$: Observable<FriendRequestDto[]>
  pager$: Observable<PagerModel>

  constructor(
    private friendRequestsClient: FriendRequestsClient
  ) {
    const response$ = this.loadItems$.pipe(
      switchMap(pageIndex => this.friendRequestsClient.listReceivedRequests(pageIndex, defaultPageSize)),
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
    this.loadItems$.next(this.currentPage)
  }

  loadPage(pageIndex: number) {
    this.currentPage = pageIndex
    this.loadItems$.next(pageIndex)
  }

  ngOnDestroy() {
    this.loadItems$.complete()
  }

  cancelFriendRequest(requestId: string) {
    this.friendRequestsClient.deleteRequest(requestId).subscribe(() => {
      this.loadItems$.next(this.currentPage)
    })
  }
}
