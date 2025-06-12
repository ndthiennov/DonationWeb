import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class OrganiserService {

  readonly baseUrl = 'http://localhost:5178/api/Organiser';

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

  GetAllUnCensored(pageIndex: any) {
    return this.http.get(this.baseUrl + '/GetAllUnCensored/' + pageIndex,
      {
        reportProgress: true,
        observe: 'events'
      });
  }

  GetSearchedUncensoredList(pageIndex: any, text: any) {
    return this.http.put(this.baseUrl + '/GetSearchedUncensoredList/' + pageIndex, text,
      {
        reportProgress: true,
        observe: 'events',
        headers: new HttpHeaders({
          'Content-Type': 'application/json'
        })
      },
    );
  }

  UpdateApprovement(organiserId: any) {
    return this.http.put(this.baseUrl + '/UpdateApprovement/' + organiserId,
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
