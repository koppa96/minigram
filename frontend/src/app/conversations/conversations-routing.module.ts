import { NgModule } from '@angular/core'
import { RouterModule, Routes } from '@angular/router'
import { ConversationListComponent } from './components/conversation-list/conversation-list.component'
import { ConversationComponent } from './components/conversation/conversation.component'

const routes: Routes = [
  {
    path: '',
    redirectTo: 'list',
    pathMatch: 'full'
  },
  {
    path: 'list',
    component: ConversationListComponent
  },
  {
    path: ':id',
    component: ConversationComponent
  }
]

@NgModule({
  imports: [
    RouterModule.forChild(routes)
  ]
})
export class ConversationsRoutingModule { }
