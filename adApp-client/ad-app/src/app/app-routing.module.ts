import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './components/login/login.component';
import { RegisterComponent } from './components/register/register.component';
import { AdsComponent } from './components/ads/ads.component';
import { EditAdComponent } from './components/ads/edit-ad/edit-ad.component';

const routes: Routes = [
  { path: 'login', component: LoginComponent },
  { path: 'ads', component: AdsComponent },
   { path: 'edit-ad/:id', component: EditAdComponent },
  { path: '**', redirectTo: 'login' },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
