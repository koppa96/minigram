import { NgModule } from '@angular/core'
import { CommonModule } from '@angular/common';
import { ConversationListComponent } from './components/conversation-list/conversation-list.component';
import { ConversationComponent } from './components/conversation/conversation.component'
import { ConversationsRoutingModule } from './conversations-routing.module'

@NgModule({
  declarations: [
    ConversationListComponent,
    ConversationComponent
  ],
  imports: [
    CommonModule,
    ConversationsRoutingModule
  ]
})
export class ConversationsModule { }
