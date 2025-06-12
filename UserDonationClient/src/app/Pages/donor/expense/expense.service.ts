import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class ExpenseService {
  private baseUrl = 'http://localhost:5000/api/expense'; // Thay bằng URL backend

  constructor(private http: HttpClient) {}

  getExpensesByCampaignId(campaignId: number, page: number, pageSize: number) {
    return this.http.get(`${this.baseUrl}/GetAll?campaignId=${campaignId}&page=${page}&pageSize=${pageSize}`);
  }

  addExpense(expense: any) {
    return this.http.post(`${this.baseUrl}/Add`, expense);
  }

  updateExpense(expense: any) {
    return this.http.put(`${this.baseUrl}/Update/${expense.id}`, expense);
  }

  deleteExpense(id: number) {
    return this.http.delete(`${this.baseUrl}/Delete/${id}`);
  }
}