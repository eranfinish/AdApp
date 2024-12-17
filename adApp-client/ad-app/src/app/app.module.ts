import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { ReactiveFormsModule } from '@angular/forms'; // Import ReactiveFormsModule
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { LoginComponent } from './components/login/login.component';
import { AdsComponent } from './components/ads/ads.component';
import { AuthService } from './services/auth.service';
import { HttpClientModule } from '@angular/common/http';
import { RegisterComponent } from './components/register/register.component';
import { FormsModule } from '@angular/forms';
import { JwtInterceptor } from './interceptors/jwt.interceptor';
import { AdsService} from './services/ads.service';
import { AdComponent } from './components/ads/ad/ad.component';
import { EditAdComponent } from './components/ads/edit-ad/edit-ad.component';

@NgModule({
  declarations: [
  AdsComponent,
    AppComponent,
    LoginComponent,
    RegisterComponent,
    AdComponent,
    EditAdComponent
    //AdsComponent
  ],

  imports: [
    BrowserModule,
    AppRoutingModule,
    ReactiveFormsModule,
    HttpClientModule,
    FormsModule
  ],
  providers: [AuthService, AdsService, JwtInterceptor],
  bootstrap: [AppComponent],
})
export class AppModule {}
