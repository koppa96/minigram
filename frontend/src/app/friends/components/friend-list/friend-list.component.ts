import { AfterViewInit, Component, OnDestroy, OnInit } from '@angular/core'
import { BehaviorSubject, Observable, of, Subject } from 'rxjs'
import { FriendshipDto, FriendshipsClient, UserListDto } from '../../../shared/clients'
import { map, switchMap } from 'rxjs/operators'
import { PagerModel } from '../../../shared/models/pager.model'
import { defaultPageSize } from '../../../shared/constants'

@Component({
  selector: 'app-friend-list',
  templateUrl: './friend-list.component.html',
  styleUrls: ['./friend-list.component.scss']
})
export class FriendListComponent implements AfterViewInit, OnDestroy {

  loadItems$ = new BehaviorSubject<number>(0)
  friendships$: Observable<FriendshipDto[]>
  pager$: Observable<PagerModel>

  constructor(private client: FriendshipsClient) {
    const response$ = this.loadItems$.pipe(
      switchMap(pageIndex => client.listFriends(pageIndex, defaultPageSize))
    )

    this.friendships$ = response$.pipe(
      map(x => x.data)
    )

    this.pager$ = response$.pipe(
      map(x => ({
        pageIndex: x.pageIndex,
        pageSize: x.pageSize,
        totalCount: x.totalCount,
        totalPages: Math.floor(x.totalCount / x.pageSize)
      }))
    )
  }

  ngAfterViewInit() {
    this.loadItems$.next(0)
  }

  loadPage(pageIndex: number) {
    this.loadItems$.next(pageIndex)
  }

  deleteFriendship(friendshipId: string) {
    this.client.deleteFriendship(friendshipId).subscribe(() => {
      this.loadItems$.next(this.loadItems$.value)
    })
  }

  ngOnDestroy() {
    this.loadItems$.complete()
  }
}
