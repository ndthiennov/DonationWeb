import { Component, OnInit } from '@angular/core';
import { UserAuthenticationService } from '../../Services/user-authentication.service';
import { Router } from '@angular/router';

import { ReactiveFormsModule, FormControl, FormGroup, Validators } from '@angular/forms';
import { HttpEventType } from '@angular/common/http';
import { SharedService } from '../../Shared/shared.service';

@Component({
  selector: 'app-donor-auth',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './donor-auth.component.html',
  styleUrl: './donor-auth.component.css'
})
export class DonorAuthComponent implements OnInit {

  constructor(private userAuthenticationService: UserAuthenticationService,
    public sharedService: SharedService,
    private router: Router) {

  }

  loginForm: FormGroup = new FormGroup({});
  signUpForm: FormGroup = new FormGroup({});

  username: any

  ngOnInit(): void {
    this.initLogin();
    this.initSignUp();

    if (typeof window !== 'undefined') {
      if (localStorage.getItem('token') != null) {
        var payLoad = JSON.parse(decodeURIComponent(escape(window.atob(localStorage.getItem('token')!.split('.')[1]))));
        var userRole = payLoad['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];
        if (userRole == "donor") {
          this.router.navigateByUrl('/');
        }
        else {
          localStorage.removeItem('token');
          this.router.navigateByUrl('/auth/donor');
        }
      }
    }
  }

  private initSignUp(): void {
    this.signUpForm = new FormGroup({
      'phonenum': new FormControl(null, Validators.required),
      'password': new FormControl(null, Validators.required),
      'name': new FormControl(null, Validators.required),
      'gender': new FormControl(null, Validators.required),
      'dob': new FormControl(null, Validators.required),
      'email': new FormControl(null, Validators.required),
      'address': new FormControl(null, Validators.required),
      'code': new FormControl(null, Validators.required),
      'disabled': new FormControl("active", Validators.required)
    });
  }

  private initLogin(): void {
    this.loginForm = new FormGroup({
      'phonenum': new FormControl(null, Validators.required),
      'password': new FormControl(null, Validators.required),
      'role': new FormControl("donor", Validators.required)
    });
  }

  SendOtp() {
    this.userAuthenticationService.CheckExistedUser(JSON.stringify(this.signUpForm.value.phonenum)).subscribe(
      (res: any) => {
        if (res.type === HttpEventType.Response) {
          // localStorage.setItem('signupform', this.signUpForm.value);
          this.sharedService.signUpRole = "donor";
          this.sharedService.signUpForm = this.signUpForm;
          this.router.navigateByUrl('/auth/otp');
        }
      },
      (err: any) => {
        console.log(err);
        this.initSignUp();
        if (!err.error.type) {
          var announce = document.getElementById('announce');
          if (announce) {
            announce.style.display = "block";
            announce.innerHTML = err.error;
            console.log(err.error);
          }
          setTimeout(function () {
            if (announce) {
              announce.style.display = "none";
            }
          }, 4000);
        }
        else {
          var announce = document.getElementById('announce');
          if (announce) {
            announce.style.display = "block";
            announce.innerHTML = "There are some errors occured";
          }
          setTimeout(function () {
            if (announce) {
              announce.style.display = "none";
            }
          }, 4000);
        }
      }
    );
  }

  LoginSubmit() {
    this.userAuthenticationService.SignIn(this.loginForm.value).subscribe(
      (res: any) => {

        // Convert res to base64
        let base64 = res.replace(/-/g, '+').replace(/_/g, '/');
        while (base64.length % 4) {
          base64 += '=';
        }
        
        var payLoad = JSON.parse(decodeURIComponent(escape(window.atob(base64!.split('.')[1]))));
        var userId = payLoad['Id'];
        var userAva = payLoad['AvaSrc'];
        var userName = payLoad['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name'];

        localStorage.setItem('token', res);
        localStorage.setItem('userid', userId);
        localStorage.setItem('username', userName);
        localStorage.setItem('userava', userAva);

        this.router.navigateByUrl('/');
      },
      (err: any) => {
        console.log(err);
        if (!err.error.type) {
          var announce = document.getElementById('announce-sign-in');
          if (announce) {
            announce.style.display = "block";
            announce.innerHTML = err.error;
            console.log(err.error);
          }
          setTimeout(function () {
            if (announce) {
              announce.style.display = "none";
            }
          }, 4000);
        }
        else {
          var announce = document.getElementById('announce-sign-in');
          if (announce) {
            announce.style.display = "block";
            announce.innerHTML = "There are some errors occured";
          }
          setTimeout(function () {
            if (announce) {
              announce.style.display = "none";
            }
          }, 4000);
        }
      }
    );
  }

  Register() {
    const container = document.getElementById('container');
    container?.classList.add("active");
  }

  Login() {
    const container = document.getElementById('container');
    container?.classList.remove("active");
  }
}
