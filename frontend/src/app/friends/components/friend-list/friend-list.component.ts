import { AfterViewInit, Component, OnDestroy } from '@angular/core'
import { merge, Observable, Subject } from 'rxjs'
import { FriendRequestCreateDto, FriendRequestsClient, FriendshipDto, FriendshipsClient } from '../../../shared/clients'
import { map, share, switchMap } from 'rxjs/operators'
import { PagerModel } from '../../../shared/models/pager.model'
import { defaultPageSize } from '../../../shared/constants'
import { HubService } from '../../../shared/services/hub.service'
import { NbDialogService, NbToastrService } from '@nebular/theme'
import { NewFriendComponent } from '../new-friend/new-friend.component'

@Component({
  selector: 'app-friend-list',
  templateUrl: './friend-list.component.html',
  styleUrls: ['./friend-list.component.scss']
})
export class FriendListComponent implements AfterViewInit, OnDestroy {
  currentPage = 0
  loadItems$ = new Subject<void>()
  friendships$: Observable<FriendshipDto[]>
  pager$: Observable<PagerModel>

  constructor(
    private client: FriendshipsClient,
    private hubService: HubService,
    private dialog: NbDialogService,
    private friendRequestsClient: FriendRequestsClient,
    private toast: NbToastrService
  ) {
    const response$ = merge(
      this.loadItems$,
      hubService.friendshipCreated$,
      hubService.friendshipDeleted$
    ).pipe(
      switchMap(() => client.listFriends(null, this.currentPage, defaultPageSize)),
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
    this.loadItems$.next()
  }

  loadPage(pageIndex: number) {
    this.currentPage = pageIndex
    this.loadItems$.next()
  }

  deleteFriendship(friendshipId: string) {
    this.client.deleteFriendship(friendshipId).subscribe(() => {
      this.loadItems$.next()
    })
  }

  openNewFriendDialog() {
    this.dialog.open(NewFriendComponent).onClose.subscribe(recipientId => {
      if (recipientId) {
        this.friendRequestsClient.sendRequest(new FriendRequestCreateDto({
          recipientId
        })).subscribe(() => {
          this.toast.success('Barátkérelem sikeresen elküldve', 'Siker')
        })
      }
    })
  }

  ngOnDestroy() {
    this.loadItems$.complete()
  }
}
