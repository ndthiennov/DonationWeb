import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class RateCampaignService {

  readonly baseUrl = 'http://localhost:5178/api/RateCampaign';

  constructor(private http: HttpClient) { }

  GetListByCampaignId(campaignId: any, pageIndex: any) {
    return this.http.get(this.baseUrl + '/GetListByCampaignId/' + campaignId + '/' + pageIndex,
      {
        reportProgress: true,
        observe: 'events',
        headers: new HttpHeaders({
          'Content-Type': 'application/json'
        })
      },
    );
  }

  Add(rateForm: any) {
    return this.http.post(this.baseUrl + '/Add', rateForm,
      {
        reportProgress: true,
        observe: 'events',
        headers: new HttpHeaders({
          'Content-Type': 'application/json'
        })
      },
    );
  }

  RemoveByDonorId(campaignId: any) {
    return this.http.delete(this.baseUrl + '/RemoveByDonorId/' + campaignId,
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
