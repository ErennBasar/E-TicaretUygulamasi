import {Component, signal} from '@angular/core';
import {RouterLink, RouterOutlet} from '@angular/router';
import {AdminModule} from './admin/admin-module';
import {UiModule} from './ui/ui-module';
import {CustomToastrService, ToastrMessageType, ToastrPosition} from './services/ui/custom-toastr';
import {NgxSpinnerModule} from 'ngx-spinner';
declare var $: any;


@Component({
  selector: 'app-root',
  imports: [
    RouterOutlet,
    AdminModule,
    UiModule,
    RouterLink,
    NgxSpinnerModule,
  ],
  providers: [
    {provide: "baseURL", useValue: "http://localhost:5013/api/", multi: true}
  ],
  templateUrl: './app.html',
  styleUrl: './app.scss'
})
export class App {
  protected readonly title = signal('ECommerceClient');

  constructor() {

  }
}
$.get("http://localhost:5013/api/products", data => {
  console.log(data);
})
