import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { CoworkingInterface } from '../interfaces/coworking-interface';
import { BaseService } from './base.service';

@Injectable({
  providedIn: 'root'
})

export class CoworkingService  extends BaseService {
  baseUrl: string = this.apiUrl + '/coworking';
  http:HttpClient = inject(HttpClient);
  constructor() { super(); }

  getCoworkings(): Observable<CoworkingInterface[]> {
    return this.http.get<CoworkingInterface[]>(this.baseUrl);
  }
}
