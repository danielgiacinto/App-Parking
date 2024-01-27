import { Component, OnInit } from '@angular/core';
import { Car } from 'src/app/Model/Car';
import { CarsService } from 'src/app/services/cars.service';

@Component({
  selector: 'app-historial',
  templateUrl: './historial.component.html',
  styleUrls: ['./historial.component.css'],
})
export class HistorialComponent implements OnInit {

  constructor(private carsService: CarsService) { }
  originalCarsHistorical: Car[] = [];
  carsHistorical: Car[] = [];
  fechaDesde: Date = new Date();
  fechaHasta: Date = new Date();

  ngOnInit() {
    this.loadCarsHistorical().subscribe(
      (data) => {
        console.log(data);
        this.originalCarsHistorical = data;
        this.carsHistorical = [...this.originalCarsHistorical];
      }
    );
  }

  loadCarsHistorical() {
    return this.carsService.getCarsHistorical();
  }

  ordenarPor(event: any) {
    const valor = event.target.value;

    if (valor == 2) {
      this.carsHistorical = [...this.originalCarsHistorical].reverse();
    } else if (valor == 1) {
      this.carsHistorical = [...this.originalCarsHistorical];
    } else {
      this.carsHistorical = [...this.originalCarsHistorical];

      if (valor == 3) {
        this.carsHistorical = this.carsHistorical.filter((car) => String(car.type) === 'Automovil');
      } else if (valor == 4) {
        this.carsHistorical = this.carsHistorical.filter((car) => String(car.type) === 'Camioneta');
      } else if (valor == 5) {
        this.carsHistorical = this.carsHistorical.filter((car) => String(car.type) === 'Motocicleta');
      }
    }
  }

  buscarPorFechas() {
    if (this.fechaDesde && this.fechaHasta) {
      // Filtra los vehículos por fechas
      this.carsHistorical = this.originalCarsHistorical.filter((car) =>
        car.admissionDate >= this.fechaDesde && car.dischargeDate <= this.fechaHasta
      );
  
      // Aplicar los filtros adicionales si ya se aplicaron antes
      this.ordenarPor(event);
    } else {
      // Si alguna fecha no está seleccionada, muestra un mensaje o realiza la lógica que desees
      console.log("Por favor, selecciona ambas fechas.");
  
      // Restaurar la lista original si no hay fechas seleccionadas
      this.carsHistorical = [...this.originalCarsHistorical];
      
      // Aplicar los filtros adicionales si ya se aplicaron antes
      this.ordenarPor(event);
    }
  }
  
}
