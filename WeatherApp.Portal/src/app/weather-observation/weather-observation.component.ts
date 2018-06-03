import { Component, ViewChild, AfterViewInit, Input } from '@angular/core';
import { ObservableMedia } from '@angular/flex-layout';
import { MatGridList } from '@angular/material';
import { WeatherObservation } from '../weather-observation';
import { Subscription, observable } from 'rxjs';
import { WeatherObservationTile } from './weather-observation-tile';

@Component({
  selector: 'app-weather-observation',
  templateUrl: './weather-observation.component.html',
  styleUrls: ['./weather-observation.component.scss']
})
export class WeatherObservationComponent implements AfterViewInit {
  @Input('observation') observation: WeatherObservation;
  tiles: Array<WeatherObservationTile>;
  cityName: string;
  private observableMediaSubscription: Subscription;
  @ViewChild('gridList')
  private matGridList: MatGridList;

  constructor(private observableMedia: ObservableMedia) { }

  ngAfterViewInit(): void {
    this.observableMediaSubscription = this.observableMedia.subscribe(change => { this.updateGrid(); });
  }

  refresh(cityName: string, observation: WeatherObservation): void {
    this.cityName = cityName;
    const cloudCondition = this.getCloudsIcon(observation.cloudsCode);
    this.tiles = [
      { label: 'Location', icon: 'fa fa-map-marker', text: observation.stationName, cols: 1, rows: 1 },
      { label: 'Time', icon: 'wi wi-time-4', text: observation.observationDateTime.toLocaleTimeString(), cols: 1, rows: 1 },
      { label: 'Wind Speed', icon: 'wi wi-strong-wind', text: `${observation.windSpeed.toString()} km/h`, cols: 1, rows: 1 },
      {
        label: 'Wind Direction', icon: `wi wi-wind towards-${observation.windDirection}-deg`,
        text: `${observation.windDirection.toString()}&deg;`, cols: 1, rows: 1
      },
      { label: 'Visibility', icon: `wi ${cloudCondition}`, text: observation.visibility, cols: 1, rows: 1 },
      { label: 'Sky Conditions', icon: `wi ${cloudCondition}`, text: observation.clouds, cols: 1, rows: 1 },
      { label: 'Temperature', icon: 'wi wi-thermometer', text: `${observation.temperature.toString()}&deg;C`, cols: 1, rows: 1 },
      { label: 'Dew Point', icon: 'wi wi-celsius', text: `${observation.dewPoint.toString()}&deg;C`, cols: 1, rows: 1 },
      { label: 'Relative Humidity', icon: 'wi wi-day-fog', text: observation.humidity.toString(), cols: 1, rows: 1 },
      { label: 'Pressure', icon: 'wi wi-barometer', text: `${observation.hectoPascAltimeter.toString()} hPA`, cols: 1, rows: 1 }
    ];
  }

  private updateGrid(): void {
    if (this.observableMedia.isActive('xl')) {
      this.matGridList.cols = 5;
    } else if (this.observableMedia.isActive('lg')) {
      this.matGridList.cols = 4;
    } else if (this.observableMedia.isActive('md')) {
      this.matGridList.cols = 3;
    } else if (this.observableMedia.isActive('sm')) {
      this.matGridList.cols = 2;
    } else if (this.observableMedia.isActive('xs')) {
      this.matGridList.cols = 1;
    }
  }

  private getCloudsIcon(cloudsCode: string): string {
    const visibilityDictionary: Array<{ key: string, value: string }> = [
      { key: 'SKC', value: 'wi-day-sunny' },
      { key: 'CLR', value: 'wi-day-sunny' },
      { key: 'FEW', value: 'wi-day-cloudy' },
      { key: 'SCT', value: 'wi-day-cloudy' },
      { key: 'BKN', value: 'wi-day-cloudy' },
      { key: 'OVC', value: 'wi-cloudy' },
      { key: 'CAVOK', value: 'wi-day-sunny' },
      { key: 'NCD', value: 'wi-day-sunny' },
      { key: 'NSC', value: 'wi-day-sunny' },
      { key: 'VV', value: 'wi-day-sunny' }
    ];

    let selectedValue = '';
    visibilityDictionary.forEach(visibility => {
      if (visibility.key === cloudsCode) {
        selectedValue = visibility.value;
      }
    });

    return selectedValue !== '' ? selectedValue : 'wi-na';
  }
}
