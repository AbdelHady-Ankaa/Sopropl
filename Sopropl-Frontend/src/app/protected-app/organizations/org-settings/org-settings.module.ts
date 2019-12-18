import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { OrgSettingsComponent } from './org-settings.component';
import { OrgProfileComponent } from './org-profile/org-profile.component';
import { OrgSettingsRoutes } from './org-settings.routing';

@NgModule({
  imports: [
    CommonModule,
    OrgSettingsRoutes
  ],
  declarations: [OrgProfileComponent]
})
export class OrgSettingsModule { }
