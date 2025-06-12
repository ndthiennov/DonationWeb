import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class CampaignParticipantService {

  readonly baseUrl = 'http://localhost:5178/api/CampaignParticipant';

  constructor(private http: HttpClient) { }

  CheckParticipated(campaignId: any) {
    return this.http.get(this.baseUrl + '/CheckParticipated/' + campaignId,
      {
        reportProgress: true,
        observe: 'events',
        headers: new HttpHeaders({
          'Content-Type': 'application/json'
        })
      },
    );
  }

  JoinCampaign(campaignId: any) {
    return this.http.post(this.baseUrl + '/JoinCampaign/' + campaignId,
      {
        reportProgress: true,
        observe: 'events',
        headers: new HttpHeaders({
          'Content-Type': 'application/json'
        })
      }
    );
  }

  CancelCampaignPartipation(campaignId: any) {
    return this.http.delete(this.baseUrl + '/CancelCampaignPartipation/' + campaignId,
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
