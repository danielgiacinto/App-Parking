import { Component } from '@angular/core';
import { Router } from '@angular/router';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent {
  barraWidth: string = '5%';
  homeWidth: string = '85%';
  expand: boolean = false;

  toggleBarWidth() {
    this.barraWidth = this.barraWidth === '15%' ? '5%' : '15%';
    this.expand = !this.expand;
  }
  calcHomeWidth(): string {
    return `calc(100% - ${this.barraWidth})`;
  }

  constructor(private router: Router) {}

  cerrarSesion() {
    Swal.fire({
      title: "Quieres cerrar sesion?",
      icon: "warning",
      showCancelButton: true,
      confirmButtonColor: "#3085d6",
      cancelButtonColor: "#d33",
      confirmButtonText: "Si, cerrar sesion"
    }).then((result) => {
      if (result.isConfirmed) {
        localStorage.clear();
        this.router.navigate(['/login']);
      }
    });
  }

}
