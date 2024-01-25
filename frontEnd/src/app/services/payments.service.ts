import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Payment } from '../Model/Payment';

@Injectable({
  providedIn: 'root'
})
export class PaymentsService {

  constructor(private http: HttpClient) { }
  urlPayments = 'http://localhost:5284/payments';

  getPayments(): Observable<Payment[]> {
    return this.http.get<Payment[]>(this.urlPayments);
  }
}
