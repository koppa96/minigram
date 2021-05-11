import { NgModule } from '@angular/core'
import { CommonModule } from '@angular/common';
import { ConversationListComponent } from './components/conversation-list/conversation-list.component';
import { ConversationComponent } from './components/conversation/conversation.component'
import { ConversationsRoutingModule } from './conversations-routing.module'
import {
  NbAutocompleteModule,
  NbButtonModule,
  NbCardModule,
  NbChatModule,
  NbDialogModule,
  NbIconModule,
  NbInputModule,
  NbListModule, NbToggleModule,
  NbUserModule
} from '@nebular/theme'
import { SharedModule } from '../shared/shared.module'
import { LastMessagePipe } from './pipes/last-message.pipe';
import { NewConversationComponent } from './components/new-conversation/new-conversation.component'
import { ReactiveFormsModule } from '@angular/forms'
import { RouterModule } from '@angular/router';
import { ConversationDetailsComponent } from './components/conversation-details/conversation-details.component'
import { IsAdminPipe } from './pipes/is-admin.pipe'
import { AnyOtherAdminsPipe } from './pipes/any-other-admins.pipe';
import { NewMemberComponent } from './components/new-member/new-member.component'

@NgModule({
  declarations: [
    ConversationListComponent,
    ConversationComponent,
    LastMessagePipe,
    NewConversationComponent,
    ConversationDetailsComponent,
    IsAdminPipe,
    AnyOtherAdminsPipe,
    NewMemberComponent
  ],
  imports: [
    CommonModule,
    ConversationsRoutingModule,
    NbCardModule,
    NbIconModule,
    NbListModule,
    NbUserModule,
    SharedModule,
    NbButtonModule,
    NbInputModule,
    ReactiveFormsModule,
    NbDialogModule,
    RouterModule,
    NbChatModule,
    NbToggleModule,
    NbAutocompleteModule
  ]
})
export class ConversationsModule { }
