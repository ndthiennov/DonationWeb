import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class RecipientService {

  readonly baseUrl = 'http://localhost:5178/api/Recipient';

  constructor(private http: HttpClient) { }

  GetById(recipientId: any) {
    return this.http.get(this.baseUrl + '/GetById/' + recipientId,
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
