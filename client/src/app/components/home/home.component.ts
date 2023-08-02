import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { SpotifyService } from 'src/app/spotify.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {

  constructor(private router: Router, private spotifyService: SpotifyService) { }

  ngOnInit(): void {
  }



  redirectToGenerate() {
    this.router.navigate(['/generate']);
  }

  redirectToTopSongs() {
    this.router.navigate(['/topsongs'])
  }

  sendEmail() {
    console.log(this.spotifyService.emailSongs().subscribe());
  }
}
