import { AfterViewInit, Component, OnDestroy } from '@angular/core'
import { Subject, Observable, merge } from 'rxjs'
import {
  FriendRequestCreateDto,
  FriendRequestsClient,
  PagedListDtoOfFriendRequestDto
} from '../../../shared/clients'
import { switchMap } from 'rxjs/operators'
import { defaultPageSize } from '../../../shared/constants'
import { HubService } from '../../../shared/services/hub.service'
import { NbDialogService, NbToastrService } from '@nebular/theme'
import { NewFriendComponent } from '../new-friend/new-friend.component'

@Component({
  selector: 'app-sent-friend-requests',
  templateUrl: './sent-friend-requests.component.html',
  styleUrls: ['./sent-friend-requests.component.scss']
})
export class SentFriendRequestsComponent implements AfterViewInit, OnDestroy {
  currentPage = 0
  loadItems$ = new Subject<void>()
  requests$: Observable<PagedListDtoOfFriendRequestDto>

  constructor(
    private friendRequestsClient: FriendRequestsClient,
    private hubService: HubService,
    private dialog: NbDialogService,
    private toast: NbToastrService
  ) {
    this.requests$ = merge(
      this.loadItems$,
      hubService.friendRequestDeleted$
    ).pipe(
      switchMap(() => this.friendRequestsClient.listSentRequests(this.currentPage, defaultPageSize))
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

  cancelFriendRequest(requestId: string) {
    this.friendRequestsClient.deleteRequest(requestId).subscribe(() => {
      this.loadItems$.next()
    })
  }

  openNewFriendDialog() {
    this.dialog.open(NewFriendComponent).onClose.subscribe(recipientId => {
      if (recipientId) {
        this.friendRequestsClient.sendRequest(new FriendRequestCreateDto({
          recipientId
        })).subscribe(() => {
          this.loadItems$.next()
          this.toast.success('Barátkérelem sikeresen elküldve', 'Siker')
        })
      }
    })
  }
}
