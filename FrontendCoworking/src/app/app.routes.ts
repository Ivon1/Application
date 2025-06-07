import { Routes } from '@angular/router';
import { WorkspacesPageComponent } from './pages/workspaces-page/workspaces-page.component';
import { BookingsPageComponent } from './pages/bookings-page/bookings-page.component';
import { BookingFormComponent } from './pages/booking-form/booking-form.component';

export const routes: Routes = [
    { path: '', component: WorkspacesPageComponent },
    { path: 'bookings', component: BookingsPageComponent },
    { path: 'bookingform', component: BookingFormComponent },
    { path: 'bookingform/:id', component: BookingFormComponent }
];