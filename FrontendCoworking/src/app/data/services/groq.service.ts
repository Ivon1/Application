import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { BaseService } from './base.service';

@Injectable({
  providedIn: 'root'
})

export class GroqService extends BaseService {
  http:HttpClient = inject(HttpClient);
  baseUrl = this.apiUrl + '/groq';
  constructor() { super(); }

  sendQuestion(userQuestion: string) : Observable<any> {
    const payload = { Question: userQuestion };
    console.log('Sending payload:', payload);
    return this.http.post<any>(`${this.baseUrl}`, payload);
  }
}
