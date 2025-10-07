import { Directive, ElementRef, EventEmitter, HostListener, Input, Output, Renderer2 } from '@angular/core';
import { HttpClientService } from '../../services/common/http-client';
import { ProductService } from '../../services/common/models/product';
import { Base, SpinnerType } from '../../base/base';
import { NgxSpinnerService } from 'ngx-spinner';

declare var $: any;

@Directive({
  selector: '[appDelete]'
})
export class DeleteDirective {

  constructor(
    private element: ElementRef, 
    private _renderer: Renderer2, 
    private productService: ProductService ,
    private spinner: NgxSpinnerService
  ) {
    const img = _renderer.createElement("img");
    img.setAttribute("src", "assets/icons/delete_icon_solid.png");
    img.setAttribute("style","cursor: pointer;");
    img.width = 25;
    img.height = 25;
    _renderer.appendChild(element.nativeElement, img);
   }
   @Input() id: string;
   @Output() hasanCallBack: EventEmitter<any> = new EventEmitter();

   @HostListener("click") // ilgili direciv'in(appDelete) kullanıldığı dom nesnesine tıklanıldığında hangi olay verildiyse o olay gerçekleşir
   async onclick() {
    this.spinner.show(SpinnerType.BALL_SPIN_CLOCKWİSE_FADE_ROTATING);
    const td: HTMLTableCellElement = this.element.nativeElement;
    await this.productService.delete(this.id)
    $(td.parentElement).fadeOut(1500, () => {
      this.hasanCallBack.emit();
    });
   }
}
