import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Available } from '../Model/Avalaible';

@Injectable({
  providedIn: 'root'
})
export class ReportService {

  constructor(private http : HttpClient) { }
  urlReports = 'http://localhost:5284/reports';

  getQuantities():Observable<Available> {
    return this.http.get<Available>(this.urlReports + '/availables')
  }

}
