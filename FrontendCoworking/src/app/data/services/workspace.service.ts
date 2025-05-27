import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { inject } from '@angular/core';
import { Observable } from 'rxjs';
import { WorkspaceInterface } from '../interfaces/workspace-interface';

@Injectable({
  providedIn: 'root'
})
export class WorkspaceService {

  http: HttpClient = inject(HttpClient);
  private baseUrl: string = 'https://localhost:7201/Workspaces';

  constructor() { }

  getAllWorkspaces() : Observable<WorkspaceInterface[]> { 
    return this.http.get<WorkspaceInterface[]>(this.baseUrl);
  }
}
