import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import {
  MatToolbarModule, MAT_LABEL_GLOBAL_OPTIONS, MatSelectModule,
  MatProgressBarModule, MatButtonModule, MatCardModule, MatProgressSpinnerModule, MatGridListModule
} from '@angular/material';
import { WeatherFormComponent } from './weather-form/weather-form.component';
import { WeatherService } from './weather.service';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { WeatherObservationComponent } from './weather-observation/weather-observation.component';
import { ObservableMediaProvider } from '@angular/flex-layout';

@NgModule({
  declarations: [
    AppComponent,
    WeatherFormComponent,
    WeatherObservationComponent
  ],
  imports: [
    CommonModule,
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule,
    MatToolbarModule,
    MatSelectModule,
    MatProgressSpinnerModule,
    MatButtonModule,
    MatCardModule,
    MatGridListModule
  ],
  providers: [
    WeatherService,
    ObservableMediaProvider
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
