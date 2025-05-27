import { Routes } from '@angular/router';
import { WorkspacesPageComponent } from './pages/workspaces-page/workspaces-page.component';
import { BookingsPageComponent } from './pages/bookings-page/bookings-page.component';

export const routes: Routes = [
    { path: '', component: WorkspacesPageComponent },
    { path: 'bookings', component: BookingsPageComponent }
];
