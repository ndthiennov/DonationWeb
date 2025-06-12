import { NgFor, NgIf, NgOptimizedImage } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { CampaignService } from '../../../Services/campaign.service';
import { ActivatedRoute, Router } from '@angular/router';
import { HttpEventType } from '@angular/common/http';
import { CampaignStatisticsService } from '../../../Services/campaign-statistics.service';
import { ImageCampaignService } from '../../../Services/image-campaign.service';
import { SharedService } from '../../../Shared/shared.service';
import { CampaignParticipantService } from '../../../Services/campaign-participant.service';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { DonationService } from '../../../Services/donation.service';

import * as signalR from '@microsoft/signalr';
import { RateCampaignService } from '../../../Services/rate-campaign.service';
import { ExpenseService } from '../../../Services/expense.service';

@Component({
  selector: 'app-campaign-detail',
  standalone: true,
  imports: [NgOptimizedImage, NgFor, NgIf, ReactiveFormsModule],
  templateUrl: './campaign-detail.component.html',
  styleUrl: './campaign-detail.component.css'
})
export class CampaignDetailComponent implements OnInit {

  constructor(private campaignService: CampaignService,
    private campaignStatisticsService: CampaignStatisticsService,
    private imageCampaignService: ImageCampaignService,
    private campaignParticipantService: CampaignParticipantService,
    private donationService: DonationService,
    private rateCampaignService: RateCampaignService,
    private expenseService: ExpenseService,
    public sharedService: SharedService,
    private router: Router,
    private route: ActivatedRoute
  ) {

  }

  // SignalR Hub
  hubConnection: signalR.HubConnection | any;

  userId: any;

  campaign: any;
  donatedTotal: number = 0;
  expensedTotal: number = 0;
  transferedTotal: number = 0;
  images1: Array<any> = [];
  images2: Array<any> = [];
  images3: Array<any> = [];
  pageIndexImages1: number = 0;
  pageIndexImages2: number = 0;
  pageIndexImages3: number = 0;
  requestImages1: boolean = true;
  requestImages2: boolean = true;
  requestImages3: boolean = true;

  participated: boolean = false;

  donationPageIndex: number = 0;
  donationResponse: any;
  donationRequest: boolean = true;
  donationList: Array<any> = [];
  donationSearchForm: FormGroup = new FormGroup({});

  expenseList: Array<any> = [];
  expenseSearchForm: FormGroup = new FormGroup({});

  ratingPageIndex: number = 0;
  ratingResponse: any;
  ratingRequest: boolean = true;
  ratingList: Array<any> = [];
  rateForm: FormGroup = new FormGroup({});


  ngOnInit(): void {

    this.userId = localStorage.getItem("userid");

    this.GetCampaign();
    this.CheckParticipated();

    this.GetTotal();
    this.GetImages(1);
    this.GetImages(2);
    this.GetImages(3);

    this.InitDonationSearch();
    this.GetDonationList();
    this.GetRatingList();
    this.InitRateForm();

    this.GetExpenseList();
    this.InitExpenseSearch()

    this.StartConnection();
    this.Listener();
  }

  // SignalR Hub
  StartConnection = () => {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl('http://localhost:5178/donationhub', {
        skipNegotiation: true,
        transport: signalR.HttpTransportType.WebSockets
      })
      .build();

    this.hubConnection
      .start()
      .then(() => {
        console.log('Hub connection started');
      })
      .catch((err: any) => console.log(err))
  }

  Listener() {
    var id = this.route.snapshot.paramMap.get('id');
    var campaignId: number = +id!;

    this.hubConnection.on("Campaign:" + campaignId, (response: any) => {
      this.donatedTotal = response.campaignDonationTotal;
      console.log(response);
    })
  }

  GetCampaign() {
    var id = this.route.snapshot.paramMap.get('id');
    var campaignId: number = +id!;
    this.campaignService.GetById(campaignId).subscribe(
      (res: any) => {
        if (res.type === HttpEventType.Response) {
          this.campaign = res.body
          console.log(this.campaign);

          this.ReviewRating();
        }
      },
      err => {
        console.log(err);
      }
    )
  }

  GetTotal() {
    var id = this.route.snapshot.paramMap.get('id');
    var campaignId: number = +id!;
    this.campaignStatisticsService.GetById(campaignId).subscribe(
      (res: any) => {
        if (res.type === HttpEventType.Response) {
          this.donatedTotal = res.body.totalDonationAmount;
          this.expensedTotal = res.body.totalExpendedAmount;
          this.transferedTotal = res.body.totalTransferredAmount;
        }
      },
      err => {
        console.log(err);
      }
    )
  }

