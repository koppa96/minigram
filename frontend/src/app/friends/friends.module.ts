import { NgModule } from '@angular/core'
import { CommonModule } from '@angular/common';
import { FriendListComponent } from './components/friend-list/friend-list.component';
import { IncomingFriendRequestsComponent } from './components/incoming-friend-requests/incoming-friend-requests.component';
import { SentFriendRequestsComponent } from './components/sent-friend-requests/sent-friend-requests.component'
import { FriendsRoutingModule } from './friends-routing.module'
import {
  NbAutocompleteModule,
  NbButtonModule,
  NbCardModule,
  NbDialogModule,
  NbIconModule, NbInputModule,
  NbListModule, NbToastrModule,
  NbUserModule
} from '@nebular/theme'
import { SharedModule } from '../shared/shared.module';
import { NewFriendComponent } from './components/new-friend/new-friend.component'
import { ReactiveFormsModule } from '@angular/forms'

@NgModule({
  declarations: [
    FriendListComponent,
    IncomingFriendRequestsComponent,
    SentFriendRequestsComponent,
    NewFriendComponent
  ],
  imports: [
    CommonModule,
    FriendsRoutingModule,
    NbCardModule,
    NbListModule,
    NbUserModule,
    NbButtonModule,
    NbIconModule,
    SharedModule,
    NbDialogModule.forChild(),
    ReactiveFormsModule,
    NbAutocompleteModule,
    NbInputModule,
    NbToastrModule
  ]
})
export class FriendsModule { }
