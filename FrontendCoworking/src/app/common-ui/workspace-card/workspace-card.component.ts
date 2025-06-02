import { Component, Input } from '@angular/core';
import { WorkspaceInterface } from '../../data/interfaces/workspace-interface';
import { ImagesGalleryComponent } from "../images-gallery/images-gallery.component";
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-workspace-card',
  imports: [ImagesGalleryComponent, RouterLink],
  templateUrl: './workspace-card.component.html',
  styleUrl: './workspace-card.component.scss'
})

export class WorkspaceCardComponent {
  @Input() workspace: WorkspaceInterface | null = null;


  getFirstTwoWords(text: string): string {
    if (!text) return '';
    const words = text.split(' ');
    return words.slice(0, 2).join(' ');
  }
  
  getRemainingText(text: string): string {
    if (!text) return '';
    const words = text.split(' ');
    return words.length > 2 ? ' ' + words.slice(2).join(' ') : '';
  }
}
