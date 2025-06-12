import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class UserTokenService {

  readonly baseUrl = 'http://localhost:5178/api/UserToken';

  constructor(private http: HttpClient) { }

  Add(token: any) {
    return this.http.post(this.baseUrl + '/Add', token,
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
}
