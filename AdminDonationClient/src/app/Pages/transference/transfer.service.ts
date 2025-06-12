import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({ providedIn: 'root' })
export class TransferService {
  private baseUrl = 'http://localhost:5178/api/transference';

  constructor(private http: HttpClient) {}

  getTransfers(campaignId: number | null, page: number, itemsPerPage: number) {
    return this.http.get(
      `${this.baseUrl}/GetAll?campaignId=${campaignId}&page=${page}&pageSize=${itemsPerPage}`
    );
  }

  getTransferById(id: number) {
    return this.http.get(`${this.baseUrl}/Detail/${id}`);
  }

  createTransfer(transfer: any) {
    return this.http.post(`${this.baseUrl}/Add`, transfer);
  }

  updateTransfer(id: number, transfer: any) {
    return this.http.put(`${this.baseUrl}/Update/${id}`, transfer);
  }

  deleteTransfer(id: number) {
    return this.http.delete(`${this.baseUrl}/Delete/${id}`);
  }

  getCampaigns() {
    return this.http.get('http://localhost:5000/api/campaign');
  }
}