import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class OrganiserService {

  readonly baseUrl = 'http://localhost:5178/api/Organiser';

  constructor(private http: HttpClient) { }

  GetById(organiserId: any) {
    return this.http.get(this.baseUrl + '/GetById/' + organiserId,
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
