import { Component, OnInit } from '@angular/core';
import { OrganiserService } from '../../../Services/organiser.service';
import { SharedService } from '../../../Shared/shared.service';
import { ActivatedRoute, Router, RouterLink, RouterOutlet } from '@angular/router';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { NgFor, NgIf } from '@angular/common';
import { CampaignService } from '../../../Services/campaign.service';

@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [RouterOutlet, RouterLink, ReactiveFormsModule, NgFor, NgIf],
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.css'
})
export class ProfileComponent implements OnInit {

  constructor(private organiserService: OrganiserService,
    private campaignService: CampaignService,
    public sharedService: SharedService,
    private router: Router,
    private route: ActivatedRoute) {

  }

  username: string | null = null;
  userava: string | null = null;
  organiserId: number = -1;
  organiser: any;

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
    this.organiserId = +id!;

    this.GetProfile();
    this.InitSearch();
    this.GetCampaignList();
  }

  GetProfile() {
    this.organiserService.GetById(this.organiserId).subscribe(
      (res: any) => {
        this.organiser = res.body
        console.log(res)
      },
      err => {
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
      this.campaignService.GetSearchedListByOrganiser(this.pageIndex, this.searchForm.value).subscribe(
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

  //Add Campaign
  AddCampaign(){
    this.sharedService.campaign = null;
    this.router.navigateByUrl('/organiser/campaign/addedit');
  }
}
