import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SettingsRoutingModule } from './settings.routing';
import { AccountComponent } from './account/account.component';
import { ProfileComponent } from './profile/profile.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MaterialModule } from 'src/app/material.module';
import { SoproplCloudinaryModule } from 'src/app/sopropl-cloudinary.module';

@NgModule({
  imports: [
    CommonModule,
    SettingsRoutingModule,
    MaterialModule,
    SoproplCloudinaryModule,
    FormsModule,
    ReactiveFormsModule
  ],
  declarations: [AccountComponent, ProfileComponent]
})
export class SettingsModule { }
