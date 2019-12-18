import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UnprotectedAppRoutes } from './unprotected-app.routing';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';

@NgModule({
  imports: [
    CommonModule,
    UnprotectedAppRoutes,
    ReactiveFormsModule,
    FormsModule
  ],
  declarations: [LoginComponent, RegisterComponent]
})
export class UnprotectedAppModule {}
