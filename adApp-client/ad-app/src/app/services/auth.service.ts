import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { User } from '../models/user';
@Injectable({
  providedIn: 'root',
})

export class AuthService {
  private apiUrl = `${environment.apiUrl}/user`; // Replace with your API URL

  constructor(private http: HttpClient, ) {}

  login(user:User): Observable<any> {
    return this.http.post(`${this.apiUrl}/login`, user,{ withCredentials: true });
  }

  register(user:User): Observable<any> {
    const headers = new HttpHeaders({
      'Authorization': 'Basic ' + btoa(`${user.userName}:${user.password}`)
    });
    return this.http.post(`${this.apiUrl}/register`, user, { withCredentials: true });
  }

  logout(): void {
    localStorage.removeItem('token'); // Clear the token from local storage
  }

  isLoggedIn(): boolean {
    return !!localStorage.getItem('token'); // Check if a token exists
  }
}
