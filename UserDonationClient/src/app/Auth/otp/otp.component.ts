import { Component, OnInit } from '@angular/core';
import { SharedService } from '../../Shared/shared.service';
import { Router } from '@angular/router';
import { UserAuthenticationService } from '../../Services/user-authentication.service';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { HttpEventType } from '@angular/common/http';

@Component({
  selector: 'app-otp',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './otp.component.html',
  styleUrl: './otp.component.css'
})
export class OtpComponent implements OnInit {
  constructor(public shareService: SharedService,
    private userAuthenticationService: UserAuthenticationService,
    private router: Router
  ) {

  }

  code: string = "";
  codeForm: FormGroup = new FormGroup({});
  loginForm: FormGroup = new FormGroup({});

  ngOnInit(): void {
    this.initLogin();
    this.InputType();
  }

  private initLogin(): void {
    this.loginForm = new FormGroup({
      'phonenum': new FormControl(null, Validators.required),
      'password': new FormControl(null, Validators.required),
      'role': new FormControl(this.shareService.signUpRole, Validators.required)
    });
  }

  SignUpSubmit() {
    if (this.shareService.signUpRole == "donor") {
      this.shareService.signUpForm.value.code = this.code;
      this.userAuthenticationService.SignUpDonor(this.shareService.signUpForm.value).subscribe(
        (res: any) => {
          if (res.type === HttpEventType.Response) {
            this.loginForm.value.phonenum = this.shareService.signUpForm.value.phonenum;
            this.loginForm.value.password = this.shareService.signUpForm.value.password;
            this.shareService.signUpForm = new FormGroup({});
            this.shareService.signUpRole = "";
            this.LoginSubmit(this.loginForm, "donor");
          }
        },
        (err: any) => {
          console.log(err);
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
    if (this.shareService.signUpRole == "organiser") {
      this.shareService.signUpFormData.append("code", this.code);
      console.log(this.shareService.signUpRole);
      console.log(this.shareService.signUpFormData);
      this.userAuthenticationService.SignUpOrganiser(this.shareService.signUpFormData).subscribe(
        (res: any) => {
          if (res.type === HttpEventType.Response) {
            this.ActiveToast(true, "Sign up successfully. Please wait for approvement");
            this.shareService.signUpForm = new FormGroup({});
            this.shareService.signUpFormData = new FormData();
            this.shareService.signUpRole = "";
          }
        },
        (err: any) => {
          console.log(err);
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
    if (this.shareService.signUpRole == "recipient") {
      this.shareService.signUpForm.value.code = this.code;
      this.userAuthenticationService.SignUpRecipient(this.shareService.signUpForm.value).subscribe(
        (res: any) => {
          if (res.type === HttpEventType.Response) {
            this.loginForm.value.phonenum = this.shareService.signUpForm.value.phonenum;
            this.loginForm.value.password = this.shareService.signUpForm.value.password;
            this.shareService.signUpForm = new FormGroup({});
            this.shareService.signUpRole = "";
            this.LoginSubmit(this.loginForm, "recipient");
          }
        },
        (err: any) => {
          console.log(err);
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
  }

  LoginSubmit(loginForm: any, role: any) {
    this.userAuthenticationService.SignIn(loginForm.value).subscribe(
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

        if (role == "donor") {
          this.router.navigateByUrl('/');
        }
        if (role == "recipient") {
          this.router.navigateByUrl('/recipient/profile/'+userId);
        }
      },
      (err: any) => {
        console.log(err);
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

  InputType() {
    const inputs = document.querySelectorAll("input");
    const button = document.querySelector("button");
    // iterate over all inputs
    inputs.forEach((input, index1) => {
      input.addEventListener("keyup", (e) => {
        // This code gets the current input element and stores it in the currentInput variable
        // This code gets the next sibling element of the current input element and stores it in the nextInput variable
        // This code gets the previous sibling element of the current input element and stores it in the prevInput variable
        const currentInput = input,
          nextInput = <HTMLInputElement>input.nextElementSibling,
          prevInput = <HTMLInputElement>input.previousElementSibling;
        // if the value has more than one character then clear it
        if (currentInput.value.length > 1) {
          currentInput.value = "";
          this.code = this.code.substring(0, this.code.length - 1)
          return;
        }
        // if the next input is disabled and the current value is not empty
        //  enable the next input and focus on it
        if (nextInput && nextInput.hasAttribute("disabled") && currentInput.value !== "") {
          nextInput.removeAttribute("disabled");
          nextInput.focus();
        }
        // if the backspace key is pressed
        if (e.key === "Backspace") {
          this.code = "";
          // iterate over all inputs again
          inputs.forEach((input, index2) => {
            // console.log(input.value);
            this.code = this.code + input.value.toString();
            // if the index1 of the current input is less than or equal to the index2 of the input in the outer loop
            // and the previous element exists, set the disabled attribute on the input and focus on the previous element
            if (index1 <= index2 && prevInput) {
              input.setAttribute("disabled", "true");
              // if (input.value != "") {
              //   this.code = this.code.substring(1, this.code.length - 1);
              //   console.log(this.code);
              // }
              input.value = "";
              prevInput.focus();
            }
          });
        }
        else {
          this.code = this.code + currentInput.value.toString();
        }
        //if the fourth input( which index number is 3) is not empty and has not disable attribute then
        //add active class if not then remove the active class.
        if (!inputs[5].disabled && inputs[5].value !== "") {
          button?.classList.add("active");
          return;
        }
        button?.classList.remove("active");
        console.log(this.code);
      });
    });
    //focus the first input which index is 0 on window load
    window.addEventListener("load", () => inputs[0].focus());
  }

  ActiveToast(isSuccess: boolean, response: string) {
    var toastContainer = document.getElementById("toast-container");
    var progress = document.getElementById("progress");
    var toastTitle = document.getElementById("toast-title");
    var toastBody = document.getElementById("toast-body");
    var toastSuccessIcon = document.getElementById("toast-success");
    var toastErrorIcon = document.getElementById("toast-error");

    if (!isSuccess) {
      progress?.classList.add("error")
      toastContainer?.classList.add("error")
      toastTitle!.innerHTML = "Error";
      toastBody!.innerHTML = response;
      toastErrorIcon!.style.display = "flex";
      toastSuccessIcon!.style.display = "none";
    }
    else {
      progress?.classList.remove("error")
      toastContainer?.classList.remove("error")
      toastTitle!.innerHTML = "Success";
      toastBody!.innerHTML = response;
      toastErrorIcon!.style.display = "none";
      toastSuccessIcon!.style.display = "flex";
    }

    toastContainer?.classList.add("active")
    progress?.classList.add("active")

    setTimeout(() => {
      toastContainer?.classList.remove("active")
      progress?.classList.remove("active")
    }, 5000)
  }
}