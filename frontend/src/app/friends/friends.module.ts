import { NgModule } from '@angular/core'
import { CommonModule } from '@angular/common';
import { FriendListComponent } from './components/friend-list/friend-list.component';
import { IncomingFriendRequestsComponent } from './components/incoming-friend-requests/incoming-friend-requests.component';
import { SentFriendRequestsComponent } from './components/sent-friend-requests/sent-friend-requests.component'
import { FriendsRoutingModule } from './friends-routing.module'
import { NbButtonModule, NbCardModule, NbIconModule, NbListModule, NbUserModule } from '@nebular/theme'
import { SharedModule } from '../shared/shared.module'

@NgModule({
  declarations: [
    FriendListComponent,
    IncomingFriendRequestsComponent,
    SentFriendRequestsComponent
  ],
  imports: [
    CommonModule,
    FriendsRoutingModule,
    NbCardModule,
    NbListModule,
    NbUserModule,
    NbButtonModule,
    NbIconModule,
    SharedModule
  ]
})
export class FriendsModule { }
