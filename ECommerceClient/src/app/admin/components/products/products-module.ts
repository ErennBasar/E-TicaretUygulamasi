import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {RouterModule} from '@angular/router';
import {Products} from './products';



@NgModule({
  declarations: [
    
  ],
  imports: [
    Products,
    CommonModule,
    
    RouterModule.forChild([
      { path: "", component: Products}
    ]),
  ]
})
export class ProductsModule { }
