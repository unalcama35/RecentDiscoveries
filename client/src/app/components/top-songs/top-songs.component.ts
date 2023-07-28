import { Component, OnInit } from '@angular/core';
import { SpotifyService } from 'src/app/spotify.service';
import { DomSanitizer, SafeResourceUrl, SafeUrl} from '@angular/platform-browser';


@Component({
  selector: 'app-top-songs',
  templateUrl: './top-songs.component.html',
  styleUrls: ['./top-songs.component.css']
})
export class TopSongsComponent implements OnInit {

  all: any;

  constructor(private api: SpotifyService, readonly domSanitizer: DomSanitizer) { }

  ngOnInit(): void {
    this.getSongs();
  }

  getSongs(): void{
    this.api.getTopTracks().subscribe(songs=>{
      this.all = songs;
      console.log(this.all)
    })

  }
}
