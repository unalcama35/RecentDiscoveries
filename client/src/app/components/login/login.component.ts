import { Component, OnInit } from '@angular/core';
import { CookieService } from 'ngx-cookie-service';
import { NavbarService } from 'src/app/navbar.service';
import { SpotifyService } from 'src/app/spotify.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  constructor(private api:SpotifyService, private cookieService: CookieService, private navbarService: NavbarService) { 
    this.navServ = navbarService;
  }

  ngOnInit(): void {

  }

  userName:string = "";
  userPassword:string = "";
  email:string = "";
  buttonText:string = "Log in";
  register:boolean = false;
  loggedin:boolean = false;
  navServ:NavbarService;

  onSubmit() {
    if(!this.navbarService.getLogged()){
      
      if(!this.register){
          this.api.appLogin(this.userName,this.userPassword).subscribe(
            data => {
              var msg = data.message;
              if(msg == "Account not found."){
                throw new Error(msg);          
              }else{
                this.navbarService.setProfilePic(data.profilepic);
                console.log("Token retrieved");
                this.cookieService.set('authToken', msg);
                console.log(this.api.login().subscribe());
                this.navbarService.setLogged(true);
                this.navbarService.setNavText(this.userName);
                
              }
            }
          );
      } else {
          this.api.appRegister(this.userName, this.userPassword, this.email).subscribe(
            data => {
              console.log(data);
              this.register=false;
              this.buttonText = "Log in";
            }
          )
      }
    } else{

    }
  }


  
  toggleRegText(){
    this.register = !this.register 
    if(!this.register){
      this.buttonText = "Log in";
    }else{
      this.buttonText = "Register";
    }
  }
}
