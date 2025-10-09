import {Directive, ElementRef, EventEmitter, HostListener, inject, Input, Output, Renderer2} from '@angular/core';
import {HttpClientService} from '../../services/common/http-client';
import {SpinnerType} from '../../base/base';
import {NgxSpinnerService} from 'ngx-spinner';
import {MatDialog} from '@angular/material/dialog';
import {DeleteDialog} from '../../dialogs/delete-dialog/delete-dialog';
import {AlertifyService, MessageType, Position} from '../../services/admin/alertify';
import {HttpErrorResponse} from '@angular/common/http';

declare var $: any;

@Directive({
  selector: '[appDelete]'
})
export class DeleteDirective {

  readonly dialog = inject(MatDialog);
  constructor(
    private element: ElementRef,
    private _renderer: Renderer2,
    private httpClientService: HttpClientService ,
    private spinner: NgxSpinnerService,
    private alertifyService: AlertifyService,
  ) {
    const img = _renderer.createElement("img");
    img.setAttribute("src", "assets/icons/delete_icon_solid.png");
    img.setAttribute("style","cursor: pointer;");
    img.width = 25;
    img.height = 25;
    _renderer.appendChild(element.nativeElement, img);
   }
   @Input() id: string;
   @Input() controller: string;
   @Output() hasanCallBack: EventEmitter<any> = new EventEmitter();

   @HostListener("click") // ilgili direciv'in(appDelete) kullanıldığı dom nesnesine tıklanıldığında hangi olay verildiyse o olay gerçekleşir

   onclick() {
    this.openDialog('300ms', '300ms');
   }

  openDialog(enterAnimationDuration: string, exitAnimationDuration: string): void {
    const dialogRef = this.dialog.open(DeleteDialog, {
      width: '250px',
      enterAnimationDuration,
      exitAnimationDuration,
    });

    dialogRef.afterClosed().subscribe(async result => {
      if (result) {
        this.spinner.show(SpinnerType.BALL_SPIN_CLOCKWİSE_FADE_ROTATING);
        const td: HTMLTableCellElement = this.element.nativeElement;
        this.httpClientService.delete({
          controller: this.controller
        }, this.id).subscribe( () => {
          $(td.parentElement).fadeOut(1000, () => {
            this.hasanCallBack.emit();
            this.alertifyService.message("Product succesfuly deleted",{
              dismissOthers: true,
              messageType: MessageType.SUCCESS,
              position: Position.TOP_CENTER,
            })
          });

        }, (errorResponse: HttpErrorResponse) => {
          this.spinner.hide(SpinnerType.BALL_SPIN_CLOCKWİSE_FADE_ROTATING);
          this.alertifyService.message("Unexpected Error Occured",{
            dismissOthers: true,
            messageType: MessageType.ERROR,
            position: Position.TOP_CENTER,
          })
        })
      }
    });
  }
}
