import {Component, OnInit} from '@angular/core';
import {Base, SpinnerType} from '../../../base/base';
import {NgxSpinnerService} from 'ngx-spinner';

@Component({
  selector: 'app-customers',
  imports: [],
  templateUrl: './customers.html',
  styleUrl: './customers.scss'
})
export class Customers extends Base implements OnInit{
  constructor(spinner: NgxSpinnerService) {
    super(spinner);
  }
  ngOnInit(): void {
    this.showSpinner(SpinnerType.BALL_SPIN_CLOCKWİSE_FADE_ROTATING);
  }
}
