import {Component, OnInit} from '@angular/core';
import {AlertifyService, MessageType, Position} from '../../../services/admin/alertify';

@Component({
  selector: 'app-dashboard',
  imports: [],
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.scss'
})
export class Dashboard implements OnInit{
  constructor(private alertify: AlertifyService ) {}
  ngOnInit(): void {

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
