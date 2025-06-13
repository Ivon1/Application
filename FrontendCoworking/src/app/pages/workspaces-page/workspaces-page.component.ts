import { Component, inject } from '@angular/core';
import { WorkspaceInterface } from '../../data/interfaces/workspace-interface';
import { WorkspaceService } from '../../data/services/workspace.service';
import { CommonModule } from '@angular/common';
import { WorkspaceCardComponent } from '../../common-ui/workspace-card/workspace-card.component';
import { Observable } from 'rxjs';
import { ActivatedRoute } from '@angular/router';
import { Location } from '@angular/common';

@Component({
  selector: 'app-workspaces-page',
  imports: [CommonModule, WorkspaceCardComponent],
  templateUrl: './workspaces-page.component.html',
  styleUrl: './workspaces-page.component.scss'
})

export class WorkspacesPageComponent {
  workspaces$: Observable<WorkspaceInterface[]> = new Observable<WorkspaceInterface[]>();
  workspaceService = inject(WorkspaceService);
  route = inject(ActivatedRoute);
  location = inject(Location);
  
  constructor() { 
    this.route.params.subscribe(params => {
      const id = params['id'];
      if (id) {
        this.workspaces$ = this.workspaceService.getWorkspacesByCoworkingId(id);
      }
    })
  }

  goBack() {
    this.location.back();
  }
}
