import { APP_INITIALIZER, NgModule } from '@angular/core'
import { BrowserModule } from '@angular/platform-browser'

import { AppRoutingModule } from './app-routing.module'
import { AppComponent } from './app.component'
import { BrowserAnimationsModule } from '@angular/platform-browser/animations'
import { NbThemeModule, NbLayoutModule, NbSidebarModule, NbMenuModule, NbDialogModule, NbToastrModule } from '@nebular/theme'
import { NbEvaIconsModule } from '@nebular/eva-icons'
import { OAuthModule, OAuthService } from 'angular-oauth2-oidc'
import { HttpClientModule } from '@angular/common/http'
import { API_BASE_URL } from './shared/clients'
import { ReactiveFormsModule } from '@angular/forms'

export function initializeApp(
  oauthService: OAuthService
) {
  return async () => {
    oauthService.configure({
      clientId: 'minigram-angular',
      issuer: 'https://localhost:5101',
      postLogoutRedirectUri: window.location.origin,
      redirectUri: window.location.origin,
      requireHttps: true,
      responseType: 'code',
      scope: 'openid Conversations.Read Conversations.Manage Friendships.Read Friendships.Manage',
      useSilentRefresh: true,
      skipIssuerCheck: true
    })
    oauthService.setupAutomaticSilentRefresh()
    return oauthService.loadDiscoveryDocumentAndTryLogin()
  }
}

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    NbThemeModule.forRoot({name: 'cosmic'}),
    NbLayoutModule,
    NbEvaIconsModule,
    NbSidebarModule.forRoot(),
    HttpClientModule,
    OAuthModule.forRoot({
      resourceServer: {
        allowedUrls: ['https://localhost:5001'],
        sendAccessToken: true
      }
    }),
    NbMenuModule.forRoot(),
    NbDialogModule.forRoot({
      autoFocus: false
    }),
    NbToastrModule.forRoot(),
    ReactiveFormsModule
  ],
  providers: [
    {
      provide: APP_INITIALIZER,
      useFactory: initializeApp,
      deps: [OAuthService],
      multi: true
    },
    {
      provide: API_BASE_URL,
      useValue: 'https://localhost:5001'
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
