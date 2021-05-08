import { AfterViewInit, Component, OnDestroy } from '@angular/core'
import {  Observable, Subject } from 'rxjs'
import { FriendshipDto, FriendshipsClient } from '../../../shared/clients'
import { map, share, switchMap } from 'rxjs/operators'
import { PagerModel } from '../../../shared/models/pager.model'
import { defaultPageSize } from '../../../shared/constants'

@Component({
  selector: 'app-friend-list',
  templateUrl: './friend-list.component.html',
  styleUrls: ['./friend-list.component.scss']
})
export class FriendListComponent implements AfterViewInit, OnDestroy {
  currentPage = 0
  loadItems$ = new Subject<number>()
  friendships$: Observable<FriendshipDto[]>
  pager$: Observable<PagerModel>

  constructor(private client: FriendshipsClient) {
    const response$ = this.loadItems$.pipe(
      switchMap(pageIndex => client.listFriends(pageIndex, defaultPageSize)),
      share()
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
    console.log('ngAfterViewInit')
    this.loadItems$.next(this.currentPage)
  }

  loadPage(pageIndex: number) {
    this.currentPage = pageIndex
    console.log('loadPage')
    this.loadItems$.next(pageIndex)
  }

  deleteFriendship(friendshipId: string) {
    this.client.deleteFriendship(friendshipId).subscribe(() => {
      console.log('deleteFriendship')
      this.loadItems$.next(this.currentPage)
    })
  }

  ngOnDestroy() {
    this.loadItems$.complete()
  }
}
