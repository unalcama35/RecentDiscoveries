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

  login(): Promise<void> {
    this.http
    .get<any[]>(this.api_url+'/Login')
    .pipe(map(data => data));
    return Promise.resolve();
    
  }
}
