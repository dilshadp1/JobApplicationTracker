import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: 'login',
    loadComponent: () =>
      import('./core/auth/pages/login/login.component').then(
        (m) => m.LoginComponent
      ),
  },
  {
    path: 'register',
    loadComponent: () =>
      import('./core/auth/pages/register-user/register-user.component').then(
        (m) => m.RegisterUserComponent
      ),
  },
  {
    path: 'user',
    loadChildren: () =>
      import('./features/user/user.routes').then((m) => m.userRoutes),
  },
];
