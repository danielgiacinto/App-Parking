import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import Swal from 'sweetalert2';
import {
  FormBuilder,
  FormControl,
  FormGroup,
  NgForm,
  Validators,
} from '@angular/forms';
import { Car } from 'src/app/Model/Car';
import Brand from 'src/app/Model/Brand';
import Price from 'src/app/Model/Price';
import { CarsService } from 'src/app/services/cars.service';
import { BrandService } from 'src/app/services/brand.service';
import { PriceService } from 'src/app/services/price.service';
import { jsPDF } from 'jspdf';
import { format } from 'date-fns';
import { CarTypeService } from 'src/app/services/carType.service';
import { Observable, forkJoin, map } from 'rxjs';
import { CarType } from 'src/app/Model/CarType';
import { Payment } from 'src/app/Model/Payment';
import { PaymentsService } from 'src/app/services/payments.service';

@Component({
  selector: 'app-cars-list',
  templateUrl: './cars-list.component.html',
  styleUrls: ['./cars-list.component.css'],
})
export class CarsListComponent {
  cars: Car[] = [];
  car: Car = new Car();
  brands: Brand[] = [];
  payments: Payment[] = [];
  price: Price = new Price();
  patent: string | null = null;
  formRegister: FormGroup = new FormGroup({});
  formExit: FormGroup = new FormGroup({});
  ubicaciones: string[][] = [
    ['A1', 'A2', 'A3', 'A4', 'A5', 'A6', 'A7', 'A8', 'A9', 'A10'],
    ['B1', 'B2', 'B3', 'B4', 'B5', 'B6', 'B7', 'B8', 'B9', 'B10'],
    ['C1', 'C2', 'C3', 'C4', 'C5', 'C6', 'C7', 'C8', 'C9', 'C10'],
  ];
  ubicacionSeleccionada: string | null = null;
  constructor(
    private carsService: CarsService,
    private route: ActivatedRoute,
    private brandService: BrandService,
    private priceService: PriceService,
    private fb: FormBuilder,
    private paymentsService: PaymentsService
  ) {
    this.formRegister = this.fb.group({
      patent: new FormControl('', [
        Validators.required,
        Validators.minLength(6),
        Validators.maxLength(7),
      ]),
      type: new FormControl('', [Validators.required]),
      brand: new FormControl('', [Validators.required]),
      admissionDate: new FormControl('', [Validators.required]),
      location: new FormControl('', [Validators.required]),

    });

    this.formExit = this.fb.group({
      dischargeDate: new FormControl('', [Validators.required]),
      format: new FormControl('', [Validators.required]),
    })
  }

  ngOnInit(): void {
      if (this.patent) {
        this.carsService.getCar(this.patent).subscribe(
          (data) => {
            console.log(data);
            this.cars = [data];
          },
          (err) => {
            console.log(err);
          }
        );
      } else {
        forkJoin({
          cars: this.loadCars(),
          brands: this.brandService.getBrands(),
          price: this.priceService.getPrice(),
          payments: this.paymentsService.getPayments(),
        }).subscribe(({ cars, brands, price, payments }) => {
          this.cars = cars;
          this.brands = brands;
          this.price = price;
          this.payments = payments;
        });
      }
  }

  loadCars(): Observable<Car[]> {
    this.carsService.getCars().subscribe((data) => {
      this.cars = data;
    });
    return this.carsService.getCars();
  }

  inicializarFechaEntrada() {
    const currentDate = new Date();
    const year = currentDate.getFullYear();
    const month = (currentDate.getMonth() + 1).toString().padStart(2, '0');
    const day = currentDate.getDate().toString().padStart(2, '0');
    const hours = currentDate.getHours().toString().padStart(2, '0');
    const minutes = currentDate.getMinutes().toString().padStart(2, '0');

    const formattedCurrentDate = `${year}-${month}-${day}T${hours}:${minutes}`;

    this.formRegister.patchValue({ admissionDate: formattedCurrentDate });
  }

  inicializarFechaSalida() {
    const currentDate = new Date();
    const year = currentDate.getFullYear();
    const month = (currentDate.getMonth() + 1).toString().padStart(2, '0');
    const day = currentDate.getDate().toString().padStart(2, '0');
    const hours = currentDate.getHours().toString().padStart(2, '0');
    const minutes = currentDate.getMinutes().toString().padStart(2, '0');

    const formattedCurrentDate = `${year}-${month}-${day}T${hours}:${minutes}`;

    this.formExit.patchValue({ dischargeDate: formattedCurrentDate });
  }

