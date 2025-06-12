import { Component, OnInit } from '@angular/core';
import { ReactiveFormsModule, FormControl, FormGroup, Validators } from '@angular/forms';
import { UserAuthenticationService } from '../../Services/user-authentication.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent implements OnInit {

  constructor(private userAuthenticationService: UserAuthenticationService, private router: Router) {

  }

  loginForm: FormGroup = new FormGroup({});

  username: any

  ngOnInit(): void {

    this.initLogin();

    if (typeof window !== 'undefined') {
      if (localStorage.getItem('token') != null) {
        var payLoad = JSON.parse(decodeURIComponent(escape(window.atob(localStorage.getItem('token')!.split('.')[1]))));
        var userRole = payLoad['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];
        // var userId = payLoad['Id'];
        // console.log(userId);
        // this.username=payLoad['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name'];
        // console.log(this.username);
        if (userRole == "admin") {
          this.router.navigateByUrl('/');
        }
        else {
          localStorage.removeItem('token');
          this.router.navigateByUrl('/login');
        }
      }
    }
  }

  private initLogin(): void {
    this.loginForm = new FormGroup({
      'phonenum': new FormControl(null, Validators.required),
      'password': new FormControl(null, Validators.required),
      'role': new FormControl("admin", Validators.required)
    });
  }

  OnSubmit() {
    this.userAuthenticationService.SignIn(this.loginForm.value).subscribe(
      (res: any) => {
        var payLoad = JSON.parse(decodeURIComponent(escape(window.atob(res!.split('.')[1]))));
        var userId = payLoad['Id'];
        var userName = payLoad['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name'];
        
        localStorage.setItem('token', res);
        localStorage.setItem('userid', userId);
        localStorage.setItem('username', userName);

        console.log(res);
        this.router.navigateByUrl('/');
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
