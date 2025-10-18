import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {DeleteDialog} from './delete-dialog/delete-dialog';
import {FileUploadDialog} from './file-upload-dialog/file-upload-dialog';
import {SelectProductImageDialog} from './select-product-image-dialog/select-product-image-dialog';


@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    DeleteDialog,
    FileUploadDialog,
    SelectProductImageDialog,
  ],

})
export class DialogsModule { }
