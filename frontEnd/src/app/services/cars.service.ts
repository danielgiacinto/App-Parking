import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Car } from '../Model/Car';

@Injectable({
  providedIn: 'root'
})
export class CarsService {

  constructor(private http: HttpClient) { }

  urlCars: string = 'http://localhost:5284/cars';
  urlExit: string = 'http://localhost:5284/exit';

  car:Car = new Car();

  getCarsHistorical() :Observable<Car[]> {
    return this.http.get<Car[]>(this.urlCars);
  }
  getCars() :Observable<Car[]> {
    return this.http.get<Car[]>(this.urlCars + '/garage');
  }

  getCar(patent: string) :Observable<Car> {
    return this.http.get<Car>(this.urlCars + '/garage/' + patent);
  }

  createCar(car: Car): Observable<Car> {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json'
      })
    };
    return this.http.post<Car>(this.urlCars, car, httpOptions);
  }

  registerExit(data: {patent:string, dischargeDate: Date}): Observable<Car> {
    return this.http.put<Car>(this.urlExit, data);
  }
}
