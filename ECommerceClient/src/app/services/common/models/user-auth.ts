import { Injectable } from '@angular/core';
import {HttpClientService} from '../http-client';
import {CustomToastrService, ToastrMessageType, ToastrPosition} from '../../ui/custom-toastr';
import {firstValueFrom, Observable} from 'rxjs';
import {TokenResponse} from '../../../contracts/token/tokenResponse';
import {SocialUser} from '@abacritt/angularx-social-login';

@Injectable({
  providedIn: 'root'
})
export class UserAuthService {

  constructor(private httpClientService: HttpClientService, private toastrService: CustomToastrService) { }
  async login(usernameOrEmail: string, password: string, callBackFunction?: () => void) : Promise<any> {
    const observable: Observable<any | TokenResponse> = this.httpClientService.post<any | TokenResponse >({
      controller: "auth",
      action: "login",
    }, {usernameOrEmail, password })

    const tokenResponse: TokenResponse = await firstValueFrom(observable) as TokenResponse;

    if(tokenResponse){
      localStorage.setItem("accessToken", tokenResponse.token.accessToken); // toke.accesToken  f12 ile incelediğimde undefined geliyor niye
      //localStorage.setItem("expiration", token.expiration.toString());

      this.toastrService.message("User successfully login", "Login Successful",{
        messageType: ToastrMessageType.SUCCESS,
        position: ToastrPosition.TOP_LEFT
      })
    }

    callBackFunction();
  }

  async googleLogin(user: SocialUser, callBackFunction?: ()=> void) : Promise<any> {
    const observable: Observable<SocialUser | TokenResponse> = this.httpClientService.post<SocialUser | TokenResponse>({
      controller: "auth",
      action: "google-login",
    }, user)

    const tokenResponse: TokenResponse = await firstValueFrom(observable) as TokenResponse;

    if(tokenResponse){
      localStorage.setItem("accessToken", tokenResponse.token.accessToken);

      this.toastrService.message("User successfully login", "Login Successful",{
        messageType: ToastrMessageType.SUCCESS,
        position: ToastrPosition.TOP_LEFT
      })
    }
    callBackFunction();
  }

  async facebookLogin(user: SocialUser, callBackFunction?: () => void) : Promise<any> {
    const observable: Observable<SocialUser | TokenResponse> = this.httpClientService.post<SocialUser | TokenResponse >({
      controller: "auth",
      action: "facebook-login",
    }, user)
    const tokenResponse: TokenResponse = await firstValueFrom(observable) as TokenResponse;

    if(tokenResponse){
      localStorage.setItem("accessToken", tokenResponse.token.accessToken);

      this.toastrService.message("User successfully login", "Login Successful",{
        messageType: ToastrMessageType.SUCCESS,
        position: ToastrPosition.TOP_LEFT
      })
    }
  }

}
