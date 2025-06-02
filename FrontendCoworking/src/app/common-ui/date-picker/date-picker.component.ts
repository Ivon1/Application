import { Component, OnInit, Output, EventEmitter, Input } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-date-picker',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './date-picker.component.html',
  styleUrl: './date-picker.component.scss'
})

export class DatePickerComponent implements OnInit {
  @Input() title: string = 'Start date';
  
  days: number[] = [];
  months: { value: number, name: string } [] = [];
  years: number[] = [];
  
  selectedDay: number | null = null;
  selectedMonth: number | null = null;
  selectedYear: number | null = null;
  
  @Output() dateChange = new EventEmitter<Date | null>();

  ngOnInit(): void {
    this.generateDays();
    this.generateMonths();
    this.generateYears();
    
    this.emitSelectedDate();
  }

  generateDays(): void {
    this.days = Array.from({length: 31}, (_, i) => i + 1);
  }

  generateMonths(): void {
    this.months = [
      { value: 0, name: 'January' },
      { value: 1, name: 'February' },
      { value: 2, name: 'March' },
      { value: 3, name: 'April' },
      { value: 4, name: 'May' },
      { value: 5, name: 'June' },
      { value: 6, name: 'July' },
      { value: 7, name: 'August' },
      { value: 8, name: 'September' },
      { value: 9, name: 'October' },
      { value: 10, name: 'November' },
      { value: 11, name: 'December' }
    ];
  }

  generateYears(): void {
    const currentYear = new Date().getFullYear();
    this.years = Array.from({length: 5}, (_, i) => currentYear + i);
  }

  selectDay(day: number): void {
    this.selectedDay = day;
    this.emitSelectedDate();
  }

  selectMonth(month: number): void {
    this.selectedMonth = month;
    this.emitSelectedDate();
  }

  selectYear(year: number): void {
    this.selectedYear = year;
    this.emitSelectedDate();
  }

  getMonthName(): string {
    return this.months.find(m => m.value === this.selectedMonth)?.name || '';
  }

  emitSelectedDate(): void {
    if (this.selectedDay === null || this.selectedMonth === null || this.selectedYear === null) {
      this.dateChange.emit(null);
      return;
    }
    
    const daysInMonth = new Date(this.selectedYear, this.selectedMonth + 1, 0).getDate();
    const adjustedDay = Math.min(this.selectedDay, daysInMonth);
    
    const selectedDate = new Date(this.selectedYear, this.selectedMonth, adjustedDay);
    this.dateChange.emit(selectedDate);
  }
}
