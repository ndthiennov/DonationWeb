import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class CampaignService {

  readonly baseUrl = 'http://localhost:5178/api/Campaign';

  constructor(private http: HttpClient) { }

  GetSearchedListByUser(pageIndex: any, searchForm: any) {
    return this.http.post(this.baseUrl + '/GetSearchedListByUser/' + pageIndex, searchForm,
      {
        reportProgress: true,
        observe: 'events',
        headers: new HttpHeaders({
          'Content-Type': 'application/json'
        })
      },
    );
  }

  GetSearchedListByOrganiser(pageIndex: any, searchForm: any) {
    return this.http.post(this.baseUrl + '/GetSearchedListByOrganiser/' + pageIndex, searchForm,
      {
        reportProgress: true,
        observe: 'events',
        headers: new HttpHeaders({
          'Content-Type': 'application/json'
        })
      },
    );
  }

  GetSearchedListByRecipient(pageIndex: any, searchForm: any) {
    return this.http.post(this.baseUrl + '/GetSearchedListByRecipient/' + pageIndex, searchForm,
      {
        reportProgress: true,
        observe: 'events',
        headers: new HttpHeaders({
          'Content-Type': 'application/json'
        })
      },
    );
  }

  UpdateRecivedByRecipient(campaignId: any, received: any) {
    return this.http.put(this.baseUrl + '/UpdateRecivedByRecipient/' + campaignId, received,
      {
        reportProgress: true,
        observe: 'events',
        headers: new HttpHeaders({
          'Content-Type': 'application/json'
        })
      },
    );
  }

  UpdateRatedByRecipient(campaignId: any, rateForm: any) {
    return this.http.put(this.baseUrl + '/UpdateRatedByRecipient/' + campaignId, rateForm,
      {
        reportProgress: true,
        observe: 'events',
        headers: new HttpHeaders({
          'Content-Type': 'application/json'
        })
      },
    );
  }

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

  Add(cuFormData: any) {
    return this.http.post(this.baseUrl + '/Add', cuFormData,
      {
        reportProgress: true,
        observe: 'events'
      },
    );
  }

  Update(campaignId: any, cuFormData: any) {
    return this.http.put(this.baseUrl + '/Update/' + campaignId, cuFormData,
      {
        reportProgress: true,
        observe: 'events'
      },
    );
  }

  exportDonations(campaignId: number, startDate: string, endDate: string) {
    const endpoint = `${this.baseUrl}/ExportDonationsToExcel?`;
    const params = {campaignId, startDate, endDate };
    return this.http.get(endpoint, { params, responseType: 'blob' });
  }

  exportExpenses(campaignId: number,startDate: string, endDate: string) {
    const endpoint = `${this.baseUrl}/ExportExpenseToExcel?`;
    const params = { campaignId, startDate, endDate };
    return this.http.get(endpoint, { params, responseType: 'blob' });
  }

  downloadFile(response: Blob, filename: string) {
    const blob = new Blob([response], {
      type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet',
    });
    const url = window.URL.createObjectURL(blob);
    const a = document.createElement('a');
    a.href = url;
    a.download = filename;
    a.click();
    window.URL.revokeObjectURL(url);
  }
}
