import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, BehaviorSubject, of } from 'rxjs';
import { catchError, map, tap } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class WeatherApiService {
  private apiUrl = 'https://localhost:8080/api/weather/forecast';
  private cachedWeather = new BehaviorSubject<any>(null);

  constructor(private http: HttpClient) { }

  getWeather(city: string, offlineMode: boolean = false): Observable<any> {
    if (offlineMode && this.cachedWeather.value) {
      return of(this.cachedWeather.value);
    }

    return this.http.get(`${this.apiUrl}?city=${city}`).pipe(
      tap(data => this.cachedWeather.next(data)),
      map((response: any) => {
        console.log('API Response:', response);
        return {
          city: city,
          temp: response.forecast[0]?.highTemp ?? 'N/A',
          condition: response.forecast[0]?.advice ?? 'No advisory available'
        };
      }),
      catchError(error => {
        console.error('Error fetching weather data', error);
        return of({ city, temp: 'N/A', condition: 'Failed to load weather data. Try again later.' });
      })
    );
  }
}
