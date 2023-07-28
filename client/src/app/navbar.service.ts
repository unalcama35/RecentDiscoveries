import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class NavbarService {

  constructor() { }

  private navText: string = 'Login';
  private isLogged:boolean = false;
  private profilePic:string = "";

  setNavText(text: string): void {
    this.navText = text;
  }

  getNavText(): string {
    return this.navText;
  }

  setLogged(value:boolean){
    this.isLogged=value;
  }
  getLogged(){
    return this.isLogged;
  }
  getProfilePic(){
    return this.profilePic;
  }
  setProfilePic(text:string){
    this.profilePic= text;
  }
}
