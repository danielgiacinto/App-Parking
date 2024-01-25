import { Component } from '@angular/core';
import { Observable } from 'rxjs';
import Brand from 'src/app/Model/Brand';
import Price from 'src/app/Model/Price';
import { BrandService } from 'src/app/services/brand.service';
import { LoginService } from 'src/app/services/login.service';
import { PriceService } from 'src/app/services/price.service';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-support',
  templateUrl: './support.component.html',
  styleUrls: ['./support.component.css']
})
export class SupportComponent {


  price: Price = new Price();
  brands: Brand[] = [];
  brand: Brand = new Brand();
  constructor(private priceService: PriceService, private brandService: BrandService, private loginService: LoginService) {}

  updatePrice(price : Price) {
    this.price = price;
    console.log(this.price);
    this.priceService.updatePrice(price).subscribe({
      next: (data) => {
        console.log(data);
        alert("Actualizado con exito");
      },
      error: (err) => {
        console.log(err);
      },
    });
  }
  getPrice() {
    this.priceService.getPrice().subscribe((data) => {
      console.log(data);
      this.price = data;
    });
  }

  getBrands() {
    this.brandService.getBrands().subscribe((data) => {
      console.log(data);
      this.brands = data;
    });
  }

  deleteBrand(id: number) {
    Swal.fire({
      title: "Quieres eliminar la marca?",
      icon: "warning",
      showCancelButton: true,
      confirmButtonColor: "#3085d6",
      cancelButtonColor: "#d33",
      confirmButtonText: "Si, borrar."
    }).then((result) => {
      if (result.isConfirmed) {
        this.brandService.deleteBrand(id).subscribe({
          next: (data) => {
            console.log(data);
            Swal.fire({
              title: "Exito!",
              text: "Se elimino correctamente la marca",
              icon: "success",
            });
          },
          error: (error) => {
            console.log(error);
            Swal.fire({
              title: "Error!",
              text: "No se pudo borrar la marca",
              icon: "error",
            })
          },
        })
      }
    });
  }
  createBrand(brand: Brand){
    this.brand = brand;
    this.brandService.createBrand(brand).subscribe({
      next: (data) => {
        console.log(data);
        Swal.fire({
          position: "top-end",
          icon: "success",
          title: "Se guardo correctamente!",
          showConfirmButton: false,
          timer: 1500
        });
        this.getBrands();
      },
      error: (err) => {
        console.log(err);
        alert("No se pudo crear");
      },
    })
  }

  updatePassword(actualPassword: string, newPassword: string) {
    const data = {
      actualPassword: actualPassword,
      newPassword: newPassword
    }
    this.loginService.updatePassword(data).subscribe({
      next: (data) => {
        console.log(data);
        alert("Contraseña actualizada");
      },
      error: (err) => {
        console.log(err);
        alert("Error, contraseña no actualizada");
      },
    })
  }
}
