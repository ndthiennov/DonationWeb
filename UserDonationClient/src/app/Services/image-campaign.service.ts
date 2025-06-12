import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ImageCampaignService {

  readonly baseUrl = 'http://localhost:5178/api/ImageCampaign';

  constructor(private http: HttpClient) { }

  GetAll(pageIndex: any, campaignId: any, campaignStatusId:any) {
    return this.http.get(this.baseUrl + '/GetAll/' + pageIndex + "/" + campaignId + "/" + campaignStatusId,
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
