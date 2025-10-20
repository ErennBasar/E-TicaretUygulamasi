import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {Register} from './register';
import {RouterModule} from '@angular/router';


@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    Register,
    RouterModule.forChild([
      {path: "", component: Register},
    ]),
  ]
})
export class RegisterModule { }
