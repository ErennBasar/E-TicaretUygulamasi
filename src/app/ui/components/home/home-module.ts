import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {RouterModule} from '@angular/router';
import {Baskets} from '../baskets/baskets';
import {Home} from './home';



@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    RouterModule.forChild([
      { path: "", component: Home}
    ])
  ]
})
export class HomeModule { }
