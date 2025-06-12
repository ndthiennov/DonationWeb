import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class CampaignService {

  readonly baseUrl = 'http://localhost:5178/api/Campaign';

  constructor(private http: HttpClient) { }

  GetListByAdmin(pageIndex: any) {
    return this.http.get(this.baseUrl + '/GetListByAdmin/' + pageIndex,
      {
        reportProgress: true,
        observe: 'events'
      });
  }

  GetSearchedListByAdmin(pageIndex: any, searchForm: any) {
    return this.http.post(this.baseUrl + '/GetSearchedListByAdmin/' + pageIndex, searchForm,
      {
        reportProgress: true,
        observe: 'events',
        headers: new HttpHeaders({ 
          'Content-Type': 'application/json' 
      })
      },
    );
  }

  UpdateDisabledCampaign(campaignId: any, disabled: any) {
    return this.http.put(this.baseUrl + '/UpdateDisabledCampaign/' + campaignId, disabled,
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
