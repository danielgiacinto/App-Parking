import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';
import { AppComponent } from './app.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { Routes, RouterModule } from '@angular/router';
import { LoginComponent } from './components/login/login.component';
import { HomeComponent } from './components/home/home.component';
import { CarsListComponent } from './components/cars-list/cars-list.component';
import { SupportComponent } from './components/support/support.component';
import { MainComponent } from './components/main/main.component';
import { ReportComponent } from './components/report/report.component';
import { HistorialComponent } from './components/historial/historial.component';

const routes :Routes = [
  {path: 'login', component: LoginComponent},
  {path: 'home', redirectTo: '/home/main', pathMatch: 'full'},
  { path: 'home', component: HomeComponent, children: [
    { path: 'cars', component: CarsListComponent },
    {path: 'cars/:patent', component: CarsListComponent},
    {path: 'support', component: SupportComponent},
    {path: 'main', component: MainComponent},
    {path: 'report', component: ReportComponent},
    {path: 'history', component: HistorialComponent}
  ]},
  { path: '', redirectTo: '/login', pathMatch: 'full' }
  
]

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    HomeComponent,
    CarsListComponent,
    SupportComponent,
    MainComponent,
    ReportComponent,
    HistorialComponent
  ],
  imports: [
    BrowserModule,
    FormsModule,
    HttpClientModule,
    ReactiveFormsModule,
    RouterModule.forRoot(routes)
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
