import { Component, OnInit } from '@angular/core';
import { SharedService } from '../../../Shared/shared.service';
import { Router } from '@angular/router';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { DonationService } from '../../../Services/donation.service';
import { HttpEventType } from '@angular/common/http';

@Component({
  selector: 'app-donation',
  standalone: true,
  imports: [],
  templateUrl: './donation.component.html',
  styleUrl: './donation.component.css'
})
export class DonationComponent implements OnInit {

  constructor(public donationService: DonationService,
    public sharedService: SharedService,
    private router: Router) {

  }

  date: any;
  campaign: any;
  paymentRequestForm: FormGroup = new FormGroup({});

  ngOnInit(): void {
    this.campaign = this.sharedService.campaign;
    var userIdString = localStorage.getItem('userid')
    var userIdNumber: number | null = userIdString == null ? null : +userIdString;
    this.paymentRequestForm = new FormGroup({
      'campaignid': new FormControl(this.campaign.id, Validators.required),
      'userid': new FormControl(userIdNumber, Validators.required),
      'userrole': new FormControl("donor", Validators.required),
      'amount': new FormControl(0, Validators.required),
      'paymentmethod': new FormControl(null, Validators.required)
    });
    let dateTime = new Date()
    this.date = dateTime.getDate() + '/' + (dateTime.getMonth() + 1) + '/' + dateTime.getFullYear();
    console.log(this.paymentRequestForm.value)
  }

  EnterTotal(e: any) {
    let total = document.getElementById("total");
    total!.innerText = e.target.value;
    this.paymentRequestForm.value.amount = e.target.value;
    console.log(this.paymentRequestForm.value)
  }

  PaymentMethod(method: any) {
    let paymentMethod = document.getElementById("payment-method");
    paymentMethod!.innerText = method;
    this.paymentRequestForm.value.paymentmethod = method;
    console.log(this.paymentRequestForm.value)
  }

  Payment() {
    this.donationService.CreatePaymentUrl(this.paymentRequestForm.value).subscribe(
      (res: any) => {
        if (res.type === HttpEventType.Response) {
          window.open(res.body, '_blank');
          //console.log(res.body);
        }
      },
      err => {
        console.log(err);
      }
    )
  }
}
