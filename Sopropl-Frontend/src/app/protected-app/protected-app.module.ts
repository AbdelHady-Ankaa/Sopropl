import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ProtectedAppRoutes } from './protected-app.routing';
import { MaterialModule } from '../material.module';
import { SoproplCloudinaryModule } from '../sopropl-cloudinary.module';
import { HomeComponent } from './home/home.component';
import { SettingsComponent } from './settings/settings.component';
import { OrganizationsComponent } from './organizations/organizations.component';
import { NewOrgComponent } from './new-org/new-org.component';
import { ReactiveFormsModule } from '@angular/forms';

@NgModule({
  imports: [
    CommonModule,
    ProtectedAppRoutes,
    MaterialModule,
    SoproplCloudinaryModule,
    ReactiveFormsModule
  ],
  declarations: [
    HomeComponent,
    SettingsComponent,
    OrganizationsComponent,
    NewOrgComponent
  ]
})
export class ProtectedAppModule { }
