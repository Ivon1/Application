import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class TimeUtilService {
  /**
   * Parses a time string in "hh:mm AM/PM" format into hours and minutes
   */
  parseTime(timeStr: string): { hours: number, minutes: number } {
    const [timePart, meridiem] = timeStr.split(' ');
    let [hours, minutes] = timePart.split(':').map(Number);
    
    if (meridiem === 'PM' && hours !== 12) {
      hours += 12;
    } else if (meridiem === 'AM' && hours === 12) {
      hours = 0;
    }
    
    return { hours, minutes };
  }

  /**
   * Formats hours and minutes into a time string in "hh:mm AM/PM" format
   */
  formatTime(hours: number, minutes: number): string {
    let period = 'AM';
    let displayHours = hours;
    
    if (hours >= 12) {
      period = 'PM';
      displayHours = hours === 12 ? 12 : hours - 12;
    }
    
    displayHours = displayHours === 0 ? 12 : displayHours;
    
    // Ensure minutes are padded with leading zero if needed
    const displayMinutes = minutes < 10 ? `0${minutes}` : `${minutes}`;
    
    return `${displayHours}:${displayMinutes} ${period}`;
  }

  differenceBetweenDates(date1: Date | any, date2: Date | any): string { 
    if (!date1 || !date2) {
      return "Invalid date"; 
    }
    
    const d1 = date1 instanceof Date ? date1 : new Date(date1);
    const d2 = date2 instanceof Date ? date2 : new Date(date2);
    
    const diffInMs = Math.abs(d2.getTime() - d1.getTime());
    const diffInDays = Math.floor(diffInMs / (1000 * 60 * 60 * 24));
    const diffInHours = Math.floor(diffInMs / (1000 * 60 * 60));
    const diffInMinutes = Math.floor(diffInMs / (1000 * 60));

    if (diffInDays > 0) {
      return `(${ diffInDays }  days)`;
    } else if (diffInHours > 0) {
      return `(${ diffInHours }  hours)`;
    } else {
      return `(${ diffInMinutes } minutes)`;
    }
  }
}