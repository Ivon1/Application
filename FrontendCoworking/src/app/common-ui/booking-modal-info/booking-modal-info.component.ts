import { DIALOG_DATA, DialogRef, DialogModule } from '@angular/cdk/dialog';
import { Component, Inject, inject } from '@angular/core';
import { RouterLink } from '@angular/router';

@Component({
    selector: 'app-booking-modal-info',
    imports: [RouterLink, DialogModule],
    templateUrl: './booking-modal-info.component.html',
    styleUrl: './booking-modal-info.component.scss'
})

export class BookingModalInfoComponent {
    success: boolean = false;
    message: string | undefined = undefined;
    coworkingId: number | undefined = undefined;

    private dialogRef = inject(DialogRef);

    constructor(@Inject(DIALOG_DATA) public data: any) {
        this.success = data.success;
        this.coworkingId = data.coworkingId;
        this.message = data.message;
    }


    closeModal(){
        this.dialogRef.close();
    }
}
