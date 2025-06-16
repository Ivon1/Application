import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { CoworkingInterface } from '../interfaces/coworking-interface';

@Injectable({
  providedIn: 'root'
})

export class CoworkingService {
  baseUrl: string = "https://localhost:7201/Coworking";
  http:HttpClient = inject(HttpClient);
  constructor() { }

  getCoworkings(): Observable<CoworkingInterface[]> {
    return this.http.get<CoworkingInterface[]>(this.baseUrl);
  }
}
