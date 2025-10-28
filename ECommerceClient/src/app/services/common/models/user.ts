import {Injectable} from '@angular/core';
import {HttpClientService} from '../http-client';
import {User} from '../../../entities/user';
import {Create_user} from '../../../contracts/users/create_user';
import {firstValueFrom, Observable} from 'rxjs';
import {Token} from '../../../contracts/token/token';
import {CustomToastrService, ToastrMessageType, ToastrPosition} from '../../ui/custom-toastr';
import {TokenResponse} from '../../../contracts/token/tokenResponse';
import {SocialUser} from '@abacritt/angularx-social-login';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  constructor(private httpClientService: HttpClientService, private toastrService: CustomToastrService) { }

  async create(user: User) : Promise<Create_user> {
    const observable: Observable<Create_user | User> = this.httpClientService.post<Create_user | User>({
      controller: "users",
    }, user);

    return await firstValueFrom(observable) as Create_user;
  }

  async login(usernameOrEmail: string, password: string, callBackFunction?: () => void) : Promise<any> {
    const observable: Observable<any | TokenResponse> = this.httpClientService.post<any | TokenResponse >({
      controller: "users",
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
      action: "google-login",
      controller: "users"
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
      controller: "users",
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