  CheckParticipated() {
    var id = this.route.snapshot.paramMap.get('id');
    var campaignId: number = +id!;

    this.campaignParticipantService.CheckParticipated(campaignId).subscribe(
      (res: any) => {
        if (res.type === HttpEventType.Response) {
          this.participated = res.body;
        }
      },
      err => {
        console.log(err);
      }
    )
  }

  JoinCampaign() {
    var id = this.route.snapshot.paramMap.get('id');
    var campaignId: number = +id!;

    this.campaignParticipantService.JoinCampaign(campaignId).subscribe(
      (res: any) => {
        this.participated = true;
      },
      err => {
        console.log(err);
      }
    )
  }

  CancelCampaignPartipation() {
    var id = this.route.snapshot.paramMap.get('id');
    var campaignId: number = +id!;

    this.campaignParticipantService.CancelCampaignPartipation(campaignId).subscribe(
      (res: any) => {
        if (res.type === HttpEventType.Response) {
          this.participated = false;
        }
      },
      err => {
        console.log(err);
      }
    )
  }

  GetImages(campaignStatusId: number) {
    let request: boolean = false;
    let pageIndex: number = 0;

    var id = this.route.snapshot.paramMap.get('id');
    var campaignId: number = +id!;

    if (campaignStatusId == 1) {
      if (this.requestImages1) {
        request = this.requestImages1;
        this.pageIndexImages1 += 1;
        pageIndex = this.pageIndexImages1;
      }
    }
    else if (campaignStatusId == 2) {
      if (this.requestImages2) {
        request = this.requestImages2;
        this.pageIndexImages2 += 1;
        pageIndex = this.pageIndexImages2;
      }
    } else {
      if (this.requestImages3) {
        request = this.requestImages3;
        this.pageIndexImages3 += 1;
        pageIndex = this.pageIndexImages3;
      }
    }

    if (request) {
      this.imageCampaignService.GetAll(pageIndex, campaignId, campaignStatusId).subscribe(
        (res: any) => {
          let response = res.body.$values;
          if (response.length < 8) {
            if (campaignStatusId == 1) {
              this.requestImages1 = false;
            }
            else if (campaignStatusId == 2) {
              this.requestImages2 = false;
            } else {
              this.requestImages3 = false;
            }
          }

          if (campaignStatusId == 1) {
            this.images1 = this.images1.concat(response);
          }
          else if (campaignStatusId == 2) {
            this.images2 = this.images2.concat(response);
          } else {
            this.images3 = this.images3.concat(response);
          }
          // this.campaignList = this.campaignList.concat(this.response);
          // console.log(res);
          // console.log(this.campaignList);
          // console.log(this.pageIndex)
        },
        err => {
          console.log(err);
        }
      )
    }
  }

  GetExpenseList(){
    var id = this.route.snapshot.paramMap.get('id');
    var campaignId: number = +id!;

    this.expenseService.GetListByCampaign(campaignId).subscribe(
      (res: any) => {
        let response = res.body.$values;
        this.expenseList = this.expenseList.concat(response);
        console.log(this.expenseList);
      },
      err => {
        console.log(err);
      }
    )
  }

