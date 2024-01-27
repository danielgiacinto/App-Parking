import { Component } from '@angular/core';
import { Route, Router } from '@angular/router';
import { Available } from 'src/app/Model/Avalaible';
import Price from 'src/app/Model/Price';
import { PriceService } from 'src/app/services/price.service';
import { ReportService } from 'src/app/services/report.service';

@Component({
  selector: 'app-main',
  templateUrl: './main.component.html',
  styleUrls: ['./main.component.css']
})
export class MainComponent {

  constructor(private reportService: ReportService, private router: Router, private priceService: PriceService) { }
  available: Available = new Available();
  priceCar: Price = new Price();
  priceMotorcycle: Price = new Price();
  priceVan: Price = new Price();
  ngOnInit() {
    this.reportService.getQuantities().subscribe((data) => {
      this.available = data;
    });
    this.priceService.getPrice(1).subscribe((data) => {
      this.priceCar = data;
    })
    this.priceService.getPrice(2).subscribe((data) => {
      this.priceVan = data;
    })
    this.priceService.getPrice(3).subscribe((data) => {
      this.priceMotorcycle = data;
    })
  }

  goToCars() {
    this.router.navigate(['/home/cars']);
  }

  goToSupport() {
    this.router.navigate(['/home/support']);
  }
}
