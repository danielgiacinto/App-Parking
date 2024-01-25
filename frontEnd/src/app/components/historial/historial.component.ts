import { Component, OnInit } from '@angular/core';
import { Car } from 'src/app/Model/Car';
import { CarsService } from 'src/app/services/cars.service';

@Component({
  selector: 'app-historial',
  templateUrl: './historial.component.html',
  styleUrls: ['./historial.component.css']
})
export class HistorialComponent implements OnInit {

  constructor(private carsService : CarsService) { }
  carsHistorical: Car[] = [];
  ngOnInit() {
    this.carsService.getCarsHistorical().subscribe(
      (data) => {
        console.log(data);
        this.carsHistorical = data;
      }
    )
  }

}
