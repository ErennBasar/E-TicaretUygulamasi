import {Component, OnInit} from '@angular/core';
import {FormBuilder, FormGroup, ReactiveFormsModule, Validators} from '@angular/forms';
import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';
import {NgClass} from '@angular/common';
import {User} from '../../../entities/user';

@Component({
  selector: 'app-register',
  imports: [
    ReactiveFormsModule,
    NgClass
  ],
  templateUrl: './register.html',
  styleUrl: './register.scss'
})
export class Register implements OnInit {

  constructor(private formBuilder: FormBuilder) {

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
        Validators.minLength(10),
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
  onSubmit(data: User) {
    this.submitted = true;

    if(this.frm.invalid)
      return;
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
