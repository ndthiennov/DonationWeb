import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class CampaignStatisticsService {

  readonly baseUrl = 'http://localhost:5178/api/CampaignStatistics';

  constructor(private http: HttpClient) { }

  GetById(campaignId: any) {
    return this.http.get(this.baseUrl + '/GetById/' + campaignId,
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
