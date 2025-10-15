import {Component, Inject, inject} from '@angular/core';
import {MatButton} from "@angular/material/button";
import {
  MAT_DIALOG_DATA,
  MatDialogActions,
  MatDialogClose,
  MatDialogContent,
  MatDialogRef,
  MatDialogTitle
} from "@angular/material/dialog";
import {BaseDialog} from '../base/base-dialog';

@Component({
  selector: 'app-file-upload-dialog',
  standalone: true,
  imports: [
    MatButton,
    MatDialogActions,
    MatDialogContent,
    MatDialogTitle,
    MatDialogClose
  ],
  templateUrl: './file-upload-dialog.html',
  styleUrl: './file-upload-dialog.scss'
})
export class FileUploadDialog extends BaseDialog<FileUploadDialog>{
  constructor(
    dialogRef: MatDialogRef<FileUploadDialog>,
    @Inject(MAT_DIALOG_DATA) public data: FileUploadDialogState
  ) {
    super(dialogRef);
  }
}

export enum FileUploadDialogState {
  Yes,
  No
}
