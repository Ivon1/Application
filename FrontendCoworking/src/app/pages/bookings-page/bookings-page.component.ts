import { Component, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { BookingInterface } from '../../data/interfaces/booking-interface';
import { BookingService } from '../../data/services/booking.service';
import { BookingCardComponent } from "../../common-ui/booking-card/booking-card.component";


@Component({
  selector: 'app-bookings-page',
  imports: [CommonModule, BookingCardComponent, RouterLink],
  templateUrl: './bookings-page.component.html',
  styleUrl: './bookings-page.component.scss'
})

export class BookingsPageComponent {
  bookings$!: Observable<BookingInterface[]>;
  bookingService = inject(BookingService);

  constructor() {
    this.bookings$ = this.bookingService.getAllBookings();
  }
}
