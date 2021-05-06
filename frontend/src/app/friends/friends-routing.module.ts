import { NgModule } from '@angular/core'
import { RouterModule, Routes } from '@angular/router'
import { FriendListComponent } from './components/friend-list/friend-list.component'
import { IncomingFriendRequestsComponent } from './components/incoming-friend-requests/incoming-friend-requests.component'
import { SentFriendRequestsComponent } from './components/sent-friend-requests/sent-friend-requests.component'

const routes: Routes = [
  {
    path: '',
    redirectTo: 'friend-list',
    pathMatch: 'full'
  },
  {
    path: 'friend-list',
    component: FriendListComponent
  },
  {
    path: 'incoming-requests',
    component: IncomingFriendRequestsComponent
  },
  {
    path: 'sent-requests',
    component: SentFriendRequestsComponent
  }
]

@NgModule({
  imports: [
    RouterModule.forChild(routes)
  ]
})
export class FriendsRoutingModule { }
