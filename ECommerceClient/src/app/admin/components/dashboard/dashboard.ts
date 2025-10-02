import {Component, OnInit} from '@angular/core';
import {AlertifyService, MessageType, Position} from '../../../services/admin/alertify';
import {NgxSpinnerService} from 'ngx-spinner';
import {Base, SpinnerType} from '../../../base/base';

@Component({
  selector: 'app-dashboard',
  imports: [],
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.scss'
})
export class Dashboard extends Base implements OnInit{
  constructor(private alertify: AlertifyService, spinner: NgxSpinnerService ) {
    super(spinner);
  }
  ngOnInit(): void {
    this.showSpinner(SpinnerType.BALL_SPIN_CLOCKWİSE_FADE_ROTATING)
  }
  m(){
    this.alertify.message("Merhaba", {
      messageType : MessageType.SUCCESS,
      delay : 5,
      position : Position.TOP_RIGHT,
    })
  }
  d(){
    this.alertify.dismiss();
  }
}
