import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { SharedService } from '../../../Shared/shared.service';
import { NgFor } from '@angular/common';
import { CampaignService } from '../../../Services/campaign.service';
import { Router, RouterLink } from '@angular/router';
import { DonationService } from '../../../Services/donation.service';

@Component({
  selector: 'app-campaign',
  standalone: true,
  imports: [ReactiveFormsModule, NgFor, RouterLink],
  templateUrl: './campaign.component.html',
  styleUrl: './campaign.component.css'
})
export class CampaignComponent implements OnInit {

  constructor(private campaignService: CampaignService,
    public sharedService: SharedService,
    private router: Router) {

  }

  pageIndex: number = 0;
  response: any;
  request: boolean = true;
  campaignList: Array<any> = [];
  campaign: any;
  campaignStatus: any;
  searchForm: FormGroup = new FormGroup({});

  ngOnInit(): void {
    this.InitSearch();
    this.GetCampaignList();
  }

  SearchSubmit(event: any) {
    this.request = true;
    this.pageIndex = 0;
    this.campaignList = [];
    this.searchForm.value.campaign = event.target.value;
    this.GetCampaignList();
  }

  FilterSubmit() {
    this.request = true;
    this.pageIndex = 0;
    this.campaignList = [];
    this.GetCampaignList();
  }

  GetCampaignList() {
    if (this.request) {
      this.pageIndex += 1;
      this.campaignService.GetSearchedListByUser(this.pageIndex, this.searchForm.value).subscribe(
        (res: any) => {
          this.response = res.body.$values;
          if (this.response.length < 20) {
            this.request = false;
          }
          this.campaignList = this.campaignList.concat(this.response);
          console.log(res);
          console.log(this.campaignList);
          console.log(this.pageIndex)
        },
        err => {
          console.log(err);
        }
      )
    }
  }

  private InitSearch(): void {
    this.searchForm = new FormGroup({
      'campaign': new FormControl("", Validators.required),
      'user': new FormControl("", Validators.required),
      'startdate': new FormControl("", Validators.required),
      'enddate': new FormControl("", Validators.required),
      'city': new FormControl("", Validators.required),
    });
  }

  ChangeProvinceSearch(province: any) {
    var textBox = <HTMLInputElement>document.querySelector(".text-box");
    textBox.value = province;

    if (province == "Tất cả") {
      this.searchForm.value.city = "";
    }
    else {
      this.searchForm.value.city = province;
    }
  }

  CityDropdownOpen() {
    var dropdown = document.querySelector(".dropdown");
    dropdown?.classList.toggle("active");
  }

  Search() {
    let section = document.getElementById("search-form-container");
    section?.classList.add("active");
  }

  SearchClose() {
    let section = document.getElementById("search-form-container");
    section?.classList.remove("active");
  }

  DonateActiveLink(campaign: any) {
    if (localStorage.getItem('token')) {
      var payLoad = JSON.parse(window.atob(localStorage.getItem('token')!.split('.')[1]));
      var userRole = payLoad['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];
      if (userRole != "donor") {
        const url = this.router.serializeUrl(
          this.router.createUrlTree([`/auth/donor`])
        );
        window.open(url, '_blank');
      }
      else {
        // var userIdString = localStorage.getItem('userid')
        // var userIdNumber: number | null = userIdString == null ? null : +userIdString;
        // this.sharedService.paymentRequestForm = new FormGroup({
        //   'campaignid': new FormControl(campaign.id, Validators.required),
        //   'userid': new FormControl(userIdNumber, Validators.required),
        //   'userrole': new FormControl("donor", Validators.required),
        //   'amount': new FormControl(0, Validators.required),
        //   'paymentmethod': new FormControl(null, Validators.required)
        // });
        this.sharedService.campaign = campaign;

        // const url = this.router.serializeUrl(
        //   this.router.createUrlTree([`/donation`])
        // );
        // window.open(url, '_blank');

        this.router.navigateByUrl('/donation');
      }
    }
    else {
      const url = this.router.serializeUrl(
        this.router.createUrlTree([`/auth/donor`])
      );
      window.open(url, '_blank');
    }
  }
}
