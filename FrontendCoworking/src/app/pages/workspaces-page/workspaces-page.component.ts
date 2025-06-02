import { Component, inject } from '@angular/core';
import { WorkspaceInterface } from '../../data/interfaces/workspace-interface';
import { WorkspaceService } from '../../data/services/workspace.service';
import { CommonModule } from '@angular/common';
import { WorkspaceCardComponent } from '../../common-ui/workspace-card/workspace-card.component';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-workspaces-page',
  imports: [CommonModule, WorkspaceCardComponent],
  templateUrl: './workspaces-page.component.html',
  styleUrl: './workspaces-page.component.scss'
})
export class WorkspacesPageComponent {
  workspaces$: Observable<WorkspaceInterface[]>;
  workspaceService = inject(WorkspaceService);
  
  constructor() { 
    this.workspaces$ = this.workspaceService.getAllWorkspaces();
  }
}
