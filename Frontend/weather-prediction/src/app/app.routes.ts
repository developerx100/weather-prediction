import { Routes } from '@angular/router';
import { WeatherComponent } from './features/weather/weather.component';

export const routes: Routes = [
  //{ path: '', redirectTo: 'weather', pathMatch: 'full' },  // Redirect default path
  { path: '', component: WeatherComponent },  // Weather page
  //{ path: '**', redirectTo: 'weather' }
];
