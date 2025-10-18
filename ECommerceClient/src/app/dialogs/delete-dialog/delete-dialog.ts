import {Component, Inject, inject} from '@angular/core';
import {MatButtonModule} from '@angular/material/button';
import {BaseDialog} from '../base/base-dialog';
import {MatDialogModule, MAT_DIALOG_DATA, MatDialogRef,} from '@angular/material/dialog';

@Component({
  selector: 'app-delete-dialog',
  standalone: true,
  imports: [
    MatButtonModule,
    MatDialogModule,
  ],
  templateUrl: './delete-dialog.html',
  styleUrl: './delete-dialog.scss'
})
export class DeleteDialog extends BaseDialog<DeleteDialog> {
  constructor(
    dialogRef: MatDialogRef<DeleteDialog>,
    @Inject(MAT_DIALOG_DATA) public data: DeleteState,
  ) {
    super(dialogRef);
  }
}

export enum DeleteState {
  Yes,
  No
}
