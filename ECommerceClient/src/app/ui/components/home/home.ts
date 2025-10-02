import {Component, OnInit} from '@angular/core';
import {Base, SpinnerType} from '../../../base/base';
import {NgxSpinnerService} from 'ngx-spinner';

@Component({
  selector: 'app-home',
  imports: [],
  templateUrl: './home.html',
  styleUrl: './home.scss'
})
export class Home extends Base implements OnInit{
  constructor(spinner: NgxSpinnerService) {
    super(spinner);
  }
  ngOnInit() {
    this.showSpinner(SpinnerType.PACMAN)
  }
}
