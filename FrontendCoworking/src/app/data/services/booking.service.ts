import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { BookingInterface } from '../interfaces/booking-interface';

@Injectable({
  providedIn: 'root'
})
    
export class BookingService {
  http: HttpClient = inject(HttpClient);
  baseUrl: string = 'https://localhost:7201/Bookings';
  constructor() { }

  getAllBookings() : Observable<BookingInterface[]> {
    return this.http.get<BookingInterface[]>(`${this.baseUrl}`);
  }

  getBookingById(id: number): Observable<BookingInterface> {
    return this.http.get<BookingInterface>(`${this.baseUrl}/${id}`);
  }

  createBooking(booking : BookingInterface): Observable<BookingInterface> {
    const bookingDto = {
      Name: booking.name,
      Email: booking.email,
      StartDate: booking.startDate,
      EndDate: booking.endDate,
      Workspace: booking.workspace,
      Availability: booking.availability
    };
    console.log('Payload to be sent:', bookingDto);

    return this.http.post<BookingInterface>(`${this.baseUrl}`, bookingDto);
  }

  deleteBooking(id: number) : Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }
}
