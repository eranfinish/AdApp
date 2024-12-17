import { Component, OnInit } from '@angular/core';
import { AdsService } from '../../services/ads.service';
import { Ad } from '../../models/ad';
import { Router  } from '@angular/router';
@Component({
  selector: 'app-ads',
  templateUrl: './ads.component.html',
  styleUrls: ['./ads.component.css']
})
export class AdsComponent implements OnInit {
  selectedImage: File | null = null;
  ads: Ad[] = []; // Holds the list of ads
  filteredAds: Ad[] = []; // Holds the list  of filtered ads
  searchQuery: string = '';
  isNewAd: boolean = false;
 currentUserId: number=0;

  newAd: Ad = {
    id: 0,
    title: '',
    description: '',
    category: '',
    createdAt: '',
    ownerId: 0,
    imageBase64: '',
    name:'',
    phone:'',
    address:'',
   // isAdmin: false,
    //isRegistering: false,
    price: 0,
    currency:0,
  };



  constructor(private adsService: AdsService,
    private router: Router) {}

editAd(ad: any): void {
    this.router.navigate(['/edit-ad', ad.id]);
  }

  ngOnInit(): void {
    const user = JSON.parse(localStorage.getItem('user') || '{}');
    if (user && user.id) {
      this.currentUserId = parseInt(user.id);
    }
    this.loadAds();
  }

  // Fetch ads from the server
  loadAds(): void {
    this.adsService.getAllAds().subscribe({
      next: (response) => {
        this.ads = response;
         this.filteredAds = response;
        console.log('Ads loaded successfully:', response);
      },
      error: (error) => {
        console.log('Error:', error);
        this.router.navigate(['/login']);
        if (error.status === 401) {
          console.error('Unauthorized. Please log in again.');

        }
        else {
          console.error('Failed to load ads:', error);
        }
      }
    });
  }
  onSearch() {
    if (!this.searchQuery) {
      this.filteredAds = this.ads;
    } else {
      const query = this.searchQuery.toLowerCase();
      this.filteredAds = this.ads.filter(ad =>
        ad.title.toLowerCase().includes(query) ||
        ad.description.toLowerCase().includes(query) ||
        ad.category.toLowerCase().includes(query)
      );
    }
  }

  // Add a new ad
  addAd(): void {
    this.adsService.createAd(this.newAd).subscribe({
      next: (response) => {
        console.log('Ad added successfully:', response);
        this.ads.push(response); // Update the list locally
      // this.newAd = { title: '', description: '', category: '' }; // Clear form
      },
      error: (error) => {

        if (error.status === 401) {
          console.error('Unauthorized. Please log in again.');
          this.router.navigate(['/login']);
        }
        else{
          console.error('Failed to add ad:', error);
        }
        this.router.navigate(['/login']);
      }
    });
  }

  toggleNewAd() {
    this.isNewAd = !this.isNewAd;
  }
   onImageSelected(event: any) {
    const file: File = event.target.files[0];
    if (file) {
      this.selectedImage = file;
    }
  }
}
