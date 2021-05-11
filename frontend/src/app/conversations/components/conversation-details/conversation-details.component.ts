import { Component, OnDestroy } from '@angular/core'
import {
  ConversationDetailsDto,
  ConversationMembershipDto, ConversationMembershipEditDto,
  ConversationMembershipsClient,
  ConversationsClient
} from '../../../shared/clients'
import { NbDialogRef } from '@nebular/theme'
import { HubService } from '../../../shared/services/hub.service'
import { Subject } from 'rxjs'
import { takeUntil } from 'rxjs/operators'

@Component({
  selector: 'app-conversation-details',
  templateUrl: './conversation-details.component.html',
  styleUrls: ['./conversation-details.component.scss']
})
export class ConversationDetailsComponent implements OnDestroy {
  destroy$ = new Subject<void>()
  conversation: ConversationDetailsDto
  currentUserId: string

  constructor(
    private membershipsClient: ConversationMembershipsClient,
    private conversationsClient: ConversationsClient,
    private dialogRef: NbDialogRef<ConversationDetailsComponent>,
    private hubService: HubService
  ) {
    hubService.conversationUpdated$.pipe(
      takeUntil(this.destroy$)
    ).subscribe(conversation => {
      this.conversation = conversation
    })
  }

  changeStatus(membership: ConversationMembershipDto) {
    this.membershipsClient.updateMember(this.conversation.id, membership.id, new ConversationMembershipEditDto({
      isAdmin: membership.isAdmin
    })).subscribe(() => {
      // Dirty hack to trigger pipe rerun
      this.conversation = new ConversationDetailsDto(this.conversation)
    })
  }

  close() {
    this.dialogRef.close()
  }

  removeMember(membership: ConversationMembershipDto) {
    this.membershipsClient.deleteMember(this.conversation.id, membership.id).subscribe(() => {
      this.conversation.memberships.splice(this.conversation.memberships.indexOf(membership), 1)
      this.conversation = new ConversationDetailsDto(this.conversation)
    })
  }

  ngOnDestroy() {
    this.destroy$.next()
    this.destroy$.complete()
  }
}
