import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AdsService } from '../../../services/ads.service';
@Component({
  selector: 'app-edit-ad',
  templateUrl: './edit-ad.component.html',
  styleUrls: ['./edit-ad.component.css']
})

export class EditAdComponent {
  ad: any = {};

  constructor(
    private adsService: AdsService,
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    const adId = this.route.snapshot.paramMap.get('id');
    if (adId) {
      this.adsService.getAdById(+adId).subscribe({
        next: (response) => {
          this.ad = response;
        },
        error: (error) => {
          console.error('Failed to load ad:', error);
        }
      });
    }
  }

  updateAd(): void {
    this.adsService.updateAd(this.ad.id, this.ad).subscribe({
      next: (response) => {
        console.log('Ad updated successfully:', response);
        // Optionally, navigate back to ads list
      },
      error: (error) => {
        console.error('Failed to update ad:', error);
      }
    });
  }
}
