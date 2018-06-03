import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Country } from './country';
import { City } from './city';
import { WeatherObservation } from './weather-observation';
import { tap } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class WeatherService {
  constructor(private httpClient: HttpClient) { }

  getCountries(): Observable<Array<Country>> {
    return this.httpClient.get<Array<Country>>('http://localhost:51353/api/get-countries');
  }

  getCities(north: number, south: number, east: number, west: number): Observable<Array<City>> {
    return this.httpClient
      .get<Array<City>>(`http://localhost:51353/api/get-cities?north=${north}&south=${south}&east=${east}&west=${west}`);
  }

  getWeatherObservation(latitude: number, longitude: number): Observable<WeatherObservation> {
    return this.httpClient
      .get<WeatherObservation>(`http://localhost:51353/api/get-observation?latitude=${latitude}&longitude=${longitude}`)
      .pipe(tap(result => result.observationDateTime = new Date(Date.parse(result.datetime))));
  }
}
