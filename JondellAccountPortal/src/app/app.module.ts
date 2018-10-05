import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { CustomMaterialModule } from './core/material.module';
import {FormsModule} from '@angular/forms';

import { AppComponent } from './app.component';
import { LoginComponent } from './login/login.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { AppRoutingModule } from './app-routing.module';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { AppService } from 'src/app/app.service';
import { HttpClientModule } from '@angular/common/http';
import { AuthGuard } from 'src/app/auth-guard.service';
import { JwtHelperService, JwtModule } from '@auth0/angular-jwt';
import { MatGridList, MatGridTile } from '@angular/material';
import { BalanceViewerComponent } from './balance-viewer/balance-viewer.component';
import { FileUploadComponent } from './file-upload/file-upload.component';
import { AngularFileUploaderModule } from "angular-file-uploader";
import { AccountHistoryComponent } from './account-history/account-history.component';
import { ChartsModule } from 'ng2-charts';

export function tokenGetter() {
  return localStorage.getItem('access_token');
}

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    DashboardComponent,
    MatGridTile,
    MatGridList,
    BalanceViewerComponent,
    FileUploadComponent,
    AccountHistoryComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    CustomMaterialModule,
    HttpClientModule,
    FormsModule,
    AppRoutingModule,
    FormsModule,
    BrowserAnimationsModule,
    AngularFileUploaderModule,
    ChartsModule,
    JwtModule.forRoot({
      config: {
        tokenGetter: tokenGetter,
        whitelistedDomains: ['localhost:4200'],
        blacklistedRoutes: ['localhost:4200/auth/']
      }
    })
  ],
  providers: [AppService,AuthGuard],
  bootstrap: [AppComponent]
})
export class AppModule { }
