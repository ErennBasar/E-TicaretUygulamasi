import {Component} from '@angular/core';
import {UserService} from '../../../services/common/models/user';
import {NgxSpinnerService} from 'ngx-spinner';
import {Base, SpinnerType} from '../../../base/base';

@Component({
  selector: 'app-login',
  imports: [],
  templateUrl: './login.html',
  standalone: true,
  styleUrl: './login.scss'
})
export class Login extends Base{
  constructor(private userService: UserService, spinner: NgxSpinnerService) {
    super(spinner)
  }

  async login(usernameOrEmail: string, password: string) {
    this.showSpinner(SpinnerType.PACMAN);
    await this.userService.login(usernameOrEmail, password, () => this.hideSpinner(SpinnerType.PACMAN));
  }
}
