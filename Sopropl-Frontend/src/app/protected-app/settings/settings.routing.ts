import { Routes, RouterModule } from '@angular/router';
import { ProfileComponent } from './profile/profile.component';
import { SettingsComponent } from './settings.component';
import { NgModule } from '@angular/core';
import { AccountComponent } from './account/account.component';
import { AuthGuard } from 'src/app/_guards/autht.guard';

@NgModule({
  imports: [
    RouterModule.forChild([
      {
        path: 'profile',
        component: ProfileComponent,
        data: { titile: 'Your Profile' }
      },
      {
        path: 'account',
        component: AccountComponent,
        data: { titile: 'Your Account' }
      }
    ])
  ],
  exports: [RouterModule]
})
export class SettingsRoutingModule { }
