import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {DeleteDialog} from './delete-dialog/delete-dialog';
import {FileUploadDialog} from './file-upload-dialog/file-upload-dialog';
import {MatDialogModule} from '@angular/material/dialog';
import {SelectProductImageDialog} from './select-product-image-dialog/select-product-image-dialog';
import {FileUploadModule} from '../services/common/file-upload/file-upload-module';



@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    DeleteDialog,
    FileUploadDialog,
    SelectProductImageDialog,
    MatDialogModule,
  ]
})
export class DialogsModule { }
