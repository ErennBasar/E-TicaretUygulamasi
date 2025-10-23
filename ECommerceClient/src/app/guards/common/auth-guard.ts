import { CanActivateFn } from '@angular/router';
import {JwtHelperService} from '@auth0/angular-jwt';
import {inject} from '@angular/core';
import {Router} from '@angular/router';
import {CustomToastrService, ToastrMessageType, ToastrPosition} from '../../services/ui/custom-toastr';
import {NgxSpinnerService} from 'ngx-spinner';
import {SpinnerType} from '../../base/base';

//npm i @auth0/angular-jwt kütüphanesini ekledik
export const authGuard: CanActivateFn = (route, state) => {

  const jwtHelper = inject(JwtHelperService);
  const router = inject(Router);
  const toastrService = inject(CustomToastrService)
  const spinner = inject(NgxSpinnerService)

  spinner.show(SpinnerType.PACMAN)

  const token: string = localStorage.getItem("accessToken");

  //const decodeToken = jwtHelper.decodeToken(token);
  //const expirationDate: Date = jwtHelper.getTokenExpirationDate(token);
  let expired: boolean;

  try{
    expired = jwtHelper.isTokenExpired(token);
  } catch{
    expired = true;
  }

  if(!token || expired){
    router.navigate(['/login'], {queryParams: {returnUrl: state.url}});

    toastrService.message("Bu sayfaya erişmek için giriş yapmalısınız","Yetkisiz Erişim", {
      messageType: ToastrMessageType.WARNING,
      position: ToastrPosition.TOP_RIGHT,
    });
  }
  spinner.hide(SpinnerType.PACMAN);
  return true;
};
