import {Component, OnInit} from '@angular/core';
import {NgxSpinnerService} from 'ngx-spinner';
import {Base, SpinnerType} from '../../../base/base';

@Component({
  selector: 'app-orders',
  imports: [],
  templateUrl: './orders.html',
  styleUrl: './orders.scss'
})
export class Orders extends Base implements OnInit{
  constructor(spinner: NgxSpinnerService) {
    super(spinner);
  }

  ngOnInit(): void {
    this.showSpinner(SpinnerType.BALL_SPIN_CLOCKWİSE_FADE_ROTATING)
  }
}
