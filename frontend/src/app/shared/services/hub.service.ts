import { Inject, Injectable } from '@angular/core'
import { API_BASE_URL, ConversationDetailsDto, ConversationListDto, FriendRequestDto, FriendshipDto, MessageDto } from '../clients'
import { HubConnection, HubConnectionBuilder, HubConnectionState } from '@microsoft/signalr'
import { OAuthService } from 'angular-oauth2-oidc'
import { Subject } from 'rxjs'

@Injectable({
  providedIn: 'root'
})
export class HubService {
  private connection: HubConnection

  private mFriendRequestCreated = new Subject<FriendRequestDto>()
  private mFriendRequestDeleted = new Subject<string>()
  private mFriendshipCreated = new Subject<FriendshipDto>()
  private mFriendshipDeleted = new Subject<string>()

  private mAddedToConversation = new Subject<ConversationListDto>()
  private mConversationUpdated = new Subject<ConversationDetailsDto>()
  private mConversationDeleted = new Subject<string>()
  private mMessageReceived = new Subject<{conversationId: string, message: MessageDto}>()

  get friendRequestCreated() { return this.mFriendRequestCreated.asObservable() }
  get friendRequestDeleted() { return this.mFriendRequestDeleted.asObservable() }
  get friendshipCreated() { return this.mFriendshipCreated.asObservable() }
  get friendshipDeleted() { return this.mFriendshipDeleted.asObservable() }

  get addedToConversation() { return this.mAddedToConversation.asObservable() }
  get conversationUpdated() { return this.mConversationUpdated.asObservable() }
  get conversationDeleted() { return this.mConversationDeleted.asObservable() }
  get messageReceived() { return this.mMessageReceived.asObservable() }

  constructor(
    @Inject(API_BASE_URL) private apiBaseUrl: string,
    private oauthService: OAuthService) {
  }

  async connect() {
    if (!this.connection) {
      this.connection = new HubConnectionBuilder()
        .withUrl(this.apiBaseUrl + '/notifications', {
          accessTokenFactory: () => this.oauthService.getAccessToken()
        }).withAutomaticReconnect()
        .build()

      this.connection.on('FriendRequestCreated', request => this.mFriendRequestCreated.next(request))
      this.connection.on('FriendRequestDeleted', requestId => this.mFriendRequestDeleted.next(requestId))
      this.connection.on('FriendshipCreated', friendship => this.mFriendshipCreated.next(friendship))
      this.connection.on('FriendshipDeleted', friendshipId => this.mFriendshipDeleted.next(friendshipId))

      this.connection.on('AddedToConversation', conversation => this.mAddedToConversation.next(conversation))
      this.connection.on('ConversationUpdated', conversation => this.mConversationUpdated.next(conversation))
      this.connection.on('ConversationDeleted', conversationId => this.mConversationDeleted.next(conversationId))
      this.connection.on('MessageReceived', (conversationId, message) => this.mMessageReceived.next({ conversationId, message }))
    }

    if (this.connection.state === HubConnectionState.Disconnected) {
      await this.connection.start()
    }
  }

  async disconnect() {
    if (this.connection.state === HubConnectionState.Connected) {
      await this.connection.stop()
    }
  }
}
