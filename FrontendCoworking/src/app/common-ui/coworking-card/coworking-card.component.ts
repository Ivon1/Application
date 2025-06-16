import { Component, Input } from '@angular/core';
import { CoworkingInterface } from '../../data/interfaces/coworking-interface';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-coworking-card',
  imports: [CommonModule, RouterLink],
  templateUrl: './coworking-card.component.html',
  styleUrl: './coworking-card.component.scss'
})

export class CoworkingCardComponent {
  @Input() coworking: CoworkingInterface | undefined;
}
