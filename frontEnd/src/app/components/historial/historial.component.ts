import { Component, OnInit } from '@angular/core';
import { format } from 'date-fns';
import { Car } from 'src/app/Model/Car';
import { Payment } from 'src/app/Model/Payment';
import { CarsService } from 'src/app/services/cars.service';
import { PaymentsService } from 'src/app/services/payments.service';

@Component({
  selector: 'app-historial',
  templateUrl: './historial.component.html',
  styleUrls: ['./historial.component.css'],
})
export class HistorialComponent implements OnInit {

  constructor(private carsService: CarsService, private paymentsService: PaymentsService) { }
  originalCarsHistorical: Car[] = [];
  carsHistorical: Car[] = [];
  payments: Payment[] = [];
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
    this.loadPayments().subscribe(
      (data) => {
        console.log(data);
        this.payments = data;
      }
    );
  }

  loadCarsHistorical() {
    return this.carsService.getCarsHistorical();
  }

  loadPayments() {
    return this.paymentsService.getPayments();
  }

  buscarPorFechas() {
    if (this.fechaDesde && this.fechaHasta) {
      this.carsHistorical = this.originalCarsHistorical.filter((car) =>
        car.admissionDate >= this.fechaDesde && car.dischargeDate <= this.fechaHasta
      );
      console.log(this.fechaDesde, this.fechaHasta);
      console.log('Lista fitrada por fechas', this.carsHistorical);
  
      const selectedFormat = document.getElementById("format") as HTMLSelectElement;
      const selectedFormatId = selectedFormat.value;
      console.log(selectedFormatId);
      
      if (selectedFormatId && selectedFormatId == "1") {
        this.carsHistorical = this.carsHistorical.filter((car) => String(car.format) == 'Efectivo');
        console.log('Lista fitrada por efectivo', this.carsHistorical);
      } else if(selectedFormatId && selectedFormatId == "2") {
        this.carsHistorical = this.carsHistorical.filter((car) => String(car.format) == 'Tarjeta de Credito');
        console.log('Lista fitrada por tarjeta credito', this.carsHistorical);
      } else if(selectedFormatId && selectedFormatId == "3") {
        this.carsHistorical = this.carsHistorical.filter((car) => String(car.format) == 'Tarjeta de Debito');
        console.log('Lista fitrada por tarjeta debito', this.carsHistorical);
      } else if (selectedFormatId && selectedFormatId == "4") {
        this.carsHistorical = this.carsHistorical.filter((car) => String(car.format) == 'Mercado Pago');
        console.log('Lista fitrada por mp', this.carsHistorical);
      } else {
        console.log('Lista sin filtrar', this.carsHistorical);
      }
  
    }
  }

  limpiar() {
    this.carsHistorical = [...this.originalCarsHistorical];
    this.fechaDesde = new Date();
    this.fechaHasta = new Date();
  }

  formatearFecha(fecha: string): string {
    return format(new Date(fecha), 'HH:mm,  dd-MM-yyyy');
  }
  
}
