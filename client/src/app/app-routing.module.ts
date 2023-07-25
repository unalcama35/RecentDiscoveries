import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './components/home/home.component';
import { GenerateComponent } from './components/generate/generate.component';
import { TopSongsComponent } from './components/top-songs/top-songs.component';
import { LoginComponent } from './components/login/login.component';

const routes: Routes = [
  {path:'home', component: HomeComponent},
  {path:'generate', component: GenerateComponent},
  {path:'topsongs', component: TopSongsComponent},
  {path:'login', component: LoginComponent},


  {path:'**', redirectTo: 'home', pathMatch: 'full' }



];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
