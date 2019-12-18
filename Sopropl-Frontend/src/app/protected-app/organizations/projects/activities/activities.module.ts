import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActHomeComponent } from './act-home/act-home.component';
import { ResourcesComponent } from './resources/resources.component';
import { ActivitiesRoutes } from './activities.routing';

@NgModule({
  imports: [
    CommonModule,
    ActivitiesRoutes
  ],
  declarations: [ActHomeComponent, ResourcesComponent]
})
export class ActivitiesModule { }
