import { Component, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { BookingInterface } from '../../data/interfaces/booking-interface';
import { BookingService } from '../../data/services/booking.service';
import { BookingCardComponent } from "../../common-ui/booking-card/booking-card.component";
import { GroqService } from '../../data/services/groq.service';

@Component({
    selector: 'app-bookings-page',
    imports: [CommonModule, BookingCardComponent, RouterLink, FormsModule],
    templateUrl: './bookings-page.component.html',
    styleUrl: './bookings-page.component.scss'
})

export class BookingsPageComponent {
    bookings$!: Observable<BookingInterface[]>;
    bookingService = inject(BookingService);

    groqService = inject(GroqService);
    userQuestion: string = ''; // For question user wants to ask
    userInput: string = ''; // For input field binding
    aiResponse: string = ''; // For AI response to the question 
    isLoadingAiResponse: boolean = false; // Flag to show loading state

    constructor() {
        this.bookings$ = this.bookingService.getAllBookings();
    }

    sendQuestion() {
        this.aiResponse = "";
        this.userQuestion = this.userInput.trim();
        this.isLoadingAiResponse = true;

        this.groqService.sendQuestion(this.userQuestion).subscribe({
            next: (data) => {
                this.aiResponse = data.response;
                this.isLoadingAiResponse = false;
                console.log('AI Response:', this.aiResponse);
            }
        });
    }

    setQuestion(question: string) {
        this.userInput = question;
    }
}
