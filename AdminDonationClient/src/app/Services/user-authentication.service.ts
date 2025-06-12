import { HttpClient } from '@angular/common/http';
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
}
