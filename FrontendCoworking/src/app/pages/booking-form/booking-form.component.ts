import { Component } from '@angular/core';
import { Location } from '@angular/common';
import { CommonModule } from '@angular/common';
import { FormGroup, FormControl, ReactiveFormsModule } from '@angular/forms';
import { DatePickerComponent } from "../../common-ui/date-picker/date-picker.component";
import { EventEmitter } from '@angular/core';
import { Output } from '@angular/core';
import { TimePickerComponent } from '../../common-ui/time-picker/time-picker.component';

@Component({
  selector: 'app-booking-form',
  imports: [CommonModule, DatePickerComponent, TimePickerComponent, ReactiveFormsModule],
  templateUrl: './booking-form.component.html',
  styleUrl: './booking-form.component.scss'
})

export class BookingFormComponent {
  bookingForm = new FormGroup ({
    name: new FormControl(''),
    email: new FormControl(''),
    startDate: new FormControl<Date | null>(null),
    endDate: new FormControl<Date | null>(null),
    startTime: new FormControl<string | null>(null),
    endTime: new FormControl<string | null>(null),
  });
  
  constructor(private location: Location) { }

  goBack() { this.location.back(); }

  onStartDateChange(date: Date | null) { 
    this.bookingForm.get('startDate')?.setValue(date);
  }

  onEndDateChange(date: Date | null) {  
    this.bookingForm.get('endDate')?.setValue(date);
  }

  onStartTimeChange(time: string) {
    this.bookingForm.get('startTime')?.setValue(time);
  } 

  onEndTimeChange(time: string) {
    this.bookingForm.get('endTime')?.setValue(time);
  } 

  onSubmit() {
    console.warn(this.bookingForm.value);
  }
}
