import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: 'tracker',
    loadChildren: () =>
      import('./features/tracker/tracker.module').then((m) => m.TrackerModule),
  },
];
