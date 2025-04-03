import { Component } from '@angular/core';
import { WeatherApiService } from '../../core/services/weather-api.service';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { FormsModule } from '@angular/forms';
import { WeatherResponse, Forecast } from '../../models/weather.model';


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
  objectKeys = Object.keys;
  hasSearched: boolean = false;
  city: string = '';
  loading: boolean = false;
  weatherData!: WeatherResponse;
  groupedForecast!: { [date: string]: Forecast[]; };


  constructor(private weatherService: WeatherApiService) { }

  groupForecastByDate(forecast: Forecast[] = []): { [date: string]: Forecast[] } {
    return forecast.reduce((acc, entry) => {
      const dateKey = entry.date.split('T')[0]; // Group by YYYY-MM-DD
      if (!acc[dateKey]) acc[dateKey] = [];
      acc[dateKey].push(entry);
      return acc;
    }, {} as { [date: string]: Forecast[] });
  }

  getWeather() {
    if (!this.city.trim()) {
      alert('Please enter a city name.');
      return;
    }

    this.loading = true;
    this.hasSearched = true;
    this.weatherService.getWeather(this.city).subscribe(
      (data) => {
        if (data && data.forecast?.length) {
          this.weatherData = data;
          this.loading = false;
          this.groupedForecast = this.groupForecastByDate(data.forecast);
        }
        else {
          this.weatherData = {city : this.city, forecast: [] };
          this.groupedForecast = {};
        }
        
      },
      (error) => {
        console.error('Error fetching weather data:', error);
        this.loading = false;
      }
    );
  }
}
