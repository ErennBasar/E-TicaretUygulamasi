import {Component, OnInit} from '@angular/core';
import {
  AbstractControl,
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  ValidationErrors,
  ValidatorFn,
  Validators
} from '@angular/forms';
import {NgClass} from '@angular/common';
import {User} from '../../../entities/user';
import {UserService} from '../../../services/common/models/user';
import {Create_user} from '../../../contracts/users/create_user';
import {CustomToastrService, ToastrMessageType, ToastrPosition} from '../../../services/ui/custom-toastr';
import {elementSelectors} from '@angular/cdk/schematics';
import {Base} from '../../../base/base';
import {NgxSpinnerService} from 'ngx-spinner';

@Component({
  selector: 'app-register',
  imports: [
    ReactiveFormsModule,
    NgClass
  ],
  templateUrl: './register.html',
  standalone: true,
  styleUrl: './register.scss'
})
export class Register extends Base implements OnInit {

  constructor(private formBuilder: FormBuilder, private userService: UserService, private toastrService: CustomToastrService, spinner: NgxSpinnerService) {
    super(spinner)
  }

  frm: FormGroup;

  ngOnInit() {
    this.frm = this.formBuilder.group({
      nameSurname: ["", [
        Validators.required,
        Validators.maxLength(18),
        Validators.minLength(3),
        ],
      ],
      userName: ["", [
        Validators.required,
        Validators.maxLength(11),
        Validators.minLength(3),
        ],
      ],
      email: ["", [
        Validators.required,
        Validators.email,
        ],
      ],
      password: ["", [
        Validators.required,
        Validators.minLength(3),
        ],
      ],
      passwordAgain: ["", [
        Validators.required,
        ],
      ],
    },{
      validators: [mustMatch('password', 'passwordAgain')],
    });
  }

  get component(){
    return this.frm.controls;
  }

  submitted: boolean = false;
  async onSubmit(user: User) {
    this.submitted = true;

    if(this.frm.invalid)
      return;

    const result: Create_user = await this.userService.create(user);
    if(result.succeeded){
      this.toastrService.message(result.message, "User Created Succesfully",{
        messageType: ToastrMessageType.SUCCESS,
        position: ToastrPosition.TOP_FULL_WIDTH
      })
    }
    else
      this.toastrService.message(result.message, "Error occurred creating user",{
        messageType: ToastrMessageType.ERROR,
        position: ToastrPosition.BOTTOM_FULL_WIDTH
      })
  }
}

// İki alanın eşleşip eşleşmediğini kontrol eden custom validator
export function mustMatch(controlName: string, matchingControlName: string): ValidatorFn {
  return (formGroup: AbstractControl): ValidationErrors | null => {
    const control = formGroup.get(controlName);
    const matchingControl = formGroup.get(matchingControlName);

    // Eğer kontrollerden biri henüz oluşturulmadıysa veya başka bir hata varsa dokunma
    if (!control || !matchingControl || matchingControl.errors && !matchingControl.errors['mustMatch']) {
      return null;
    }

    // Değerler eşleşmiyorsa, ikinci kontrol'e 'mustMatch' hatasını ata
    if (control.value !== matchingControl.value) {
      matchingControl.setErrors({ mustMatch: true });
      return { mustMatch: true }; // Form grubuna da genel bir hata atayabiliriz
    } else {
      // Değerler eşleşiyorsa, hatayı temizle
      matchingControl.setErrors(null);
      return null;
    }
  };
}
