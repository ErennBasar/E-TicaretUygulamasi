import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {RouterModule} from '@angular/router';
import {Products} from './products';
import {List} from './list/list';
import {Create} from './create/create';
import {FileUploadModule} from '../../../services/common/file-upload/file-upload-module';



@NgModule({
  declarations: [

  ],
  imports: [
    Products,
    CommonModule,
    List,
    Create,
    FileUploadModule,
    RouterModule.forChild([
      { path: "", component: Products}
    ]),
  ]
})
export class ProductsModule { }
