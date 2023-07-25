import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HomeComponent } from './components/home/home.component';
import { GenerateComponent } from './components/generate/generate.component';
import { HttpClientModule } from '@angular/common/http';
import { TopSongsComponent } from './components/top-songs/top-songs.component';

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    GenerateComponent,
    TopSongsComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
