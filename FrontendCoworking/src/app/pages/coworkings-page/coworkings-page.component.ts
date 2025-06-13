import { Component, inject } from '@angular/core';
import { CoworkingInterface } from '../../data/interfaces/coworking-interface';
import { CoworkingService } from '../../data/services/coworking.service';
import { Observable } from 'rxjs';
import { CoworkingCardComponent } from "../../common-ui/coworking-card/coworking-card.component";
import { AsyncPipe } from '@angular/common';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-coworkings-page',
  imports: [CoworkingCardComponent, AsyncPipe, CommonModule],
  templateUrl: './coworkings-page.component.html',
  styleUrl: './coworkings-page.component.scss'
})

export class CoworkingsPageComponent {
  coworkings$: Observable<CoworkingInterface[]>;
  coworkingService: CoworkingService = inject(CoworkingService);

  constructor() { this.coworkings$ = this.coworkingService.getCoworkings(); }

  refreshList(): void {
    this.coworkings$ = this.coworkingService.getCoworkings();
  }
}
