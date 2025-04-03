export interface Forecast {
  date: string; // ISO string
  highTemp: number;
  lowTemp: number;
  advice: string;
}

export interface WeatherResponse {
  city: string;
  forecast: Forecast[];
}
