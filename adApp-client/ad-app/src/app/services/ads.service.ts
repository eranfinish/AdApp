import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Ad } from '../models/ad';
import { environment } from 'src/environments/environment';
@Injectable({
  providedIn: 'root',
})
export class AdsService {
  private baseUrl: string = `${environment.apiUrl}/ads`; // API base URL

  constructor(private http: HttpClient) {}

  // Fetch all ads
  getAllAds(): Observable<Ad[]> {
    return this.http.get<Ad[]>(`${this.baseUrl}`,{ withCredentials: true } );
  }

  // Fetch a specific ad by ID
  getAdById(id: number): Observable<Ad> {
    return this.http.get<Ad>(`${this.baseUrl}/${id}`,{ withCredentials: true } );
  }

  // Create a new ad
  createAd(ad: Ad): Observable<Ad> {
    return this.http.post<Ad>(`${this.baseUrl}`, ad,{ withCredentials: true } );
  }

  // Update an existing ad
  updateAd(id: number, ad: Ad): Observable<Ad> {
    return this.http.put<Ad>(`${this.baseUrl}`, ad,
      { withCredentials: true } );
  }

  // Delete an ad by ID
  deleteAd(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }
}
