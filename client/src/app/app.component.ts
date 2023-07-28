import { Component } from '@angular/core';
import { NavbarService } from './navbar.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'client';
  navServ:NavbarService;
  constructor(private navbarService: NavbarService) {
    // Use the service to get the text to be displayed in the navbar
    this.navServ = this.navbarService;
  }
  
  
}