  buscarPatente(form: any) {
    if (this.patent && form.valid) {
      this.carsService.getCar(this.patent).subscribe(
        (data) => {
          console.log(data);
          this.cars = [data];
        },
        (error) => {
          console.error(error);
          form.controls['patent'].setErrors('invalidPatent');
        }
      );
    }
  }

  limpiarPatente(form: NgForm) {
    form.resetForm();
    this.patent = null;
    form.controls['patent'].setErrors(null);
    this.loadCars();
  }

  esUbicacionDisponible(ubicacion: string) {
    return this.cars.every((car) => car.location !== ubicacion);
  }

  asignarUbicacion(event: Event, ubicacion: string) {
    event.preventDefault();
    if (this.esUbicacionDisponible(ubicacion)) {
      this.formRegister.controls['location'].setValue(ubicacion);
      this.ubicacionSeleccionada = ubicacion;
    }
  }

  registrarVehiculo(form: FormGroup<any>) {
    if (this.formRegister.valid) {
      const car: Car = {
        patent: form.value.patent,
        type: form.value.type,
        brand: form.value.brand,
        garage: true,
        admissionDate: form.value.admissionDate,
        dischargeDate: form.value.dischargeDate,
        location: form.value.location,
        format: 1,
        amount: 0,
      };
      console.log(car);
      
      this.carsService.createCar(car).subscribe({
        next: (data) => {
          console.log(data);
          Swal.fire({
            title: 'Exito!',
            text: 'Se registro correctamente el vehiculo!',
            icon: 'success',
          });
          this.ngOnInit();
        },
        error: (error) => {
          console.log(error);
          Swal.fire({
            title: 'Error!',
            text: 'No se pudo registrar el vehiculo!',
            icon: 'error',
          });
        },
      });
    }
  }

  setCar(car: Car) {
    this.car = car;
    console.log(car);
  }

  calculateAmount(event: Event) {
    event.preventDefault();
    const admissionDate = new Date(this.car.admissionDate);
    const dischargeDate = new Date(this.formExit.value.dischargeDate);
    const diffInMilliseconds = dischargeDate.getTime() - admissionDate.getTime();
    const diffInHours = diffInMilliseconds / (1000 * 60 * 60);
    const amount = diffInHours * this.price.priceName;
    this.car.amount = amount;
  }

  registrarSalida() {
    if(this.formExit.valid){
      const exitData = {
        patent: this.car.patent,
        dischargeDate: this.formExit.value.dischargeDate,
        format: this.formExit.value.format,
      };
      this.carsService.registerExit(exitData).subscribe({
        next: (data) => {
          console.log(data);
          Swal.fire({
            title: 'Exito!',
            text: 'Se registro la salida con exito!',
            icon: 'success',
          });
          this.ngOnInit();
        },
        error: (error) => {
          console.log(error);
          Swal.fire({
            title: 'Error!',
            text: 'No se pudo registrar la salida!',
            icon: 'error',
          });
        },
      });
    }
  }

  formatearFecha(fecha: string): string {
    return format(new Date(fecha), 'HH:mm  dd-MM-yyyy');
  }

  generarPDF(car: Car) {
    const pdf = new jsPDF({
      orientation: 'p',
      unit: 'mm',
      format: 'a4',
    });
    pdf.setFontSize(16);
    pdf.text('Registro del ingreso del vehiculo', 60, 10);

    const tableData = [
      { Campo: 'Patente', Informacion: car.patent.toString() },
      { Campo: 'Tipo', Informacion: car.type.toString() },
      { Campo: 'Marca', Informacion: car.brand.toString() },
      {
        Campo: 'Entrada',
        Informacion: format(new Date(car.admissionDate), 'dd-MM-yyyy HH:mm'),
      },
      { Campo: 'Ubicacion', Informacion: car.location.toString() },
    ];

    const tableDataStrings = tableData.map((row) => ({
      Campo: row.Campo.toString(),
      Informacion: row.Informacion.toString(),
    }));

    pdf.table(50, 20, tableDataStrings, ['Campo', 'Informacion'], {
      headerBackgroundColor: '#5DADE2',
      fontSize: 18,
    });

    pdf.save(`${car.patent}_detalle.pdf`);
  }
}
