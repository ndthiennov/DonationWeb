import { Component, OnInit } from '@angular/core';
import { DonorService } from '../../../Services/donor.service';
import { AccountService } from '../../../Services/account.service';
import { Router } from '@angular/router';
import { NgFor, NgIf } from '@angular/common';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { HttpEventType } from '@angular/common/http';

@Component({
  selector: 'app-donor',
  standalone: true,
  imports: [NgFor, NgIf, ReactiveFormsModule],
  templateUrl: './donor.component.html',
  styleUrl: './donor.component.css'
})
export class DonorComponent implements OnInit {

  constructor(private donorService: DonorService,
    private accountService: AccountService,
    private router: Router) {

  }

  pageIndex: number = 0;
  response: any;
  request: boolean = true;
  donorList: Array<any> = [];
  donor: any;
  donorStatus: any;
  text: string = "";
  addForm: FormGroup = new FormGroup({});

  ngOnInit(): void {
    this.GetDonorList();
    this.InitAdd();
  }

  SearchSubmit(event: any) {
    this.request = true;
    this.pageIndex = 0;
    this.text = event.target.value;
    this.donorList = [];
    this.GetDonorList();
  }

  GetDonorList() {
    if (this.request) {
      this.pageIndex += 1;
      if (this.text != "" && this.text != null) {
        this.donorService.GetSearchedList(this.pageIndex, JSON.stringify(this.text)).subscribe(
          (res: any) => {
            if (res.type === HttpEventType.UploadProgress) {
              console.log('Upload Progress: ' + (res.loaded / res.total * 100) + '%');
              // ===== Loader =====
              var loader = document.getElementById("loader");
              loader!.style.display = "block";
            }
            if (res.type === HttpEventType.Response) {
              this.response = res.body.$values;
              if (this.response.length < 20) {
                this.request = false;
              }
              this.donorList = this.donorList.concat(this.response);
              // ===== Loader =====
              var loader = document.getElementById("loader");
              loader!.style.display = "none";
            }
          },
          err => {
            localStorage.removeItem('token');
            this.router.navigateByUrl('/login');
            console.log(err);
          }
        )
      }
      else {
        this.donorService.GetAll(this.pageIndex).subscribe(
          (res: any) => {
            if (res.type == HttpEventType.UploadProgress) {
              console.log('Upload Progress: ' + (res.loaded / res.total * 100) + '%');
              // ===== Loader =====
              var loader = document.getElementById("loader");
              loader!.style.display = "block";
            }
            if (res.type === HttpEventType.Response) {
              this.response = res.body.$values;
              if (this.response.length < 20) {
                this.request = false;
              }
              this.donorList = this.donorList.concat(this.response);
              console.log(this.donorList);
              // ===== Loader =====
              var loader = document.getElementById("loader");
              loader!.style.display = "none";
            }
          },
          err => {
            localStorage.removeItem('token');
            this.router.navigateByUrl('/login');
          }
        )
      }
    }
  }

  DisabledConfirmation() {
    let disabled = false;
    if (this.donor.disabled == "Active") {
      disabled = true;
    }
    this.accountService.UpdateDisabledAccount(this.donor.phoneNum, JSON.stringify(disabled)).subscribe(
      (res: any) => {
        if (res.type === HttpEventType.UploadProgress) {
          console.log('Upload Progress: ' + (res.loaded / res.total * 100) + '%');
        }
        if (res.type === HttpEventType.Response) {
          if (disabled) {
            this.donor.disabled = 'Disabled';
          }
          else {
            this.donor.disabled = 'Active';
          }
          this.ActiveToast(true, "Your changes have been saved");
        }

        let section = document.getElementById("confirmation");
        section?.classList.remove("active")
      },
      err => {
        this.ActiveToast(false, disabled ? "Disabled account failed" : "Active account failed");
        console.log(err);
      }
    )
  }

  private InitAdd(): void {
    this.addForm = new FormGroup({
      'phonenum': new FormControl(null, Validators.required),
      'password': new FormControl(null, Validators.required),
      'name': new FormControl(null, Validators.required),
      'gender': new FormControl(null, Validators.required),
      'dob': new FormControl(null, Validators.required),
      'email': new FormControl(null, Validators.required),
      'address': new FormControl(null, Validators.required),
      'disabled': new FormControl(null, Validators.required),
      'code': new FormControl(null, Validators.required),
    });
  }

  AddSubmit() {
    if (/[^0-9]/.test(this.addForm.value.phonenum)) {
      this.ActiveToast(false, "Số điện thoại chỉ chứa số");
    }
    else {
      this.accountService.AddDonorAccount(this.addForm.value).subscribe(
        (res: any) => {
          if (res.type === HttpEventType.UploadProgress) {
            console.log('Upload Progress: ' + (res.loaded / res.total * 100) + '%');
          }
          if (res.type === HttpEventType.Response) {
            this.ActiveToast(true, "Your addition have been saved");
          }

          let section = document.getElementById("add-form-container");
          section?.classList.remove("active")

          this.InitAdd();
        },
        err => {
          this.ActiveToast(false, "Your addition failed");
          console.log(err);

          this.InitAdd();
        }
      )
    }
  }

  Add() {
    let section = document.getElementById("add-form-container");
    section?.classList.add("active");
  }

  AddClose() {
    let section = document.getElementById("add-form-container");
    section?.classList.remove("active");

    this.InitAdd();
  }

  Confirmation(admin: any) {
    let section = document.getElementById("confirmation");
    section?.classList.add("active");

    this.donor = admin;
    this.donor.disabled == "Active" ? this.donorStatus = "disabled" : this.donorStatus = "avtice";
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
