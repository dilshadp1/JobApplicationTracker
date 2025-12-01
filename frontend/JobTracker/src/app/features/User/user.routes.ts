import { Routes } from '@angular/router';
import { authGuard } from '../../core/auth/guards/auth.guard';
import { MainLayoutComponent } from '../../core/layout/main-layout/main-layout.component';

export const userRoutes: Routes = [
  {
    path: '',
    component: MainLayoutComponent, 
    canActivate: [authGuard],       
    children: [
      { 
        path: '', 
        redirectTo: 'dashboard', 
        pathMatch: 'full' 
      },
      { 
        path: 'dashboard', 
        loadComponent: () => import('./dashboard/dashboard.component').then(m => m.DashboardComponent),
        title: 'Dashboard'
      },
      {
        path: 'profile', 
        loadComponent: () => import('../profile/profile/profile.component').then(m => m.ProfileComponent),
        title:'My Profile'
      },
      { 
        path: 'jobs', 
        loadComponent: () => import('./job-list/job-list.component').then(m => m.JobListComponent),
        title: 'My Job Applications'
      },
      { 
        path: 'jobs/add', 
        loadComponent: () => import('./job-add/job-add.component').then(m => m.JobAddComponent),
        title: 'Add New Job'
      },
    ]
  }
];