import { Component, OnDestroy } from '@angular/core'
import { of, Subject } from 'rxjs'
import {
  ConversationDetailsDto, ConversationMembershipCreateDto,
  ConversationsClient,
  MessageDto,
  MessagesClient,
} from '../../../shared/clients'
import { ActivatedRoute } from '@angular/router'
import { switchMap, takeUntil } from 'rxjs/operators'
import { OAuthService } from 'angular-oauth2-oidc'
import { HubService } from '../../../shared/services/hub.service'
import { NbDialogService } from '@nebular/theme'
import { ConversationDetailsComponent } from '../conversation-details/conversation-details.component'
import { NewMemberComponent } from '../new-member/new-member.component'

@Component({
  selector: 'app-conversation',
  templateUrl: './conversation.component.html',
  styleUrls: ['./conversation.component.scss']
})
export class ConversationComponent implements OnDestroy {
  destroy$ = new Subject<void>()
  currentUserId: string
  conversation: ConversationDetailsDto
  messages: MessageDto[] = []

  constructor(
    private conversationsClient: ConversationsClient,
    private route: ActivatedRoute,
    private oauthService: OAuthService,
    private messagesClient: MessagesClient,
    private hubService: HubService,
    private dialog: NbDialogService
  ) {
    route.params.pipe(
      takeUntil(this.destroy$),
      switchMap(params => this.conversationsClient.getConversationDetails(params.id))
    ).subscribe(details => {
      this.conversation = details
    })

    route.params.pipe(
      takeUntil(this.destroy$)
    ).subscribe(params => {
      this.messagesClient.listMessages(params.id, 0, 10000).subscribe(messages => {
        this.messages = messages.data.reverse()
      })
    })

    hubService.messageReceived$.pipe(
      takeUntil(this.destroy$)
    ).subscribe(messageReceive => {
      if (route.snapshot.params.id === messageReceive.conversationId) {
        this.messages.push(messageReceive.message)
      }
    })

    hubService.conversationUpdated$.pipe(
      takeUntil(this.destroy$)
    ).subscribe(conversation => {
      this.conversation = conversation
    })

    this.currentUserId = (this.oauthService.getIdentityClaims() as any).sub
  }

  openDetails() {
    this.dialog.open(ConversationDetailsComponent, {
      context: {
        conversation: this.conversation,
        currentUserId: this.currentUserId
      }
    })
  }

  ngOnDestroy() {
    this.destroy$.next()
    this.destroy$.complete()
  }

  sendMessage($event: { message: string; files: File[] }) {
    this.messagesClient.sendMessage(this.route.snapshot.params.id, $event.message).subscribe(message => {
      this.messages.push(message)
    })
  }

  openAddMember() {
    this.dialog.open(NewMemberComponent).onClose.subscribe(result => {
      if (result) {
        this.conversationsClient.addMember(this.conversation.id, new ConversationMembershipCreateDto({
          userId: result
        })).subscribe(membership => {
          this.conversation.memberships.push(membership)
        })
      }
    })
  }
}
