import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { User } from '../models/user';
import { Router  } from '@angular/router';
@Injectable({
  providedIn: 'root',
})

export class AuthService {
  isLoggedIn: boolean= false; // Remove the duplicate declaration
  private apiUrl = `${environment.apiUrl}/user`; // Replace with your API URL

  constructor(private http: HttpClient, private router:Router ) {}

  checkLoginStatus() {
    return this.http.get(`${this.apiUrl}/status`, { withCredentials: true }).subscribe( {
      next: (response) => {
        console.log('Login successful', response);
       // this.loginFailed =false;
       this.isLoggedIn = true;
          this.router.navigate(['/ads']);
      },
      error: (error) => {
        this.isLoggedIn = false;
        console.error('Login failed', error);
       // this.loginFailed =true;
      }
    });
  }
  login(user:User) {
     this.http.post<User>(`${this.apiUrl}/login`, user,{ withCredentials: true })
     .subscribe( {
      next: (response) => {
        console.log('Login successful', response);
       // this.loginFailed =false;
       this.isLoggedIn = true;
        if(response.id > 0){
          localStorage.setItem('user', JSON.stringify(response));
          this.router.navigate(['/ads']);
        }
      },
      error: (error) => {

        this.isLoggedIn = false;
        console.error('Login failed', error);
       // this.loginFailed =true;
      }
    });
  }

  register(user:User): Observable<any> {
    const headers = new HttpHeaders({
      'Authorization': 'Basic ' + btoa(`${user.userName}:${user.password}`)
    });
    return this.http.post(`${this.apiUrl}/register`, user, { withCredentials: true });
  }

  logout(): void {
    this.isLoggedIn = false;
    document.cookie = "jwt=; Path=/; Expires=Thu, 01 Jan 1970 00:00:01 GMT;";
    localStorage.removeItem('jwt'); // Clear the token from local storage
  }

  checkLoggedIn(): boolean {
    return this.isLoggedIn;
   // return !!localStorage.getItem('token'); // Check if a token exists
  }
}
