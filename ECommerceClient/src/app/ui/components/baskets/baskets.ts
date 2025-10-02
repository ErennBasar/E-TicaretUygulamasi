import {Component, OnInit} from '@angular/core';
import {Base, SpinnerType} from '../../../base/base';
import {NgxSpinnerService} from 'ngx-spinner';

@Component({
  selector: 'app-baskets',
  imports: [],
  templateUrl: './baskets.html',
  styleUrl: './baskets.scss'
})
export class Baskets extends Base implements OnInit{
  constructor(spinner: NgxSpinnerService) {
    super(spinner);
  }
  ngOnInit() {
    this.showSpinner(SpinnerType.PACMAN)
  }
}
