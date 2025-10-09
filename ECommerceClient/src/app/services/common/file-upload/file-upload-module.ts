import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {NgxFileDropModule} from 'ngx-file-drop';
import {DialogsModule} from '../../../dialogs/dialogs-module';



@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    NgxFileDropModule, // npm install ngx-file-drop --save
    DialogsModule,
  ]
})
export class FileUploadModule { }
