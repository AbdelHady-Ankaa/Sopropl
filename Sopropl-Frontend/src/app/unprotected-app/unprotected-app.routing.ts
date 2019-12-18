import { Routes, RouterModule } from '@angular/router';
import { RegisterComponent } from './register/register.component';
import { LoginComponent } from './login/login.component';
import { UnprotectedAppComponent } from './unprotected-app.component';

const routes: Routes = [
  { path: 'login', component: LoginComponent, data: { title: 'login' } },
  {
    path: 'register',
    component: RegisterComponent,
    data: { title: 'Register' }
  },
  // { path: '**', redirectTo: 'login' }
];

export const UnprotectedAppRoutes = RouterModule.forChild(routes);
