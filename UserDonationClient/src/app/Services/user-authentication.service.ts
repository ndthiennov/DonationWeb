import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class UserAuthenticationService {

  readonly baseUrl = 'http://localhost:5178/api/UserAuthentication';

  constructor(private http: HttpClient) { }

  SignIn(loginForm: any) {
    return this.http.post(this.baseUrl + '/SignIn', loginForm, { responseType: 'text' });
  }

  CheckExistedUser(phonenum: any) {
    return this.http.post(this.baseUrl + '/CheckAccount', phonenum,
      {
        reportProgress: true,
        observe: 'events',
        headers: new HttpHeaders({
          'Content-Type': 'application/json'
        }),
        responseType: 'text'
      },
    );
  }

  SignUpDonor(signUpForm: any) {
    return this.http.post(this.baseUrl + '/SignUpDonor', signUpForm,
      {
        reportProgress: true,
        observe: 'events',
        headers: new HttpHeaders({
          'Content-Type': 'application/json'
        })
      },
    );
  }

  SignUpOrganiser(signUpFormData: any) {
    return this.http.post(this.baseUrl + '/SignUpOrganiser', signUpFormData,
      {
        reportProgress: true,
        observe: 'events'
      },
    );
  }

  SignUpRecipient(signUpForm: any) {
    return this.http.post(this.baseUrl + '/SignUpRecipient', signUpForm,
      {
        reportProgress: true,
        observe: 'events',
        headers: new HttpHeaders({
          'Content-Type': 'application/json'
        })
      },
    );
  }
}
