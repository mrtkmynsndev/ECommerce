import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { BehaviorSubject, Observable, ReplaySubject, of } from 'rxjs';
import { IUser } from 'src/app/shared/models/user';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { Router } from '@angular/router';
import { IAddress } from '../../shared/models/address';

@Injectable({
  providedIn: 'root'
})
export class AccountService {

  private baseUrl = environment.apiUrl;
  private tokenKey = environment.tokenKey;
  private currentUserSource: ReplaySubject<IUser> = new ReplaySubject<IUser>(1);
  currentUser$ = this.currentUserSource.asObservable();

  constructor(private http: HttpClient, private router: Router) { }

  login(model: any): Observable<IUser> {
    return this.http.post(this.baseUrl + 'account/login', model).pipe(
      map((user: IUser) => {
        localStorage.setItem(this.tokenKey, user.token);
        this.currentUserSource.next(user);
        return user;
      })
    );
  }

  loadCurrentUser(token: string): Observable<IUser> {
    if (token === null) {
      this.currentUserSource.next(null);
      return of(null);
    }

    let headers = new HttpHeaders();
    headers = headers.set('Authorization', `Bearer ${token}`);
    return this.http.get(this.baseUrl + 'account', { headers }).pipe(
      map((user: IUser) => {
        localStorage.setItem(this.tokenKey, user.token);
        this.currentUserSource.next(user);
        return user;
      })
    );
  }

  register(model: any): Observable<IUser> {
    return this.http.post(this.baseUrl + 'account/register', model).pipe(
      map((user: IUser) => {
        localStorage.setItem(this.tokenKey, user.token);
        this.currentUserSource.next(user);
        return user;
      })
    );
  }

  logout(): void {
    localStorage.removeItem(this.tokenKey);
    this.currentUserSource.next(null);
    this.router.navigateByUrl('/');
  }

  checkUserName(userName: string): Observable<boolean> {
    return this.http.get<boolean>(this.baseUrl + 'account/usernameexist?userName=' + userName);
  }

  checkEmailExist(email: string): Observable<boolean> {
    return this.http.get<boolean>(this.baseUrl + 'account/emailexist?email=' + email);
  }

  getAddress(): Observable<IAddress> {
    return this.http.get<IAddress>(this.baseUrl + 'account/address');
  }

  updateAddress(address: IAddress): Observable<IAddress> {
    return this.http.post<IAddress>(this.baseUrl + 'account/address', address);
  }
}
