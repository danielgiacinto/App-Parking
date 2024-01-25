import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import Price from '../Model/Price';

@Injectable({
  providedIn: 'root'
})
export class PriceService {

  constructor(private http: HttpClient) { }

  urlPrice = 'http://localhost:5284/price/1';

  getPrice(): Observable<Price> {
    return this.http.get<Price>(this.urlPrice);
  }

  updatePrice(price: Price): Observable<Price> {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json'
      })
    };
    return this.http.put<Price>(this.urlPrice, price, httpOptions);
  }
}
