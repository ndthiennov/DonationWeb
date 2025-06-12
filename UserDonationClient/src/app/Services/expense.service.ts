import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ExpenseService {

  readonly baseUrl = 'http://localhost:5178/api/Expense';

  constructor(private http: HttpClient) { }

  GetListByCampaign(campaignId: any) {
    return this.http.get(this.baseUrl + '/GetListByCampaign/' + campaignId,
      {
        reportProgress: true,
        observe: 'events',
        headers: new HttpHeaders({
          'Content-Type': 'application/json'
        })
      },
    );
  }

  Add(campaignId: any, expenseForm: any) {
      return this.http.put(this.baseUrl + '/Add/' + campaignId, expenseForm,
        {
          reportProgress: true,
          observe: 'events',
          headers: new HttpHeaders({
            'Content-Type': 'application/json'
          })
        },
      );
    }
  
    Delete(expenseId: any, campaignId: any, expenseForm: any) {
      return this.http.put(this.baseUrl + '/Delete/' + expenseId + "/" + campaignId, expenseForm,
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
