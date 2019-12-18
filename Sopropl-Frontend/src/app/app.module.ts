import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';

import { JwtModule } from '@auth0/angular-jwt';
import { HttpClientModule } from '@angular/common/http';

import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MaterialModule } from './material.module';

import { ErrorInterceptorProvider } from './_interceptors/error.interceptor';
import { ImageCropperModule } from 'ngx-image-cropper';

import { SoproplCloudinaryModule } from './sopropl-cloudinary.module';
import { SpinnerInterceptorProvider } from './_interceptors/spinner.interceptor';
import { tokenGetter } from './_services/auth.service';
import { TimeAgoPipe } from 'time-ago-pipe';
import { ImgCropperComponent } from './protected-app/settings/profile/img-cropper/img-cropper.component';
import { UnprotectedAppComponent } from './unprotected-app/unprotected-app.component';
import { ProtectedAppComponent } from './protected-app/protected-app.component';
import { NavComponent } from './protected-app/nav/nav.component';
import { LoginComponent } from './unprotected-app/login/login.component';
import { RegisterComponent } from './unprotected-app/register/register.component';

@NgModule({
  declarations: [
    AppComponent,
    ImgCropperComponent,
    TimeAgoPipe,
    UnprotectedAppComponent,
    ProtectedAppComponent,
    NavComponent,
    // LoginComponent,
    // RegisterComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule,
    BrowserAnimationsModule,
    JwtModule.forRoot({
      config: {
        tokenGetter,
        whitelistedDomains: ['localhost:5000']
      }
    }),
    MaterialModule,
    ImageCropperModule,
    SoproplCloudinaryModule
  ],
  providers: [ErrorInterceptorProvider, SpinnerInterceptorProvider],
  bootstrap: [AppComponent],
  exports: [TimeAgoPipe],
  entryComponents: [ImgCropperComponent]
})
export class AppModule { }
