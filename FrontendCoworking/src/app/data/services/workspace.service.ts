import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { inject } from '@angular/core';
import { Observable } from 'rxjs';
import { WorkspaceInterface } from '../interfaces/workspace-interface';
import { BaseService } from './base.service';

@Injectable({
  providedIn: 'root'
})

export class WorkspaceService extends BaseService {

  http: HttpClient = inject(HttpClient);
  private baseUrl: string = this.apiUrl + '/workspaces';

  constructor() { super();  }

  getAllWorkspaces() : Observable<WorkspaceInterface[]> { 
    return this.http.get<WorkspaceInterface[]>(this.baseUrl);
  }

  getWorkspacesByCoworkingId(coworkingId: number) : Observable<WorkspaceInterface[]> { 
    return this.http.get<WorkspaceInterface[]>(`${this.baseUrl}/GetWorkspacesByCoworkingId/${coworkingId}`);
  }
}
