import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { User } from '../Model/User';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class LoginService {
  constructor(private http: HttpClient) {}

  urlLogin: string = 'http://localhost:5284/login';
  user: User = new User();

  loginUser(user: User): Observable<User> {
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    return this.http.post<User>(this.urlLogin, user, {headers});
  }

  updatePassword(any: any): Observable<true> {
    return this.http.put<true>(this.urlLogin + '/updatePassword', any);
  }
}
