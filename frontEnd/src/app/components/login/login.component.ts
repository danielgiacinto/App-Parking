import { Component } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { User } from 'src/app/Model/User';
import { LoginService } from 'src/app/services/login.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {

  email: string = '';
  password: string = '';
  login: boolean = false;
  user:User = new User();

  constructor(private loginService: LoginService, private router : Router) { }

  loginUser(login: NgForm) {
    this.user.email = login.value.email;
    this.user.password = login.value.password;
    this.loginService.loginUser(this.user).subscribe({
      next: (data) => {
        console.log(data);
        this.router.navigate(['/home/main']);
        this.login = true;
      },
      error: (err) => {
        console.log(err);
        alert("Login fallido");
      }
    }
    );
  }


}
