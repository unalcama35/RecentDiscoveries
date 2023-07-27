import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from  '@angular/common/http';
import { map } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class SpotifyService {

  api_url = 'https://localhost:7214/api/Song';


  constructor(private http:HttpClient) { }



  getRecents(){
    return this.http
    .get<any[]>(this.api_url+'/RecentLiked')
    .pipe(map(data => data));
  }

  getTopTracks(){
    return this.http
    .get<any[]>(this.api_url+'/TopTracks')
    .pip
  }

  login(): Promise<void> {
    return Promise.resolve(); // don't use login for now 07/23/2023
    const headers = new HttpHeaders().set('Content-Type', 'application/json');

    return this.http.post<any>(`${this.api_url}/Login`, {}, { headers: headers })
      .toPromise() 
      .then(data => {
        console.log('Logged In');
      })
      .catch(error => {
        console.error('Error occurred while logging in:', error);
      });
  }

  appLogin(username:string, password:string){
    const headers = new HttpHeaders().set('Content-Type', 'application/json');

    return this.http.post<any>(`${this.api_url}/AppLogin`, {"loginName": username,"password": password}, { headers: headers })
  }
}
