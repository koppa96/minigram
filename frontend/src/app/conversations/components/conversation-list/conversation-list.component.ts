import { AfterViewInit, Component } from '@angular/core'
import { merge, Observable, Subject } from 'rxjs'
import {
  ConversationCreateEditDto,
  ConversationsClient,
  PagedListDtoOfConversationListDto
} from '../../../shared/clients'
import { NbDialogService } from '@nebular/theme'
import { NewConversationComponent } from '../new-conversation/new-conversation.component'
import { HubService } from '../../../shared/services/hub.service'
import { switchMap } from 'rxjs/operators'
import { defaultPageSize } from '../../../shared/constants'

@Component({
  selector: 'app-conversation-list',
  templateUrl: './conversation-list.component.html',
  styleUrls: ['./conversation-list.component.scss']
})
export class ConversationListComponent implements AfterViewInit {
  currentPageIndex = 0
  loadItems$ = new Subject<void>()
  conversations$: Observable<PagedListDtoOfConversationListDto>

  constructor(
    private dialog: NbDialogService,
    private conversationsClient: ConversationsClient,
    private hubService: HubService
  ) {
    this.conversations$ = merge(
      this.loadItems$,
      this.hubService.addedToConversation$,
      this.hubService.conversationUpdated$,
      this.hubService.removedFromConversation$
    ).pipe(
      switchMap(() => this.conversationsClient.listConversations(this.currentPageIndex, defaultPageSize)),
    )
  }

  ngAfterViewInit() {
    this.loadItems$.next()
  }

  openNewConversationDialog() {
    this.dialog.open(NewConversationComponent).onClose.subscribe(result => {
      if (result) {
        this.conversationsClient.createConversation(new ConversationCreateEditDto({
          name: result.conversationName
        })).subscribe(() => {
          this.loadItems$.next()
        })
      }
    })
  }

  loadPage(pageIndex: number) {

  }
}
