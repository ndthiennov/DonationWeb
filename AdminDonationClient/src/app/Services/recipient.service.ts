import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class RecipientService {

  readonly baseUrl = 'http://localhost:5178/api/Recipient';

  constructor(private http: HttpClient) { }

  GetAll(pageIndex: any) {
    return this.http.get(this.baseUrl + '/GetAll/' + pageIndex,
      {
        reportProgress: true,
        observe: 'events'
      });
  }

  GetSearchedList(pageIndex: any, text: any) {
    return this.http.put(this.baseUrl + '/GetSearchedList/' + pageIndex, text,
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
