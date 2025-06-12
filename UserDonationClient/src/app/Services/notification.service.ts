import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class NotificationService {

  readonly baseUrl = 'http://localhost:5178/api/Notification';

  constructor(private http: HttpClient) { }

  CheckReadLatestNotification() {
    return this.http.get(this.baseUrl + '/CheckReadLatestNotification',
      {
        reportProgress: true,
        observe: 'events',
        headers: new HttpHeaders({
          'Content-Type': 'application/json'
        })
      },
    );
  }

  Get(pageIndex: any) {
    return this.http.get(this.baseUrl + '/Get/' + pageIndex,
      {
        reportProgress: true,
        observe: 'events',
        headers: new HttpHeaders({
          'Content-Type': 'application/json'
        })
      },
    );
  }

  UpdateRead(notificationId: any) {
    return this.http.post(this.baseUrl + '/UpdateRead/' + notificationId,
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
