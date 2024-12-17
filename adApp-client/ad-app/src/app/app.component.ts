import { Component, OnInit } from '@angular/core';
import { AuthService } from './services/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit{
  isLoggedIn: boolean = false;

  constructor(public authService: AuthService, private router: Router) {}
  ngOnInit(): void {
    this.authService.checkLoginStatus();
  }
  onLogout() {
    this.authService.logout();
    this.router.navigate(['/login']);
  }
}
