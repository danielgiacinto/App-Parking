import { Component } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { User } from 'src/app/Model/User';
import { LoginService } from 'src/app/services/login.service';
import Swal from 'sweetalert2';

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

  loginUser(loginForm: NgForm) {
    if(loginForm.valid && this.isValidEmail(loginForm.value.email)) {
      this.user.email = loginForm.value.email;
      this.user.password = loginForm.value.password;
      this.loginService.loginUser(this.user).subscribe({
        next: (data) => {
          this.router.navigate(['/home/main']);
          this.login = true;
        },
        error: () => {
          Swal.fire({
            title: "Datos incorrectos !",
            text: "Por favor, ingrese nuevamente los datos",
            icon: "error",
          });
        }
      }
      );
    } else {
      Swal.fire({
        title: "Datos incorrectos",
        text: "Por favor, ingrese nuevamente los datos",
        icon: "error",
      });
    }

  }
  isValidEmail(email: string): boolean {
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    return emailRegex.test(email);
  }

}
