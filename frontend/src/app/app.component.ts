import { Component, OnInit } from '@angular/core'
import { OAuthService } from 'angular-oauth2-oidc'
import { ConversationsClient } from './shared/clients'
import { NbMenuItem } from '@nebular/theme'

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
    private conversationsClient: ConversationsClient
  ) { }

  ngOnInit() {
    if (!this.oauthService.hasValidAccessToken()) {
      this.oauthService.initCodeFlow()
    }

    this.conversationsClient.listConversations(0, 25).subscribe(x => console.log(x))
  }
}
