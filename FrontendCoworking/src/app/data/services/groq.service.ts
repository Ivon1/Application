import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})

export class GroqService {
  http:HttpClient = inject(HttpClient);
  baseUrl = 'https://localhost:7201/Groq';
  constructor() { }

  sendQuestion(userQuestion: string) : Observable<any> {
    const payload = { Question: userQuestion };
    return this.http.post<any>(`${this.baseUrl}`, payload);
  }
}
