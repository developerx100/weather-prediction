import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, BehaviorSubject, of } from 'rxjs';
import { catchError, map, tap } from 'rxjs/operators';
import { WeatherResponse } from '../../models/weather.model';


@Injectable({
  providedIn: 'root'
})
export class WeatherApiService {
  private apiUrl = 'https://localhost:8080/api/weather/forecast';
  private cachedWeather = new BehaviorSubject<any>(null);

  constructor(private http: HttpClient) { }

  getWeather(city: string, offlineMode: boolean = false): Observable<WeatherResponse> {
    if (offlineMode && this.cachedWeather.value) {
      return of(this.cachedWeather.value);
    }

    return this.http.get(`${this.apiUrl}?city=${city}`).pipe(
      tap(data => this.cachedWeather.next(data)),
      map((response: any) => {
        console.log('API Response:', response);
        return {
          city: response.city,
          forecast: response.forecast
        };
      }),
      catchError(error => {
        console.error('Error fetching weather data', error);
        return of({ city, forecast : []});
      })
    );
  }
}
