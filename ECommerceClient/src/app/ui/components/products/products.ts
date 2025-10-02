import {Component, OnInit} from '@angular/core';
import {Base, SpinnerType} from '../../../base/base';
import {NgxSpinnerService} from 'ngx-spinner';

@Component({
  selector: 'app-products',
  imports: [],
  templateUrl: './products.html',
  styleUrl: './products.scss'
})
export class Products extends Base implements OnInit{
  constructor(spinner: NgxSpinnerService) {
    super(spinner);
  }
  ngOnInit() {
    this.showSpinner(SpinnerType.PACMAN)
  }
}
