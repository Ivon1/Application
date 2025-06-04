import { Component, inject } from '@angular/core';
import { Location } from '@angular/common';
import { CommonModule } from '@angular/common';
import { FormGroup, FormControl, ReactiveFormsModule, Validators } from '@angular/forms';
import { DatePickerComponent } from "../../common-ui/date-picker/date-picker.component";
import { TimePickerComponent } from '../../common-ui/time-picker/time-picker.component';
import { WorkspaceService } from '../../data/services/workspace.service';
import { Observable } from 'rxjs';
import { WorkspaceInterface } from '../../data/interfaces/workspace-interface';
import { AvailabilityInterface } from '../../data/interfaces/availability-interface';

@Component({
    selector: 'app-booking-form',
    imports: [CommonModule, DatePickerComponent, TimePickerComponent, ReactiveFormsModule],
    templateUrl: './booking-form.component.html',
    styleUrl: './booking-form.component.scss'
})

export class BookingFormComponent {
    bookingForm = new FormGroup
        ({
            name: new FormControl('', Validators.required),
            email: new FormControl('', [Validators.required, Validators.email]),
            startDate: new FormControl<Date | null>(null, Validators.required),
            endDate: new FormControl<Date | null>(null, Validators.required),
            startTime: new FormControl<string | null>(null, Validators.required),
            endTime: new FormControl<string | null>(null, Validators.required),
            workspaceType: new FormControl<WorkspaceInterface | null>(null, Validators.required),
            availabilityType: new FormControl<AvailabilityInterface | null>(null, Validators.required),
        });

    workspaces$!: Observable<WorkspaceInterface[]>;
    workspaceService = inject(WorkspaceService);
    selectedWorkspace: WorkspaceInterface | null = null;

    constructor(private location: Location) {
        this.workspaces$ = this.workspaceService.getAllWorkspaces();
    }

    goBack() { this.location.back(); }

    onStartDateChange(date: Date | null) {
        this.bookingForm.get('startDate')?.setValue(date);
    }

    onEndDateChange(date: Date | null) {
        this.bookingForm.get('endDate')?.setValue(date);
    }

    onStartTimeChange(time: string) {
        this.bookingForm.get('startTime')?.setValue(time);
    }

    onEndTimeChange(time: string) {
        this.bookingForm.get('endTime')?.setValue(time);
    }

    onWorkspaceTypeSelect(workspace: WorkspaceInterface) {
        this.selectedWorkspace = workspace;
        this.bookingForm.get('workspaceType')?.setValue(this.selectedWorkspace);
    }

    onSubmit() {
        console.warn(this.bookingForm.value);
    }
}