import { DIALOG_DATA, DialogRef } from '@angular/cdk/dialog';
import { Component, Inject, inject } from '@angular/core';

@Component({
    selector: 'app-booking-modal-choice',
    imports: [],
    templateUrl: './booking-modal-choice.component.html',
    styleUrl: './booking-modal-choice.component.scss'
})

export class BookingModalChoiceComponent {
    actionType: string | undefined = undefined;

    private dialogRef = inject(DialogRef);

    constructor(@Inject(DIALOG_DATA) public data: any) {
        this.actionType = data.actionType;
    }

    closeModal() {
        this.dialogRef.close();
    }

    confirmCancellation() {
        this.dialogRef.close({ confirm: true });
    }
}
