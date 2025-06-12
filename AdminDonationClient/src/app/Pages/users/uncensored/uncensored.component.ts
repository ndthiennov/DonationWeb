import { NgFor, NgIf } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { OrganiserService } from '../../../Services/organiser.service';
import { AccountService } from '../../../Services/account.service';
import { Router } from '@angular/router';
import { HttpEventType } from '@angular/common/http';

@Component({
  selector: 'app-uncensored',
  standalone: true,
  imports: [NgFor, NgIf],
  templateUrl: './uncensored.component.html',
  styleUrl: './uncensored.component.css'
})
export class UncensoredComponent implements OnInit {

  constructor(private organiserService: OrganiserService,
    private accountService: AccountService,
    private router: Router) {

  }

  pageIndex: number = 0;
  response: any;
  request: boolean = true;
  uncensoredList: Array<any> = [];
  text: string = "";
  actionName: string = "";
  uncensored: any;

  ngOnInit(): void {
    this.GetUncensoredList();
  }

  SearchSubmit(event: any) {
    this.request = true;
    this.pageIndex = 0;
    this.text = event.target.value;
    this.uncensoredList = [];
    this.GetUncensoredList();
  }

  GetUncensoredList() {
    if (this.request) {
      this.pageIndex += 1;
      if (this.text != "" && this.text != null) {
        this.organiserService.GetSearchedUncensoredList(this.pageIndex, JSON.stringify(this.text)).subscribe(
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
              this.uncensoredList = this.uncensoredList.concat(this.response);
              // ===== Loader =====
              var loader = document.getElementById("loader");
              loader!.style.display = "none";
            }
          },
          err => {
            this.ActiveToast(false, "Can not find this account");
            console.log(err);
          }
        )
      }
      else {
        this.organiserService.GetAllUnCensored(this.pageIndex).subscribe(
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
              this.uncensoredList = this.uncensoredList.concat(this.response);
              console.log(this.uncensoredList);
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

  ViewImage(uncensored: any) {
    this.uncensored = uncensored;

    console.log(this.uncensored);
    let section = document.getElementById("image-popup");
    section?.classList.add("active");
  }

  CloseImage(){
    let section = document.getElementById("image-popup");
    section?.classList.remove("active");
  }

  Approve(uncensored: any) {
    this.uncensored = uncensored;
    this.actionName = "approve"

    let section = document.getElementById("confirmation");
    section?.classList.add("active");
    console.log(section);
  }

  Delete(uncensored: any) {
    this.uncensored = uncensored;
    this.actionName = "delete"

    let section = document.getElementById("confirmation");
    section?.classList.add("active");
  }

  ConfirmationAction() {
    if (this.actionName == "approve") {
      this.organiserService.UpdateApprovement(this.uncensored.id).subscribe(
        (res: any) => {
          this.uncensoredList.forEach((item, index) => {
            if (item.id === this.uncensored.id) this.uncensoredList.splice(index, 1);
          });
          this.ActiveToast(true, "Your approvement has been saved");
          console.log(this.uncensoredList);
          let section = document.getElementById("confirmation");
          section?.classList.remove("active")
        },
        err => {
          this.ActiveToast(false, "Approment failed");
          console.log(err);
        }
      )
    }
    else if (this.actionName == "delete") {
      this.accountService.DeleteUncensorOrganiserAccount(this.uncensored.accountId, this.uncensored.id).subscribe(
        (res: any) => {
          this.uncensoredList.forEach((item, index) => {
            if (item.id === this.uncensored.id) this.uncensoredList.splice(index, 1);
          });
          this.ActiveToast(true, "Account has been deleted");
          console.log(this.uncensoredList);
          let section = document.getElementById("confirmation");
          section?.classList.remove("active")
        },
        err => {
          this.ActiveToast(false, "Deletion failed");
          console.log(err);
        }
      )
    }
    else {

    }
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
