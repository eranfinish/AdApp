import { Component } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import {User} from '../../models/user';
import { Router  } from '@angular/router';
@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  isLogin = true; // Toggle between Login and Register
  username: string|null = null;
  password: string|null = null;
  email: string|null = null;
  loginFailed = false;
//router:Router = new Router();
  loginData = {
    email: '',
    password: ''
  };

  registerData = {
    username: '',
    email: '',
    password: ''
  };
user: User = new User(-1, '', '', '', '', false, false,'','');
  constructor(private authService: AuthService, private router: Router) {}

  switchToLogin() {
    this.isLogin = true;
  }

  switchToSignUp() {
    this.isLogin = false;
  }

  onLogin() {
    this.user.isRegistering = false;
        this.authService.login(this.user);
    //     .subscribe({
    //   next: (response) => {
    //     console.log('Login successful', response);
    //     this.loginFailed =false;

    //     if(response.id > 0){
    //       localStorage.setItem('user', JSON.stringify(response));
    //       this.router.navigate(['/ads']);
    //     }
    //   },
    //   error: (error) => {
    //     console.error('Login failed', error);
    //     this.loginFailed =true;
    //   }
    // });
  }

  onRegister() {
    console.log("username: " + this.username);
    this.user.isRegistering = true;
    this.authService.register(this.user).subscribe({
      next: (response) => {
        console.log('Registration successful', response);

       this.user = response;
       if(response.id > 0){
        localStorage.setItem('user', JSON.stringify(response));
        this.router.navigate(['/ads']);
      }},
      error: (error) => {
        console.error('Registration failed', error);
      }
    });
  }

  forgotPassword(){}
}
