import { Component, OnInit, OnDestroy, ViewChild } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';
import { WeatherService } from '../weather.service';
import { Country } from '../country';
import { City } from '../city';
import { Subscription } from 'rxjs';
import { takeUntil, finalize } from 'rxjs/operators';
import { WeatherObservation } from '../weather-observation';
import { WeatherObservationComponent } from '../weather-observation/weather-observation.component';

@Component({
  selector: 'app-weather-form',
  templateUrl: './weather-form.component.html',
  styleUrls: ['./weather-form.component.scss']
})
export class WeatherFormComponent implements OnInit, OnDestroy {
  countryControl = new FormControl('', [Validators.required]);
  cityControl = new FormControl({ value: '', disabled: true }, [Validators.required]);
  countries: Array<Country>;
  cities: Array<City>;
  showSpinner: boolean;
  showServerError: boolean;
  showObservationError: boolean;
  showObservation: boolean;
  @ViewChild('weatherObservation')
  private weatherObservationComponent: WeatherObservationComponent;
  private countryServiceSubscription: Subscription;
  private cityServiceSubscription: Subscription;
  private observationServiceSubscription: Subscription;

  constructor(private weatherService: WeatherService) {
    this.showSpinner = false;
    this.showObservation = false;
  }

  ngOnInit() {
    this.showSpinner = true;
    this.weatherService.getCountries()
      .pipe(finalize(() => { this.showSpinner = false; }))
      .subscribe(result => {
        this.manageErrors();
        this.countries = result;
      },
        errorResponse => {
          this.manageErrors(true);
        }
      );
  }

  ngOnDestroy(): void {
    this.countryServiceSubscription.unsubscribe();
    this.cityServiceSubscription.unsubscribe();
    this.observationServiceSubscription.unsubscribe();
  }

  onCountryChange(): void {
    if (this.countryControl.valid) {
      this.showSpinner = true;
      this.cities = [];
      const selectedCountry = this.countryControl.value as Country;
      this.weatherService.getCities(selectedCountry.north, selectedCountry.south, selectedCountry.east, selectedCountry.west)
        .pipe(finalize(() => { this.showSpinner = false; }))
        .subscribe(result => {
          this.manageErrors();
          this.cities = result;
          this.showSpinner = false;
        },
          errorResponse => {
            this.manageErrors(true);
          }
        );
      this.cityControl.enable();
    }
  }

  onGoClick(): void {
    if (this.cityControl.valid) {
      this.showSpinner = true;
      this.showObservation = true;
      const selectedCity = this.cityControl.value as City;
      this.weatherService.getWeatherObservation(selectedCity.lat, selectedCity.lng)
        .pipe(finalize(() => { this.showSpinner = false; }))
        .subscribe(result => {
          this.manageErrors();
          this.weatherObservationComponent.refresh(selectedCity.name, result);
        },
          errorResponse => {
            this.manageErrors(false, true);
          }
        );
    }
  }

  private manageErrors(showServerError: boolean = false, showObservationError: boolean = false): void {
    this.showServerError = showServerError;
    this.showObservationError = showObservationError;
  }
}
