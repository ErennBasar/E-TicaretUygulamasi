import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {NgxFileDropModule} from 'ngx-file-drop';



@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    NgxFileDropModule, // npm install ngx-file-drop --save
  ]
})
export class FileUploadModule { }
