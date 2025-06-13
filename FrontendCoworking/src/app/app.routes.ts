import { Routes } from '@angular/router';
import { WorkspacesPageComponent } from './pages/workspaces-page/workspaces-page.component';
import { BookingsPageComponent } from './pages/bookings-page/bookings-page.component';
import { BookingFormComponent } from './pages/booking-form/booking-form.component';
import { CoworkingsPageComponent } from './pages/coworkings-page/coworkings-page.component';

export const routes: Routes = [
    { path: '', component: CoworkingsPageComponent },
    { path: 'bookings', component: BookingsPageComponent },
    { path: 'new-booking/:coworkingId', component: BookingFormComponent },
    { path: 'bookingform/:id', component: BookingFormComponent },
    { path: 'coworkings', component: CoworkingsPageComponent },
    { path: 'coworkingsDetail/:id', component: WorkspacesPageComponent }
];