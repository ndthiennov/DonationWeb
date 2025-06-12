import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { CampaignService } from '../../../Services/campaign.service';
import { SharedService } from '../../../Shared/shared.service';
import { ActivatedRoute, Router, RouterLink, RouterOutlet } from '@angular/router';
import { RecipientService } from '../../../Services/recipient.service';
import { NgFor, NgIf } from '@angular/common';

@Component({
  selector: 'app-profile-recipient',
  standalone: true,
  imports: [RouterOutlet, RouterLink, ReactiveFormsModule, NgFor, NgIf],
  templateUrl: './profile-recipient.component.html',
  styleUrl: './profile-recipient.component.css'
})
export class ProfileRecipientComponent implements OnInit{

  constructor(private recipientService: RecipientService,
    private campaignService: CampaignService,
    public sharedService: SharedService,
    private router: Router,
    private route: ActivatedRoute) {

  }

  username: string | null = null;
  userava: string | null = null;
  recipientId: number = -1;
  recipient: any;

  // Campaign
  pageIndex: number = 0;
  response: any;
  request: boolean = true;
  campaignList: Array<any> = [];
  campaign: any;
  campaignStatus: any;
  searchForm: FormGroup = new FormGroup({});

  ngOnInit(): void {
    var id = this.route.snapshot.paramMap.get('id');
    this.recipientId = +id!;

    this.GetProfile();
    this.InitSearch();
    this.GetCampaignList();
  }

  GetProfile() {
    this.recipientService.GetById(this.recipientId).subscribe(
      (res: any) => {
        this.recipient = res.body
        console.log(res)
      },
      (err:any) => {
        console.log(err);
      }
    )
  }

  // Campaign list
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
      this.campaignService.GetSearchedListByRecipient(this.pageIndex, this.searchForm.value).subscribe(
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
        (err:any) => {
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
}
