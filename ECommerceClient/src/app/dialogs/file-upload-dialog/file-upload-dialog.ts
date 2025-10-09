import {Component, inject} from '@angular/core';
import {MatButton} from "@angular/material/button";
import {MatDialogActions, MatDialogClose, MatDialogContent, MatDialogRef, MatDialogTitle} from "@angular/material/dialog";

@Component({
  selector: 'app-file-upload-dialog',
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
export class FileUploadDialog {
  readonly dialogRef = inject(MatDialogRef<FileUploadDialog>);
}
