import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import {MatButtonModule} from '@angular/material/button';
import { ProductService } from '../../../../services/common/models/product';
import { Create_Product } from '../../../../contracts/create_product';
import { Base, SpinnerType } from '../../../../base/base';
import { NgxSpinnerService } from 'ngx-spinner';
import { AlertifyService, MessageType, Position } from '../../../../services/admin/alertify';

@Component({
  selector: 'app-create',
  imports: [
    MatFormFieldModule, 
    MatInputModule,
    MatButtonModule
  ],
  templateUrl: './create.html',
  styleUrl: './create.scss'
})
export class Create extends Base implements OnInit{
  
  constructor(private productService: ProductService, spinner: NgxSpinnerService, private alertify: AlertifyService){
    super(spinner)
  }

  ngOnInit(): void {
    
  }

  @Output() createdProduct : EventEmitter<Create_Product> = new EventEmitter();

  create(name: HTMLInputElement, stock: HTMLInputElement, price: HTMLInputElement){

    this.showSpinner(SpinnerType.PACMAN);
    
    const create_product: Create_Product = new Create_Product();

    create_product.name = name.value;
    create_product.stock = parseInt(stock.value);
    create_product.price = parseFloat(price.value);

    if(!name.value){
      this.alertify.message("Please enter the product name!",{
        dismissOthers: true,
        messageType: MessageType.WARNING,
        position: Position.BOTTOM_RIGHT
      });
      return;
    }

    if(parseInt(stock.value) < 0){
      this.alertify.message("Please check your stock value!",{
        dismissOthers: true,
        messageType: MessageType.WARNING,
        position: Position.BOTTOM_RIGHT
      });
      return;
    }

    this.productService.create(create_product, () => {
      this.hideSpinner(SpinnerType.PACMAN)
      this.alertify.message("product succesfly added",{
        dismissOthers: true,
        messageType: MessageType.SUCCESS,
        position: Position.BOTTOM_RIGHT
      });
      this.createdProduct.emit(create_product);
    }, errorMessage => {
      this.alertify.message(errorMessage, {
        dismissOthers: true,
        messageType: MessageType.ERROR,
        position: Position.TOP_RIGHT
      });
    });
  };
}
