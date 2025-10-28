import {Component, signal} from '@angular/core';
import {Router, RouterLink, RouterOutlet} from '@angular/router';
import {AdminModule} from './admin/admin-module';
import {UiModule} from './ui/ui-module';
import {NgxSpinnerModule} from 'ngx-spinner';
import {HttpClientModule} from '@angular/common/http';
import {AuthService} from './services/common/auth';
import {CustomToastrService, ToastrMessageType, ToastrPosition} from './services/ui/custom-toastr';

declare var $: any;


@Component({
  selector: 'app-root',
  imports: [
    RouterOutlet,
    AdminModule,
    UiModule,
    RouterLink,
    NgxSpinnerModule,
    HttpClientModule,
  ],
  templateUrl: './app.html',
  styleUrl: './app.scss'
})
export class App {
  protected readonly title = signal('ECommerceClient');

  constructor(public authService: AuthService, private toastrService: CustomToastrService, private router: Router) {
    authService.identityCheck()
  }

  signOut(){
    localStorage.removeItem('accessToken');
    this.authService.identityCheck();
    this.router.navigate(['']);
    this.toastrService.message("Oturum başarıyla kapatılmıştır","Oturum Durumu", {
      messageType: ToastrMessageType.INFO,
      position: ToastrPosition.TOP_LEFT
    })
  }
}
// $.get("http://localhost:5013/api/products", data => {
//   console.log(data);
// })
