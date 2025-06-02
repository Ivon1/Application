import { Component } from '@angular/core';
import { Location } from '@angular/common';
import { CommonModule } from '@angular/common';
import { DatePickerComponent } from "../../common-ui/date-picker/date-picker.component";
import { EventEmitter } from '@angular/core';
import { Output } from '@angular/core';
import { TimePickerComponent } from '../../common-ui/time-picker/time-picker.component';

@Component({
  selector: 'app-booking-form',
  imports: [CommonModule, DatePickerComponent, TimePickerComponent],
  templateUrl: './booking-form.component.html',
  styleUrl: './booking-form.component.scss'
})

export class BookingFormComponent {
  
  constructor(private location: Location) { }

  goBack() { this.location.back(); }

  onStartDateChange(date: Date | null) {
    console.log('Start date changed:', date);
  }

  onEndDateChange(date: Date | null) {  
    console.log('End date changed:', date);
  }

  onStartTimeChange(time: string) {
    console.log('Selected time:', time);
  } 

  onEndTimeChange(time: string) {
    console.log('Selected time:', time);
  } 
}
