import { Component, OnInit } from '@angular/core';
import { SpotifyService } from 'src/app/spotify.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  constructor(private api:SpotifyService) { }

  ngOnInit(): void {

  }

  username:string = "";
  password:string = "";

  onSubmit() {
    console.log('Login:', this.username);
    console.log('Password:', this.password);
    this.api.appLogin(this.username,this.password).subscribe(
      data => {
        console.log(data.message);
      }
    );
  }
}
