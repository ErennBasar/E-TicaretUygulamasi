import {Component, OnInit, ViewChild} from '@angular/core';
import {Base, SpinnerType} from '../../../base/base';
import {NgxSpinnerService} from 'ngx-spinner';
import { HttpClientService } from '../../../services/common/http-client';
import { Create_Product } from '../../../contracts/create_product';
import { MatSidenavModule } from '@angular/material/sidenav';
import { Create } from './create/create';
import { List } from './list/list';

@Component({
  selector: 'app-products',
  imports: [
    MatSidenavModule,
    Create,
    List,
    
  ],
  templateUrl: './products.html',
  styleUrl: './products.scss'
})
export class Products extends Base implements OnInit {
  constructor(spinner: NgxSpinnerService, private httpClientService: HttpClientService) {
    super(spinner);
  }

  ngOnInit() {
    // this.showSpinner(SpinnerType.BALL_SPIN_CLOCKWİSE_FADE_ROTATING);

    // this.httpClientService.get<Create_Product[]>({
    //   controller : "products"
    // }).subscribe(data => console.log(data));

    // this.httpClientService.post({
    //   controller: "products"
    // },{
    //   name : "Kalem",
    //   stock : 1000,
    //   price : 5
    // }).subscribe();

    // this.httpClientService.put({
    //   controller:"products",
    // },{
    //   id:"0199a6c1-b77f-7b02-9bef-8041bcc7dc6e",
    //   name:"Renkli Kağıt",
    //   stock:1500,
    //   price: 7
    // }).subscribe()

  //   this.httpClientService.delete({
  //     controller: "products"
  //   },"0199a6c1-b77f-71ed-8ca0-4f5fd234ecc9").subscribe();

    // this.httpClientService.get({
    //   fullEndPoint: "https://jsonplaceholder.typicode.com/posts"
    // }).subscribe(data => console.log(data));
  }

  @ViewChild(List) listComponents: List ;

  createdProduct(createdProduct: Create_Product){
    this.listComponents.getProducts();
  }
}
