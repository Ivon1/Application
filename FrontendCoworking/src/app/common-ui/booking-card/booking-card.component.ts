import { Component, inject, Input } from '@angular/core';
import { DatePipe } from '@angular/common';
import { RouterModule } from '@angular/router';
import { BookingInterface } from '../../data/interfaces/booking-interface';
import { TimeUtilService } from '../../data/services/time-util.service';
import { BookingService } from '../../data/services/booking.service';


@Component({
    selector: 'app-booking-card',
    imports: [DatePipe, RouterModule],
    templateUrl: './booking-card.component.html',
    styleUrl: './booking-card.component.scss'
})

export class BookingCardComponent {
    @Input() booking: BookingInterface | undefined;
    timeService: TimeUtilService = inject(TimeUtilService);
    bookingService: BookingService = inject(BookingService);

    deleteBooking(): void {
        if (!this.booking || !this.booking.id) {
            return;
        }


        if (confirm('Are you sure you want to delete this booking?')) {
            console.log('Deleting booking with ID:', this.booking.id);
            this.bookingService.deleteBooking(this.booking.id).subscribe({
                next: () => {
                    console.log('Booking deleted successfully');
                },
                error: (error) => {
                    console.error('Error deleting booking:', error);
                }
            });

            window.location.reload();
        }
    }

    getFormattedAmountBookedFor(bookedFor: string | null): string {
        
        const upToPattern = /for up to \d+ people/;
        const match = bookedFor?.match(upToPattern);
        if(match) {
            return match[0];
        } else {
            const roomsForPattern = /for \d+ (person|people)/;
            const roomsForMatch = bookedFor?.match(roomsForPattern);
            if (roomsForMatch) {
                return roomsForMatch[0];
            }
        }
        return "";
    }
}