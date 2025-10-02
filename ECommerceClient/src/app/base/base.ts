import { Component } from '@angular/core';
import {NgxSpinnerService} from 'ngx-spinner';

// @Component({
//   selector: 'app-base',
//   imports: [],
//   templateUrl: './base.html',
//   styleUrl: './base.scss'
// })
export class Base {
  constructor(private spinner: NgxSpinnerService) {}

  showSpinner(spinnerNameType: SpinnerType) {
    this.spinner.show(spinnerNameType);

    setTimeout(() => this.hideSpinner(spinnerNameType), 1000);
  }

  hideSpinner(spinnerNameType: SpinnerType) {
    this.spinner.hide(spinnerNameType);
  }
}

export enum SpinnerType {
  PACMAN = "pacman",
  BALL_SPIN_CLOCKWİSE_FADE_ROTATING = "ball",
}