  private InitExpenseSearch(): void {
    this.expenseSearchForm = new FormGroup({
      'fromdate': new FormControl("", Validators.required),
      'todate': new FormControl("", Validators.required)
    });
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
        this.sharedService.campaign = campaign;
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

  FilterSubmit() {
    this.donationRequest = true;
    this.donationSearchForm.value.pageindex = 0;
    this.donationList = [];
    this.GetDonationList();
  }

  GetDonationList() {
    if (this.donationRequest) {
      // this.donationSearchForm.value.pageindex += 1;
      this.donationSearchForm.get("pageindex")?.setValue(this.donationSearchForm.value.pageindex += 1);
      console.log(this.donationSearchForm.value.pageindex);

      var id = this.route.snapshot.paramMap.get('id');
      var campaignId: number = +id!;

      this.donationService.GetSearchedListByCampaignId(campaignId, this.donationSearchForm.value).subscribe(
        (res: any) => {
          if (res.type === HttpEventType.Response) {
            this.donationResponse = res.body.$values;
            if (this.donationResponse.length < 10) {
              this.donationRequest = false;
            }
            this.donationList = this.donationList.concat(this.donationResponse);
            console.log(res);
            console.log(this.donationList);
          }
        },
        err => {
          console.log(err);
        }
      )
    }
  }

  private InitDonationSearch(): void {
    this.donationSearchForm = new FormGroup({
      'fromdate': new FormControl("", Validators.required),
      'todate': new FormControl("", Validators.required),
      'donor': new FormControl("", Validators.required),
      'orderby': new FormControl("", Validators.required),
      'pageindex': new FormControl(0, Validators.required),
    });
  }

  GetRatingList() {
    if (this.ratingRequest) {
      this.ratingPageIndex += 1;

      var id = this.route.snapshot.paramMap.get('id');
      var campaignId: number = +id!;

      this.rateCampaignService.GetListByCampaignId(campaignId, this.ratingPageIndex).subscribe(
        (res: any) => {
          this.ratingResponse = res.body.$values;
          if (this.ratingResponse.length < 5) {
            this.ratingRequest = false;
          }
          this.ratingList = this.ratingList.concat(this.ratingResponse);
          console.log(res);
        },
        (err:any) => {
          console.log(err);
        }
      )
    }
  }

  RatingRemove() {
    var id = this.route.snapshot.paramMap.get('id');
    var campaignId: number = +id!;

    this.rateCampaignService.RemoveByDonorId(campaignId).subscribe(
      (res: any) => {
        this.ratingList.forEach((item, index) => {
          if (item.donorId == this.userId) this.ratingList.splice(index, 1);
        });

        this.ConfirmationClose();
      },
      err => {
        console.log(err);
      }
    )
  }

  InitRateForm(): void {

    let allStar = document.querySelectorAll('.rate-wrapper .rating .star')
    const ratingValue = <HTMLInputElement>document.querySelector('.rating input')

    var textarea = <HTMLInputElement>document.getElementById("rate-comment");
    textarea.value = "";

    allStar.forEach(i => {
      i.classList.replace('bi-star-fill', 'bi-star')
      i.classList.remove('active')
    })

    var id = this.route.snapshot.paramMap.get('id');
    var campaignId: number = +id!;

    let donorId: number = +this.userId;

    this.rateForm = new FormGroup({
      'campaignid': new FormControl(campaignId, Validators.required),
      'donorid': new FormControl(donorId, Validators.required),
      'rate': new FormControl(null, Validators.required),
      'comment': new FormControl(null, Validators.required),
      'rateddate': new FormControl(null, Validators.required),
      'campaign': new FormControl(null, Validators.required),
      'donor': new FormControl(null, Validators.required),
    });
  }

  Rate() {
    console.log(this.rateForm.value);
    this.rateCampaignService.Add(this.rateForm.value).subscribe(
      (res: any) => {
        if (res.type === HttpEventType.Response) {
          let response = res.body;
          response.donorAva = localStorage.getItem("userava");
          response.donorName = localStorage.getItem("username");

          console.log(response)

          let tempt = [response].concat(this.ratingList);

          this.ratingList = tempt;

          this.InitRateForm();
        }
      },
      err => {
        console.log(err);
      }
    )
  }

  //Export File Excell Donation and Expensese
  startDate!: string; // Ngày bắt đầu
  endDate!: string;   // Ngày kết thúc
  exportDonationFile() {
    const campaignId = this.campaign?.id;

    if (!campaignId) {
      alert('Không tìm thấy ID của chiến dịch.');
      return;
    }

    // Lấy giá trị từ form group
    this.startDate = this.donationSearchForm.get('fromdate')?.value;
    this.endDate = this.donationSearchForm.get('todate')?.value;

    // Kiểm tra giá trị trước khi gọi API
    if (!this.startDate || !this.endDate) {
      alert('Vui lòng chọn ngày bắt đầu và ngày kết thúc hợp lệ.');
      return;
    }

    // Gọi API xuất file với campaignId
    this.campaignService.exportDonations(campaignId, this.startDate, this.endDate).subscribe(
      (response) => {
        this.campaignService.downloadFile(response, `Donations_Campaign_${campaignId}.xlsx`);
      },
      (error) => {
        console.error('Lỗi khi xuất file Donations:', error);
      }
    );
  }

  //   exportDonationFile(campaignId: any) {
  //     // Lấy giá trị từ form group

  //     this.startDate = this.donationSearchForm.get('fromdate')?.value;
  //     this.endDate = this.donationSearchForm.get('todate')?.value;

  //     // Kiểm tra giá trị trước khi gọi API
  //     if (!this.startDate || !this.endDate) {
  //         alert('Vui lòng chọn ngày bắt đầu và ngày kết thúc hợp lệ.');
  //         return;
  //     }

  //     // Gọi API xuất file với campaignId
  //     this.campaignService.exportDonations(campaignId, this.startDate, this.endDate).subscribe(
  //         (response) => {
  //             this.campaignService.downloadFile(response, `Donations_Campaign_${campaignId}.xlsx`);
  //         },
  //         (error) => {
  //             console.error('Lỗi khi xuất file Donations:', error);
  //         }
  //     );
  // }


  //   // Gọi API xuất file
  //   this.campaignService.exportDonations(this.startDate, this.endDate).subscribe(
  //     (response) => {
  //       this.campaignService.downloadFile(response, 'Donations.xlsx');
  //     },
  //     (error) => {
  //       console.error('Lỗi khi xuất file Donations:', error);
  //     }
  //   );
  // }

  exportExpenseFile() {
    const campaignId = this.campaign?.id;

    if (!campaignId) {
      alert('Không tìm thấy ID của chiến dịch.');
      return;
    }
    this.startDate = this.expenseSearchForm.get('fromdate')?.value;
    this.endDate = this.expenseSearchForm.get('todate')?.value;

    if (!this.startDate || !this.endDate) {
      alert('Vui lòng chọn ngày bắt đầu và ngày kết thúc hợp lệ.');
      return;
    }

    this.campaignService.exportExpenses(campaignId, this.startDate, this.endDate).subscribe(
      (response) => {
        this.campaignService.downloadFile(response, `Expenses_${campaignId}.xlsx`);
      },
      (error) => {
        console.error('Lỗi khi xuất file Expenses:', error);
      }
    );
  }

  // ----- Design -----
  ViewImage() {
    console.log("hello");
  }

  TabOnClick(id: number) {
    let tabs = document.querySelectorAll(".tabs button");
    let tabContents = document.querySelectorAll(".tab-content .tab-content-div");

    tabContents.forEach((content) => {
      content.classList.remove("active");
    });
    tabs.forEach((tab) => {
      tab.classList.remove("active");
    });
    tabContents[id].classList.add("active");
    tabs[id].classList.add("active");
  }

  SortDropDown() {
    var dropdown = document.querySelector(".dropdown");
    dropdown?.classList.toggle("active");
  }

  ChangeOption(command: string) {
    var textBox = <HTMLInputElement>document.querySelector(".text-box");

    if (command == "ascending") {
      textBox.value = "Đóng góp tăng dần";
      this.donationSearchForm.get("orderby")?.setValue("asc");
      // this.donationSearchForm.value.orderby = "asc";
    }
    else if (command == "decreasing") {
      textBox.value = "Đóng góp giảm dần";
      this.donationSearchForm.get("orderby")?.setValue("desc");
      // this.donationSearchForm.value.orderby = "desc";
    }
  }

  StartChange(rateNum: number) {
    let allStar = document.querySelectorAll('.rate-wrapper .rating .star')
    const ratingValue = <HTMLInputElement>document.querySelector('.rating input')

    allStar.forEach(i => {
      i.classList.replace('bi-star-fill', 'bi-star')
      i.classList.remove('active')
    })

    for (let i = 0; i < allStar.length; i++) {
      if (i <= rateNum - 1) {
        allStar[i].classList.replace('bi-star', 'bi-star-fill')
        allStar[i].classList.add('active')
      } else {
        // allStar[i].style.setProperty('--i', click)
        // click++
      }
    }

    this.rateForm.get("rate")?.setValue(rateNum);
  }

  ReviewRating() {
    let allStar = document.querySelectorAll('.reviews .review .rating .star')

    console.log(allStar);
    for (let i = 0; i < this.campaign.ratedByRecipient; i++) {
      if (i <= this.campaign.ratedByRecipient - 1) {
        allStar[i].classList.replace('bi-star', 'bi-star-fill')
      } else {
        // allStar[i].style.setProperty('--i', click)
        // click++
      }
    }
  }

  RatingRemovePopUp() {
    let section = document.getElementById("confirmation");
    section?.classList.add("active");
  }

  ConfirmationClose() {
    let section = document.getElementById("confirmation");
    section?.classList.remove("active");
  }
}
