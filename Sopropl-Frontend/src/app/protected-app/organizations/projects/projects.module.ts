import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ProjHomeComponent } from './proj-home/proj-home.component';
import { ProjectsRoutes } from './projects.routing';
import { GraphComponent } from './graph/graph.component';
import { AccessListComponent } from './access-list/access-list.component';
import { ActivitiesComponent } from './activities/activities.component';
import { GanttComponent } from './gantt/gantt.component';
import { MaterialModule } from 'src/app/material.module';

@NgModule({
  imports: [
    CommonModule,
    ProjectsRoutes,
    MaterialModule
  ],
  declarations: [ProjHomeComponent, GraphComponent, AccessListComponent,
    ActivitiesComponent, GanttComponent]
})
export class ProjectsModule { }
