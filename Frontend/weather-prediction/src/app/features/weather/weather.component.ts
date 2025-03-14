import { Component } from '@angular/core';
import { WeatherApiService } from '../../core/services/weather-api.service';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-weather',
  templateUrl: './weather.component.html',
  styleUrls: ['./weather.component.scss'],
  imports: [
    CommonModule,
    MatCardModule,
    MatButtonModule,
    MatFormFieldModule,
    MatInputModule,
    MatProgressSpinnerModule,
    FormsModule
  ]
})
export class WeatherComponent {
  city: string = '';
  weatherData: any;
  loading: boolean = false;

  constructor(private weatherService: WeatherApiService) { }

  getWeather() {
    if (!this.city.trim()) {
      alert('Please enter a city name.');
      return;
    }

    this.loading = true;
    this.weatherService.getWeather(this.city).subscribe(
      (data) => {
        this.weatherData = data;
        this.loading = false;
      },
      (error) => {
        console.error('Error fetching weather data:', error);
        this.loading = false;
      }
    );
  }
}
