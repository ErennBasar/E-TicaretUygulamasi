import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {DeleteDialog} from './delete-dialog/delete-dialog';
import {FileUploadDialog} from './file-upload-dialog/file-upload-dialog';
import {MatDialogModule} from '@angular/material/dialog';



@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    DeleteDialog,
    FileUploadDialog,
    MatDialogModule,
  ]
})
export class DialogsModule { }
