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


}
