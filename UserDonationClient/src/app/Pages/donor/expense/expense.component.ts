import { Component, Input, OnInit } from '@angular/core';
import { ExpenseService } from './expense.service';
// import { ExpenseService } from 'src/app/Services/expense.service';
// import { AuthService } from 'src/app/Services/auth.service';

@Component({
  selector: 'app-expense-list',
  templateUrl: './expense-list.component.html',
  styleUrls: ['./expense-list.component.css']
})
export class ExpenseListComponent implements OnInit {
  @Input() campaignId!: number; // Nhận campaignId từ cha (campaign-detail.component)
  expenses: any[] = [];
  currentPage: number = 1;
  pageSize: number = 10;
  hasMore: boolean = true;
  isOrganiser: boolean = false;

  newExpense = {
    description: '',
    expenseDate: '',
    amount: 0
  };
  editingExpense: any = null;

  constructor(private expenseService: ExpenseService) {}

  ngOnInit(): void {
    // this.isOrganiser = this.authService.isOrganiser(this.campaignId);
    this.loadExpenses();
  }

  loadExpenses(): void {
    this.expenseService
      .getExpensesByCampaignId(this.campaignId, this.currentPage, this.pageSize)
      .subscribe((data: any) => {
        if (data.expenses.length < this.pageSize) {
          this.hasMore = false;
        }
        this.expenses = [...this.expenses, ...data.expenses]; // Đã sửa để khớp với BE
      });
  }

  loadMore(): void {
    if (this.hasMore) {
      this.currentPage++;
      this.loadExpenses();
    }
  }

  addExpense(): void {
    this.expenseService.addExpense({ ...this.newExpense, campaignId: this.campaignId }).subscribe(() => {
      this.newExpense = { description: '', expenseDate: '', amount: 0 };
      this.currentPage = 1;
      this.expenses = [];
      this.loadExpenses();
    });
  }

  editExpense(expense: any): void {
    this.editingExpense = { ...expense };
  }

  saveEditExpense(): void {
    this.expenseService.updateExpense(this.editingExpense).subscribe(() => {
      this.editingExpense = null;
      this.currentPage = 1;
      this.expenses = [];
      this.loadExpenses();
    });
  }

  deleteExpense(expenseId: number): void {
    this.expenseService.deleteExpense(expenseId).subscribe(() => {
      this.currentPage = 1;
      this.expenses = [];
      this.loadExpenses();
    });
  }
}