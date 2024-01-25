import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import Brand from '../Model/Brand';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class BrandService {

  constructor(private http: HttpClient) { }

  urlBrand = 'http://localhost:5284/brand';

  getBrands(): Observable<Brand[]> {
    return this.http.get<Brand[]>(this.urlBrand);
  }

  getBrand(id: number): Observable<Brand> {
    return this.http.get<Brand>(this.urlBrand + '/' + id);
  }

  deleteBrand(id: number){
    return this.http.delete<number>( this.urlBrand + '/' + id);
  }

  createBrand(brand: Brand) {
    return this.http.post<Brand>(this.urlBrand, brand);
  }
}
