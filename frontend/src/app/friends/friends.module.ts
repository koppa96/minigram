import { NgModule } from '@angular/core'
import { CommonModule } from '@angular/common';
import { FriendListComponent } from './components/friend-list/friend-list.component';
import { IncomingFriendRequestsComponent } from './components/incoming-friend-requests/incoming-friend-requests.component';
import { SentFriendRequestsComponent } from './components/sent-friend-requests/sent-friend-requests.component'
import { FriendsRoutingModule } from './friends-routing.module'

@NgModule({
  declarations: [
    FriendListComponent,
    IncomingFriendRequestsComponent,
    SentFriendRequestsComponent
  ],
  imports: [
    CommonModule,
    FriendsRoutingModule
  ]
})
export class FriendsModule { }
