import { Component, OnInit } from '@angular/core';
import { UserAuthenticationService } from '../../Services/user-authentication.service';
import { SharedService } from '../../Shared/shared.service';
import { Router } from '@angular/router';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { HttpEventType } from '@angular/common/http';

@Component({
  selector: 'app-organiser-auth',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './organiser-auth.component.html',
  styleUrl: './organiser-auth.component.css'
})
export class OrganiserAuthComponent implements OnInit {
  constructor(private userAuthenticationService: UserAuthenticationService,
    public sharedService: SharedService,
    private router: Router) {

  }

  loginForm: FormGroup = new FormGroup({});
  signUpForm: FormGroup = new FormGroup({});
  signUpFormData: any = new FormData();
  imagePreview: any = "imagepreview.png"
  selectedFile: File | null = null;

  username: any

  ngOnInit(): void {
    this.initLogin();
    this.initSignUp();

    if (typeof window !== 'undefined') {
      if (localStorage.getItem('token') != null) {
        var payLoad = JSON.parse(decodeURIComponent(escape(window.atob(localStorage.getItem('token')!.split('.')[1]))));
        var userRole = payLoad['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];
        if (userRole == "organiser") {
          this.router.navigateByUrl('/');
        }
        else {
          localStorage.removeItem('token');
          this.router.navigateByUrl('/auth/organiser');
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
      'disabled': new FormControl(false, Validators.required)
    });

    this.selectedFile = null;
    this.imagePreview = "";
    this.signUpFormData = new FormData();
  }

  private initLogin(): void {
    this.loginForm = new FormGroup({
      'phonenum': new FormControl(null, Validators.required),
      'password': new FormControl(null, Validators.required),
      'role': new FormControl("organiser", Validators.required)
    });
  }

  SendOtp() {
    //  Create form data to add to [FromForm] of API
    this.signUpFormData = new FormData();
    this.signUpFormData.append("phonenum", this.signUpForm.get('phonenum')?.value);
    this.signUpFormData.append("password", this.signUpForm.get('password')?.value);
    this.signUpFormData.append("name", this.signUpForm.get('name')?.value);
    this.signUpFormData.append("dob", this.signUpForm.get('dob')?.value);
    this.signUpFormData.append("email", this.signUpForm.get('email')?.value);
    this.signUpFormData.append("address", this.signUpForm.get('address')?.value);
    if (this.selectedFile != null) {
      this.signUpFormData.append("certificationfile", this.selectedFile, this.selectedFile?.name);
    }
    this.signUpFormData.append("description", null);
    this.signUpFormData.append("disabled", "active");

    this.userAuthenticationService.CheckExistedUser(JSON.stringify(this.signUpForm.value.phonenum)).subscribe(
      (res: any) => {
        if (res.type === HttpEventType.Response) {
          // localStorage.setItem('signupform', this.signUpForm.value);
          this.sharedService.signUpRole = "organiser";
          this.sharedService.signUpForm = this.signUpForm;
          this.sharedService.signUpFormData = this.signUpFormData;
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

        console.log(res);
        this.router.navigateByUrl('/organiser/profile/'+userId);
      },
      (err: any) => {
        console.log(err);
        console.log(!err.error.type);
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

  UploadImg(event: any) {
    let reader = new FileReader();
    reader.readAsDataURL(event.target.files[0]);
    reader.onload = () => {
      let img = <HTMLIFrameElement>document.querySelector(".image-preview");
      img.style.display = "block";
      this.imagePreview = reader.result;
    };
    this.selectedFile = <File>event.target.files[0];
  }
}
