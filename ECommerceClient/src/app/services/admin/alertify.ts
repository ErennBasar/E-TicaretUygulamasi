import { Injectable } from '@angular/core';
declare var alertify: any;
@Injectable({
  providedIn: 'root'
})
export class AlertifyService {
  //message(message: string, messageType: MessageType, position: Position, delay : number = 3, dismissOthers : boolean = false) {
  message(message: string, options: Partial<AlertifyOptions>) {
    alertify.set('notifier','delay', options.delay);
    alertify.set('notifier','position', options.position);
    const msg = alertify[options.messageType](message);
    if (options.dismissOthers) {
      msg.dismissOthers();
    }
  }
  dismiss(){
    alertify.dismissAll();
  }
}
export class AlertifyOptions {
  messageType : MessageType = MessageType.MESSAGE;
  delay : number = 3;
  position : Position = Position.BOTTOM_LEFT;
  dismissOthers : boolean = false;
}
export enum MessageType {
  ERROR = "error",
  MESSAGE = "message",
  NOTIFY = "notify",
  SUCCESS = "success",
  WARNING = "warning"
}
export enum Position {
  TOP_LEFT = "top-left",
  TOP_CENTER = "top-center",
  TOP_RIGHT = "top-right",
  BOTTOM_LEFT = "bottom-left",
  BOTTOM_CENTER = "bottom-center",
  BOTTOM_RIGHT = "bottom-right"
}
