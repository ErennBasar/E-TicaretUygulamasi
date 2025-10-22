import { Injectable } from '@angular/core';
import {HttpClientService} from '../http-client';
import {User} from '../../../entities/user';
import {Create_user} from '../../../contracts/users/create_user';
import {firstValueFrom, Observable} from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  constructor(private httpClientService: HttpClientService) { }

  async create(user: User) : Promise<Create_user> {
    const observable: Observable<Create_user | User> = this.httpClientService.post<Create_user | User>({
      controller: "users",
    }, user);

    return await firstValueFrom(observable) as Create_user;
  }

  async login(usernameOrEmail: string, password: string, callBackFunction?: () => void) : Promise<void> {
    const observable: Observable<any> = this.httpClientService.post({
      controller: "users",
      action: "login",
    }, {usernameOrEmail, password })

    await firstValueFrom(observable);
    callBackFunction();
  }
}
