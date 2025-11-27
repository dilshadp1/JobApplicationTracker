import { Routes } from '@angular/router';
import { JobListComponent } from './job-list/job-list.component';
import { JobAddComponent } from './job-add/job-add.component';

export const trackerRoutes: Routes = [
    { path: '', component: JobListComponent },
    { path: 'add', component: JobAddComponent }
];