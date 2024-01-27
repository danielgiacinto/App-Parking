import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { CarType } from '../Model/CarType';
import { Observable } from 'rxjs';


@Injectable({
  providedIn: 'root'
})
export class CarTypeService {

  constructor(private httpClient: HttpClient) { }

  urlCarType: string = 'http://localhost:5284/carTypes';

  getCarTypes(): Observable<CarType[]>{
    return this.httpClient.get<CarType[]>(this.urlCarType);
    
  }
}
