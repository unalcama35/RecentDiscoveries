import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from  '@angular/common/http';
import { map } from 'rxjs';
import { CookieService } from 'ngx-cookie-service';

@Injectable({
  providedIn: 'root'
})
export class SpotifyService {

  api_url = 'https://localhost:7214/api/Song'; //url up to api/Song


  constructor(private http:HttpClient, private cookieService: CookieService) { }



  getRecents(){
  //  console.log(this.cookieService.get('authToken'));
    const params = new HttpParams().set('userToken', this.cookieService.get('authToken'));

    return this.http
    .get<any[]>(this.api_url+'/RecentLiked', {params})
    .pipe(map(data => data));
  }

  getTopTracks(){
    const params = new HttpParams().set('userToken', this.cookieService.get('authToken'));

    return this.http
    .get<any[]>(this.api_url+'/TopTracks', {params})
    .pipe(map(data => data));
  }

  login() { //deprecated
  //  return Promise.resolve(); // don't use login for now 07/23/2023
    const headers = new HttpHeaders().set('Content-Type', 'application/json');

    return this.http.post<any>(`${this.api_url}/Login`, {}, { headers: headers });
  }

  appLogin(username:string, password:string){
    const headers = new HttpHeaders().set('Content-Type', 'application/json');

    return this.http.post<any>(`${this.api_url}/AppLogin`, {"loginName": username,"password": password}, { headers: headers })
  }
  appRegister(username:string, password:string, email:string){
    const headers = new HttpHeaders().set('Content-Type', 'application/json');

    const currentDate = new Date().toISOString();

    const requestBody = {
      username: username,
      password: password,
      email: email,
      lastLogin: currentDate,
      dateCreated: currentDate
    };

    return this.http.post<any>(`${this.api_url}/AppRegister`, requestBody, { headers: headers })
  }


}
