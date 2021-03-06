import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { ArticlesListComponent } from './articles-list/articles-list.component';
import { ArticlesCreateComponent } from './articles-create/articles-create.component';
import { ArticlesReadComponent } from './articles-read/articles-read.component';
import { ArticlesNavigationComponent } from './articles-navigation/articles-navigation.component';
import { ArticlesScoreComponent } from './articles-score/articles-score.component';
import { ApiAuthorizationModule } from 'src/api-authorization/api-authorization.module';
import { AuthorizeGuard } from 'src/api-authorization/authorize.guard';
import { AuthorizeInterceptor } from 'src/api-authorization/authorize.interceptor';
import { HttpClientService } from './shared/services/http-client.service';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    ArticlesListComponent,
    ArticlesCreateComponent,
    ArticlesReadComponent,
    ArticlesNavigationComponent,
    ArticlesScoreComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    ApiAuthorizationModule,
    NgbModule,
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full' },
      { path: 'article/:id', component: ArticlesReadComponent, pathMatch: 'full', canActivate: [AuthorizeGuard] }
    ])
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: AuthorizeInterceptor, multi: true },
    HttpClientService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
