import { Component, inject, Input, OnInit } from '@angular/core';
import { Location, CommonModule } from '@angular/common';
import { Observable } from 'rxjs';
import { FormGroup, FormControl, ReactiveFormsModule, Validators } from '@angular/forms';
import { DatePickerComponent } from "../../common-ui/date-picker/date-picker.component";
import { TimePickerComponent } from '../../common-ui/time-picker/time-picker.component';
import { WorkspaceService } from '../../data/services/workspace.service';
import { WorkspaceInterface } from '../../data/interfaces/workspace-interface';
import { AvailabilityInterface } from '../../data/interfaces/availability-interface';
import { BookingService } from '../../data/services/booking.service';
import { BookingInterface } from '../../data/interfaces/booking-interface';
import { TimeUtilService } from '../../data/services/time-util.service';
import { ActivatedRoute } from '@angular/router';
import { Dialog } from '@angular/cdk/dialog';
import { BookingModalInfoComponent } from '../../common-ui/booking-modal-info/booking-modal-info.component';

@Component({
    selector: 'app-booking-form',
    imports: [CommonModule, DatePickerComponent, TimePickerComponent, ReactiveFormsModule],
    templateUrl: './booking-form.component.html',
    styleUrl: './booking-form.component.scss'
})

export class BookingFormComponent implements OnInit {
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
    bookingService = inject(BookingService);
    timeUtilService = inject(TimeUtilService);
    route = inject(ActivatedRoute);
    selectedWorkspace: WorkspaceInterface | null = null;

    isEditMode = false;
    bookingId: number | null = null;
    pageTitle = 'Book your workspace';

    //Modal window
    private dialog = inject(Dialog);
    protected openModal(success: boolean, coworkingId?: number, message?: string) {
        this.dialog.open<string>(BookingModalInfoComponent, {
            data: { success: success, coworkingId: coworkingId, message: message }
        });
    }

    constructor(private location: Location) {
        // this.workspaces$ = this.workspaceService.getAllWorkspaces();
    }

    ngOnInit() {
        this.route.paramMap.subscribe(params => {
            const id = params.get('id');
            const coworkingId = params.get('coworkingId');
            if (id) {
                this.isEditMode = true;
                this.bookingId = parseInt(id, 10);
                this.pageTitle = 'Edit your booking';
                this.loadBookingData(this.bookingId);
            } else if (coworkingId) {
                this.workspaces$ = this.workspaceService.getWorkspacesByCoworkingId(parseInt(coworkingId));
            }

        });
    }

    goBack() { this.location.back(); }

    onStartDateChange(date: Date | null) {
        this.bookingForm.get('startDate')?.setValue(date);
        const endDateControl = this.bookingForm.get('endDate');
        console.log('Start date changed:', endDateControl);
    }

    onEndDateChange(date: Date | null) {
        this.bookingForm.get('endDate')?.setValue(date);
        console.log('End date changed:', date);
    }

    onStartTimeChange(time: string) {
        this.bookingForm.get('startTime')?.setValue(time);
        console.log('Start time changed:', time);
    }

    onEndTimeChange(time: string) {
        this.bookingForm.get('endTime')?.setValue(time);
        console.log('Start time changed:', time);
    }

    onWorkspaceTypeSelect(workspace: WorkspaceInterface) {
        this.selectedWorkspace = workspace;
        this.bookingForm.get('workspaceType')?.setValue(this.selectedWorkspace);
    }

    loadBookingData(id: number) {
        this.bookingService.getBookingById(id).subscribe({
            next: (booking) => {
                if (booking) {
                    this.selectedWorkspace = booking.workspace;
                    console.log('selected availability:', booking.availability?.name);

                    const startTime = booking.startDate ?
                        this.timeUtilService.formatTime(
                            new Date(booking.startDate).getHours(),
                            new Date(booking.startDate).getMinutes()
                        ) : null;

                    const endTime = booking.endDate ?
                        this.timeUtilService.formatTime(
                            new Date(booking.endDate).getHours(),
                            new Date(booking.endDate).getMinutes()
                        ) : null;

                    let matchingAvailability = null;
                    if (booking.availability && this.selectedWorkspace?.availabilities) {
                        matchingAvailability = this.selectedWorkspace.availabilities.find(
                            a => a.id === booking.availability?.id
                        );
                    }

                    setTimeout(() => {
                        if (matchingAvailability) {
                            this.bookingForm.get('availabilityType')?.setValue(matchingAvailability);
                        }
                    }, 0);


                    this.bookingForm.patchValue({
                        name: booking.name,
                        email: booking.email,
                        startDate: booking.startDate ? new Date(booking.startDate) : null,
                        endDate: booking.endDate ? new Date(booking.endDate) : null,
                        startTime: startTime,
                        endTime: endTime,
                        workspaceType: booking.workspace,
                        availabilityType: booking.availability
                    });
                }
            },
            error: (error) => {
                console.error('Error loading booking data:', error);
            }
        });
    }

    onSubmit() {
        // Combine date and time into a single Date object
        const startDate = this.bookingForm.get('startDate')?.value;
        const endDate = this.bookingForm.get('endDate')?.value;
        const startTime = this.bookingForm.get('startTime')?.value;
        const endTime = this.bookingForm.get('endTime')?.value;

        if (startDate && endDate && startTime && endTime) {
            const startDateTime = new Date(startDate);
            const endDateTime = new Date(endDate);
            console.log('Start DateTime:', startDateTime);
            console.log('End DateTime:', endDateTime);

            const startTimeParsed = this.timeUtilService.parseTime(startTime);
            const endTimeParsed = this.timeUtilService.parseTime(endTime);

            startDateTime.setHours(startTimeParsed.hours, startTimeParsed.minutes);
            endDateTime.setHours(endTimeParsed.hours, endTimeParsed.minutes);
            console.log('Start DateTime:', startDateTime);
            console.log('End DateTime:', endDateTime);

            const booking: BookingInterface = {
                id: this.isEditMode ? this.bookingId : -1,
                name: this.bookingForm.get('name')?.value || null,
                email: this.bookingForm.get('email')?.value || null,
                startDate: startDateTime,
                endDate: endDateTime,
                workspace: this.bookingForm.get('workspaceType')?.value || null,
                availability: this.bookingForm.get('availabilityType')?.value || null
            };

            if (this.isEditMode) {
                this.bookingService.updateBooking(booking).subscribe({
                    next: (response: any) => {
                        this.openModal(true, booking.workspace?.coworkingId, response.message);
                    },
                    error: (error) => {
                        this.openModal(false, booking.workspace?.coworkingId, error.message);
                    }
                });
            } else {
                this.bookingService.createBooking(booking).subscribe({
                    next: (response: any) => {
                        this.openModal(true, booking.workspace?.coworkingId, response.message);
                    },
                    error: (error) => {
                        this.openModal(false, booking.workspace?.coworkingId, 'Please choose a different time slot');
                    }
                });
            }


        } else {
            console.error('Please fill in all required fields.');
        }
    }
}