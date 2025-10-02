import { Injectable } from '@angular/core';
import {ToastrService} from 'ngx-toastr';

@Injectable({
  providedIn: 'root'
})
export class CustomToastrService {
  constructor(private toastr: ToastrService) { }

  message(message: string, title: string, toastrOptions: Partial<ToastrOptions>) {
    this.toastr[toastrOptions.messageType](message, title,{
      positionClass: toastrOptions.position
    });
  }
}
export class ToastrOptions {
  messageType : ToastrMessageType = ToastrMessageType.SUCCESS;
  position : ToastrPosition = ToastrPosition.TOP_RIGHT;
}
export enum ToastrMessageType {
  SUCCESS = "success",
  ERROR = "error",
  INFO = "info",
  WARNING = "warning"
}

export enum ToastrPosition {
  TOP_RIGHT = "toast-top-right",
  TOP_LEFT = "toast-top-left",
  TOP_CENTER = "toast-top-center",
  BOTTOM_RIGHT = "toast-bottom-right",
  BOTTOM_LEFT = "toast-bottom-left",
  BOTTOM_CENTER = "toast-bottom-center",
  TOP_FULL_WIDTH = "toast-top-full-width",
  BOTTOM_FULL_WIDTH = "toast-bottom-full-width",
}
