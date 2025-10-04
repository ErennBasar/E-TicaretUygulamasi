import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {RouterModule} from '@angular/router';
import {Products} from './products';



@NgModule({
  declarations: [
  
  ],
  imports: [
    CommonModule,
    RouterModule.forChild([
      { path: "", component: Products}
    ]),
    Products,
  ]
})
export class ProductsModule { }
