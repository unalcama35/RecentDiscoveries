import { Component, OnInit } from '@angular/core';
import { SpotifyService } from 'src/app/spotify.service';
import { DomSanitizer, SafeResourceUrl, SafeUrl} from '@angular/platform-browser';



@Component({
  selector: 'app-generate',
  templateUrl: './generate.component.html',
  styleUrls: ['./generate.component.css']
})
export class GenerateComponent implements OnInit {

  all: any;

  constructor(private api: SpotifyService, readonly domSanitizer: DomSanitizer) { }

  ngOnInit(): void {
    this.api.login().then(() => {
      this.getSongs();
    }).catch(error => {
      console.error('Login failed:', error);
    });
  }

  getSongs(): void{
    this.api.getRecents().subscribe(songs=>{
      this.all = songs;
      console.log(this.all)
    })

  }

}
