import { Component, OnInit, Output, EventEmitter, Input } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-time-picker',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './time-picker.component.html',
  styleUrl: './time-picker.component.scss'
})

export class TimePickerComponent implements OnInit {
  @Input() title: string = 'Start time';
  @Output() timeChange = new EventEmitter<string>();
  
  timeOptions: string[] = [];
  selectedTime: string | null = null;
  
  ngOnInit(): void {
    this.generateTimeOptions();
  }
  
  /**
   * Generates time options in 30-minute intervals from 9:00 AM to 5:30 PM
   */
  generateTimeOptions(): void {
    const startHour = 9;  // 9 AM
    const endHour = 17;   // 5 PM
    
    for (let hour = startHour; hour <= endHour; hour++) {
      const period = hour >= 12 ? 'PM' : 'AM';
      const displayHour = hour > 12 ? hour - 12 : hour;
      const displayHourStr = displayHour === 0 ? '12' : displayHour.toString();
      
      // Add hour:00
      this.timeOptions.push(`${displayHourStr}:00 ${period}`);
      
      // Add hour:30 (except for the last hour)
      if (hour < endHour) {
        this.timeOptions.push(`${displayHourStr}:30 ${period}`);
      }
    }
  }
  
  /**
   * Selects a time and closes the dropdown
   */
  selectTime(time: string): void {
    this.selectedTime = time;
    this.timeChange.emit(time);
  }
}
