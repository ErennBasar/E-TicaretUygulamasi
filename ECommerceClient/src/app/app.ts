import { Component, signal } from '@angular/core';
import {RouterLink, RouterOutlet} from '@angular/router';
import {Layout} from './admin/layout/layout';
import {AdminModule} from './admin/admin-module';
import {UiModule} from './ui/ui-module';
declare var $: any;
import {BrowserAnimationsModule, provideAnimations} from '@angular/platform-browser/animations';
import {ToastrModule, provideToastr, ToastrService} from 'ngx-toastr';
import bootstrap from '../main.server';

@Component({
  selector: 'app-root',
  imports: [
    RouterOutlet,
    Layout,
    AdminModule,
    UiModule,
    RouterLink,
    BrowserAnimationsModule,
  ],
  providers: [
    provideAnimations(),
  ],
  templateUrl: './app.html',
  styleUrl: './app.scss'
})
export class App {
  protected readonly title = signal('ECommerceClient');

  constructor(private toastr: ToastrService) {
    toastr.success('Hello world!', 'Toastr fun!');
  }
}
