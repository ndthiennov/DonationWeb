import { Component, OnInit } from '@angular/core';
import { CampaignService } from '../../Services/campaign.service';
import { Router } from '@angular/router';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { NgFor, NgIf } from '@angular/common';
import { SharedService } from '../../Shared/shared.service';

@Component({
  selector: 'app-campaign',
  standalone: true,
  imports: [NgFor, NgIf, ReactiveFormsModule],
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
  campaignStatus:any;
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
      this.campaignService.GetSearchedListByAdmin(this.pageIndex, this.searchForm.value).subscribe(
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
          this.ActiveToast(false, "Can not find this campaign");
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

  ChangeProvinceSearch(province:any){
    var textBox = <HTMLInputElement>document.querySelector(".text-box");
    textBox.value = province;

    if(province == "Tất cả"){
      this.searchForm.value.city = "";
    }
    else{
      this.searchForm.value.city = province;
    }
  }

  CityDropdownOpen(){
    var dropdown = document.querySelector(".dropdown");
    dropdown?.classList.toggle("active");
  }

  DisabledConfirmation() {
    let disabled = false;
    if (this.campaign.disabled == "Active") {
      disabled = true;
    }
    this.campaignService.UpdateDisabledCampaign(this.campaign.id, JSON.stringify(disabled)).subscribe(
      (res: any) => {
        if (disabled) {
          this.campaign.disabled = 'Disabled';
        }
        else {
          this.campaign.disabled = 'Active';
        }
        this.ActiveToast(true, "Your changes have been saved");
        let section = document.getElementById("confirmation");
        section?.classList.remove("active");
      },
      (err: any) => {
        this.ActiveToast(false, disabled ? "Disabled campaign failed" : "Active campaign failed");
        console.log(err);
      }
    )
  }

  Search() {
    let section = document.getElementById("search-form-container");
    section?.classList.add("active");
  }

  SearchClose() {
    let section = document.getElementById("search-form-container");
    section?.classList.remove("active");
  }

  Confirmation(campaign: any) {
    let section = document.getElementById("confirmation");
    section?.classList.add("active");

    this.campaign = campaign;
    this.campaign.disabled == "Active" ? this.campaignStatus = "disabled" : this.campaignStatus = "avtice";
  }

  ConfirmationClose() {
    let section = document.getElementById("confirmation");
    section?.classList.remove("active");
  }

  ActiveToast(isSuccess: boolean, response: string) {
    var toastContainer = document.getElementById("toast-container");
    var progress = document.getElementById("progress");
    var toastTitle = document.getElementById("toast-title");
    var toastBody = document.getElementById("toast-body");
    var toastSuccessIcon = document.getElementById("toast-success");
    var toastErrorIcon = document.getElementById("toast-error");

    if (!isSuccess) {
      progress?.classList.add("error")
      toastContainer?.classList.add("error")
      toastTitle!.innerHTML = "Error";
      toastBody!.innerHTML = response;
      toastErrorIcon!.style.display = "flex";
      toastSuccessIcon!.style.display = "none";
    }
    else {
      progress?.classList.remove("error")
      toastContainer?.classList.remove("error")
      toastTitle!.innerHTML = "Success";
      toastBody!.innerHTML = response;
      toastErrorIcon!.style.display = "none";
      toastSuccessIcon!.style.display = "flex";
    }

    toastContainer?.classList.add("active")
    progress?.classList.add("active")

    setTimeout(() => {
      toastContainer?.classList.remove("active")
      progress?.classList.remove("active")
    }, 5000)
  }

}
