import { Component, inject } from '@angular/core';
import { WorkspaceInterface } from '../../data/interfaces/workspace-interface';
import { WorkspaceService } from '../../data/services/workspace.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-workspaces-page',
  imports: [CommonModule],
  templateUrl: './workspaces-page.component.html',
  styleUrl: './workspaces-page.component.scss'
})

export class WorkspacesPageComponent {

  workspaces: WorkspaceInterface[] = [];
  workspaceService: WorkspaceService = inject(WorkspaceService);

  constructor() { 
    this.workspaceService.getAllWorkspaces().subscribe({
      next: (workspaces: WorkspaceInterface[]) => {
        this.workspaces = workspaces;
      },
      error: (error) => {
        console.error('Error fetching workspaces:', error);
      }
    });}
}
