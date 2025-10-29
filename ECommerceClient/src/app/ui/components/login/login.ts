import {Component} from '@angular/core';
import {NgxSpinnerService} from 'ngx-spinner';
import {Base, SpinnerType} from '../../../base/base';
import {AuthService} from '../../../services/common/auth';
import {ActivatedRoute, Router} from '@angular/router';
import {
  FacebookLoginProvider,
  GoogleSigninButtonDirective,
  SocialAuthService,
  SocialUser
} from '@abacritt/angularx-social-login';
import {CustomToastrService} from '../../../services/ui/custom-toastr';
import {UserAuthService} from '../../../services/common/models/user-auth';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [
    GoogleSigninButtonDirective
  ],
  templateUrl: './login.html',
  styleUrl: './login.scss'
})
export class Login extends Base{
  constructor(
    private userAuthService: UserAuthService,
    spinner: NgxSpinnerService,
    private authService: AuthService,
    private activatedRoute: ActivatedRoute,
    private router: Router,
    private socialAuthService: SocialAuthService,
    private toastr: CustomToastrService,) {
    super(spinner)
    socialAuthService.authState.subscribe(async (user: SocialUser) => {
      console.log(user);
      this.showSpinner(SpinnerType.PACMAN);
      switch (user.provider) {
        case 'GOOGLE':
          await userAuthService.googleLogin(user, ()=> {
            this.authService.identityCheck();
            this.hideSpinner(SpinnerType.PACMAN)
          })
          break;
        case 'FACEBOOK':
          await userAuthService.facebookLogin(user, ()=> {
            this.authService.identityCheck();
            this.hideSpinner(SpinnerType.PACMAN)
          })
          break;
      }
    });

  }
  async login(usernameOrEmail: string, password: string) {
    this.showSpinner(SpinnerType.PACMAN);
    await this.userAuthService.login(usernameOrEmail, password, () => {
      this.authService.identityCheck()

      this.activatedRoute.queryParams.subscribe(params => {
        const returnUrl: string = params['returnUrl'];
        if(returnUrl){
          this.router.navigate([returnUrl]);
        }
      })
      this.hideSpinner(SpinnerType.PACMAN);
    });
  }
  async facebookLogin() {
    await this.socialAuthService.signIn(FacebookLoginProvider.PROVIDER_ID)
  }
}
