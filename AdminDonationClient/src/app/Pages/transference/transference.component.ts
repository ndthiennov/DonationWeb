import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { TransferService } from './transfer.service';

@Component({
  selector: 'app-transference',
  templateUrl: './transference.component.html',
  styleUrls: ['./transference.component.css'],
})
export class TransferenceComponent implements OnInit {
  transferForm: FormGroup;
  isCreateMode: boolean = true;
  transfers: any[] = [];
  campaigns: any[] = [];
  searchCampaignId: number | null = null;
  currentPage: number = 1;
  itemsPerPage: number = 5;

  constructor(private fb: FormBuilder, private transferService: TransferService) {
    this.transferForm = this.fb.group({
      description: [''],
      transDate: [''],
      amount: [''],
      campaignId: [''],
    });
  }

  ngOnInit(): void {
    this.loadTransfers();
    this.loadCampaigns();
  }

  loadTransfers() {
    this.transferService
      .getTransfers(this.searchCampaignId, this.currentPage, this.itemsPerPage)
      .subscribe((data: any) => {
        this.transfers = data.transferences;
        this.currentPage++;
      });
  }

  loadCampaigns() {
    this.transferService.getCampaigns().subscribe((data: any) => {
      this.campaigns = data;
    });
  }

  searchTransfers() {
    this.currentPage = 1;
    this.loadTransfers();
  }

  onSubmit() {
    const formData = this.transferForm.value;

    if (this.isCreateMode) {
      this.transferService.createTransfer(formData).subscribe(() => this.loadTransfers());
    } else {
      this.transferService.updateTransfer(formData.id, formData).subscribe(() => this.loadTransfers());
    }
  }

  editTransfer(id: number) {
    this.isCreateMode = false;
    this.transferService.getTransferById(id).subscribe((data:any) => {
      this.transferForm.patchValue(data);
    });
  }

  deleteTransfer(id: number) {
    this.transferService.deleteTransfer(id).subscribe(() => this.loadTransfers());
  }

  onPageChange(page: number) {
    this.currentPage = page;
    this.loadTransfers();
  }
}
