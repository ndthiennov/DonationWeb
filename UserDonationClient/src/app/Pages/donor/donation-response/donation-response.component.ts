import { NgIf } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router, RouterLink, RouterOutlet } from '@angular/router';
import { SharedService } from '../../../Shared/shared.service';

@Component({
  selector: 'app-donation-response',
  standalone: true,
  imports: [RouterOutlet, RouterLink, NgIf],
  templateUrl: './donation-response.component.html',
  styleUrl: './donation-response.component.css'
})
export class DonationResponseComponent implements OnInit {

  constructor(private router: Router,
    private route: ActivatedRoute
  ) {

  }

  status: boolean = true;
  ngOnInit(): void {
    let statusResponse = this.route.snapshot.paramMap.get('paymentresult');
    if (statusResponse != "200") {
      this.status = false;
    }
  }

}