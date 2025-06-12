import { Component, OnInit } from '@angular/core';
import { OrganiserService } from '../../../Services/organiser.service';
import { AccountService } from '../../../Services/account.service';
import { Router } from '@angular/router';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { NgFor, NgIf } from '@angular/common';
import { HttpEventType } from '@angular/common/http';

@Component({
  selector: 'app-organiser',
  standalone: true,
  imports: [NgFor, NgIf, ReactiveFormsModule],
  templateUrl: './organiser.component.html',
  styleUrl: './organiser.component.css'
})
export class OrganiserComponent implements OnInit{

  constructor(private organiserService: OrganiserService,
    private accountService: AccountService,
    private router: Router) {
    
  }

  pageIndex: number = 0;
  response: any;
  request: boolean = true;
  organiserList: Array<any> = [];
  organiser:any;
  organiserStatus:any;
  text: string = "";
  addForm: FormGroup = new FormGroup({});
  addFormData: any = new FormData();
  imagePreview: any = "/public/imagepreview.png"
  selectedFile: File | null = null;

  ngOnInit(): void {
    this.GetOrganiserList();
    this.InitAdd();
  }

  SearchSubmit(event: any) {
    this.request = true;
    this.pageIndex = 0;
    this.text = event.target.value;
    this.organiserList = [];
    this.GetOrganiserList();
  }

  GetOrganiserList() {
    if (this.request) {
      this.pageIndex += 1;
      if (this.text != "" && this.text != null) {
        this.organiserService.GetSearchedList(this.pageIndex, JSON.stringify(this.text)).subscribe(
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
              this.organiserList = this.organiserList.concat(this.response);
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
        this.organiserService.GetAll(this.pageIndex).subscribe(
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
              this.organiserList = this.organiserList.concat(this.response);
              console.log(this.organiserList);
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
    if (this.organiser.disabled == "Active") {
      disabled = true;
    }
    this.accountService.UpdateDisabledAccount(this.organiser.phoneNum, JSON.stringify(disabled)).subscribe(
      (res: any) => {
        if (res.type === HttpEventType.UploadProgress) {
          console.log('Upload Progress: ' + (res.loaded / res.total * 100) + '%');
        }
        if (res.type === HttpEventType.Response) {
          if (disabled) {
            this.organiser.disabled = 'Disabled';
          }
          else {
            this.organiser.disabled = 'Active';
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
      'dob': new FormControl(null, Validators.required),
      'email': new FormControl(null, Validators.required),
      'address': new FormControl(null, Validators.required),
      'disabled': new FormControl(null, Validators.required)
    });

    this.selectedFile = null;
    this.imagePreview = "";
    this.addFormData = new FormData();
  }

  AddSubmit() {
    if (/[^0-9]/.test(this.addForm.value.phonenum)) {
      this.ActiveToast(false, "Số điện thoại chỉ chứa số");
    }
    else {
      //  Create form data to add to [FromForm] of API
      this.addFormData = new FormData();
      this.addFormData.append("phonenum", this.addForm.get('phonenum')?.value);
      this.addFormData.append("password", this.addForm.get('password')?.value);
      this.addFormData.append("name", this.addForm.get('name')?.value);
      this.addFormData.append("dob", this.addForm.get('dob')?.value);
      this.addFormData.append("email", this.addForm.get('email')?.value);
      this.addFormData.append("address", this.addForm.get('address')?.value);
      if (this.selectedFile != null) {
        this.addFormData.append("certificationfile", this.selectedFile, this.selectedFile?.name);
      }
      this.addFormData.append("description", null);
      this.addFormData.append("code", null);
      this.addFormData.append("disabled", this.addForm.get('disabled')?.value);

      console.log(this.addFormData.get("certificationfile"));

      this.accountService.AddOrganiserAccount(this.addFormData).subscribe(
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

  Add(){
    let section = document.getElementById("add-form-container");
    section?.classList.add("active");
  }

  AddClose(){
    let section = document.getElementById("add-form-container");
    section?.classList.remove("active");

    this.InitAdd();
  }

  UploadImg(event: any) {
    let reader = new FileReader();
    reader.readAsDataURL(event.target.files[0]);
    reader.onload = () => {
      this.imagePreview = reader.result;
    };
    this.selectedFile = <File>event.target.files[0];
  }

  Confirmation(admin:any) {
    let section = document.getElementById("confirmation");
    section?.classList.add("active");

    this.organiser = admin;
    this.organiser.disabled == "Active"?this.organiserStatus="disabled":this.organiserStatus="avtice";
  }

  ConfirmationClose(){
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
