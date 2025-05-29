import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-images-gallery',
  imports: [CommonModule],
  templateUrl: './images-gallery.component.html',
  styleUrl: './images-gallery.component.scss'
})

export class ImagesGalleryComponent {
  @Input() images: string[] = [];
  selectedImageIndex: number = 0;

  constructor() { }

  setMainImage(index: number): void {
    this.selectedImageIndex = index;
  }
}
