import { Component, OnInit } from '@angular/core'
import { OAuthService } from 'angular-oauth2-oidc'
import { NbMenuItem } from '@nebular/theme'
import { HubService } from './shared/services/hub.service'

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  title = 'minigram'
  menuItems: NbMenuItem[] = [
    {
      title: 'Beszélgetések',
      icon: 'paper-plane-outline',
      link: '/conversations/list'
    },
    {
      title: 'Barátlista',
      icon: 'people-outline',
      link: '/friends/friend-list'
    },
    {
      title: 'Bejövő barátkérelmek',
      icon: 'person-done-outline',
      link: '/friends/incoming-requests'
    },
    {
      title: 'Kimenő barátkérelmek',
      icon: 'person-add-outline',
      link: '/friends/sent-requests'
    }
  ]

  constructor(
    private oauthService: OAuthService,
    private hubService: HubService
  ) { }

  async ngOnInit() {
    if (!this.oauthService.hasValidAccessToken()) {
      this.oauthService.initCodeFlow()
    }

    await this.hubService.connect()
    this.hubService.friendRequestCreated$.subscribe(x => console.log('Friend request created', x))
    this.hubService.friendRequestDeleted$.subscribe(x => console.log('Friend request deleted', x))
    this.hubService.friendshipCreated$.subscribe(x => console.log('Friendship created', x))
    this.hubService.friendshipDeleted$.subscribe(x => console.log('Friendship deleted', x))

    this.hubService.conversationUpdated$.subscribe(x => console.log('Conversation updated', x))
    this.hubService.removedFromConversation$.subscribe(x => console.log('Conversation deleted', x))
    this.hubService.addedToConversation$.subscribe(x => console.log('Added to conversation', x))
    this.hubService.messageReceived$.subscribe(x => console.log('Message received', x))
  }

  logout() {
    this.oauthService.logOut()
  }
}
