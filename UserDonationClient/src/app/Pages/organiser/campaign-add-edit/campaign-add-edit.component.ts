import { Component, OnInit } from '@angular/core';
import { CampaignService } from '../../../Services/campaign.service';
import { SharedService } from '../../../Shared/shared.service';
import { Router } from '@angular/router';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { HttpEventType } from '@angular/common/http';
import { NgFor } from '@angular/common';

@Component({
  selector: 'app-campaign-add-edit',
  standalone: true,
  imports: [ReactiveFormsModule, NgFor],
  templateUrl: './campaign-add-edit.component.html',
  styleUrl: './campaign-add-edit.component.css'
})
export class CampaignAddEditComponent implements OnInit {

  constructor(private campaignService: CampaignService,
    public sharedService: SharedService,
    private router: Router
  ) {

  }

  cuForm: FormGroup = new FormGroup({});
  cuFormData: any = new FormData();
  imagePreview: any = "/public/imagepreview.png"
  selectedFile: File | null = null;
  ngOnInit(): void {
    this.InitForm();
  }

  private InitForm(): void {
    if (this.sharedService.campaign == null) {
      this.cuForm = new FormGroup({
        'title': new FormControl(null, Validators.required),
        'target': new FormControl(null, Validators.required),
        'description': new FormControl(null, Validators.required),
        'startdate': new FormControl(null, Validators.required),
        'enddate': new FormControl(null, Validators.required),
        'address': new FormControl(null, Validators.required),
        'city': new FormControl(null, Validators.required),
        'statuscampaignid': new FormControl(null, Validators.required), //////
        'targetamount': new FormControl(null, Validators.required),
        'recipientid': new FormControl(null, Validators.required),
        'disabled': new FormControl(null, Validators.required)
      });

      this.selectedFile = null;
      this.imagePreview = "";
      this.cuFormData = new FormData();
    }

    else {
      this.cuForm = new FormGroup({
        'title': new FormControl(this.sharedService.campaign.title, Validators.required),
        'target': new FormControl(this.sharedService.campaign.target, Validators.required),
        'description': new FormControl(this.sharedService.campaign.description, Validators.required),
        'startdate': new FormControl(null, Validators.required),
        'enddate': new FormControl(null, Validators.required),
        'address': new FormControl(null, Validators.required),
        'city': new FormControl(null, Validators.required),
        'statuscampaignid': new FormControl(null, Validators.required), //////
        'targetamount': new FormControl(this.sharedService.campaign.targetAmount, Validators.required),
        'recipientid': new FormControl(this.sharedService.campaign.recipientId, Validators.required),
        'disabled': new FormControl(null, Validators.required)
      });

      this.selectedFile = null;
      this.imagePreview = this.sharedService.campaign.coverSrc;
      this.cuFormData = new FormData();

      this.InitFormDesign();
    }
  }

  Submit() {
    //  Create form data to add to [FromForm] of API
    this.cuFormData = new FormData();
    this.cuFormData.append("title", this.cuForm.get('title')?.value);
    this.cuFormData.append("target", this.cuForm.get('target')?.value);
    this.cuFormData.append("description", this.cuForm.get('description')?.value);
    this.cuFormData.append("startdate", this.cuForm.get('startdate')?.value);
    this.cuFormData.append("enddate", this.cuForm.get('enddate')?.value);
    this.cuFormData.append("address", this.cuForm.get('address')?.value);
    this.cuFormData.append("city", this.cuForm.get('city')?.value);
    this.cuFormData.append("statuscampaignid", this.cuForm.get('statuscampaignid')?.value);
    this.cuFormData.append("targetamount", this.cuForm.get('targetamount')?.value);
    if (this.selectedFile != null) {
      this.cuFormData.append("CoverSrc", this.selectedFile, this.selectedFile?.name);
    }
    this.cuFormData.append("recipientid", this.cuForm.get('recipientid')?.value);
    this.cuFormData.append("disabled", this.cuForm.get('disabled')?.value);

    console.log(this.cuFormData.get("CoverSrc"));

    if (this.sharedService.campaign == null) {
      this.campaignService.Add(this.cuFormData).subscribe(
        (res: any) => {
          if (res.type === HttpEventType.UploadProgress) {
            console.log('Upload Progress: ' + (res.loaded / res.total * 100) + '%');
          }
          if (res.type === HttpEventType.Response) {
            var userId = localStorage.getItem("userid");
            this.router.navigateByUrl('/organiser/profile/' + userId);
          }

          this.InitForm();
        },
        err => {
          console.log(err);

          // this.InitForm();
        }
      )
    }
    else {
      this.campaignService.Update(this.sharedService.campaign.id, this.cuFormData).subscribe(
        (res: any) => {
          if (res.type === HttpEventType.UploadProgress) {
            console.log('Upload Progress: ' + (res.loaded / res.total * 100) + '%');
          }
          if (res.type === HttpEventType.Response) {
            var userId = localStorage.getItem("userid");
            this.router.navigateByUrl('/organiser/profile/' + userId);
          }

          this.InitForm();
        },
        err => {
          console.log(err);

          // this.InitForm();
        }
      )
    }

  }

  UploadImg(event: any) {
    let reader = new FileReader();
    reader.readAsDataURL(event.target.files[0]);
    reader.onload = () => {
      this.imagePreview = reader.result;
    };
    this.selectedFile = <File>event.target.files[0];
  }

  ChangeProvinceSearch(province: any) {
    var textBox = <HTMLInputElement>document.querySelector(".text-box");
    textBox.value = province;

    if (province == "Tất cả") {
      // this.cuForm.value.city = "";
      this.cuForm.get("city")?.setValue("");
    }
    else {
      this.cuForm.get("city")?.setValue(province);
      // this.cuForm.value.city = province;
    }
  }

  CityDropdownOpen() {
    var dropdown = document.querySelector(".dropdown");
    dropdown?.classList.toggle("active");
  }

  InitFormDesign() {
    var title = <HTMLInputElement>document.getElementById("title");
    var target = <HTMLInputElement>document.getElementById("target");
    var description = <HTMLInputElement>document.getElementById("description");
    var startdate = <HTMLInputElement>document.getElementById("startdate");
    var enddate = <HTMLInputElement>document.getElementById("enddate");
    var address = <HTMLInputElement>document.getElementById("address");
    var city = <HTMLInputElement>document.getElementById("city");
    var amountTarget = <HTMLInputElement>document.getElementById("amount-target");
    var recipientid = <HTMLInputElement>document.getElementById("recipientid");

    title.value = this.sharedService.campaign.title;
    target.value = this.sharedService.campaign.target;
    description.value = this.sharedService.campaign.description;
    amountTarget.value = this.sharedService.campaign.amountTarget;
    recipientid.value = this.sharedService.campaign.recipientId;

    let [day, month, year] = this.sharedService.campaign.startDate.split('/');
    startdate.value = `${year}-${month.padStart(2, '0')}-${day.padStart(2, '0')}`
    this.cuForm.get("startdate")?.setValue(startdate.value);

    [day, month, year] = this.sharedService.campaign.endDate.split('/');
    enddate.value = `${year}-${month.padStart(2, '0')}-${day.padStart(2, '0')}`
    this.cuForm.get("enddate")?.setValue(enddate.value);

    let partsAddress = this.sharedService.campaign.address.split(',').map((part: any) => part.trim());
    address.value = partsAddress[0];
    this.cuForm.get("address")?.setValue(address.value);
    city.value = partsAddress[1];
    this.cuForm.get("city")?.setValue(city.value);
  }
}
