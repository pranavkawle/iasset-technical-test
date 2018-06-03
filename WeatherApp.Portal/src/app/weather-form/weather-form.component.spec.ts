import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { WeatherFormComponent } from './weather-form.component';
import { MatCardModule, MatButtonModule, MatProgressSpinnerModule, MatSelectModule, MatGridListModule } from '@angular/material';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { WeatherObservationComponent } from '../weather-observation/weather-observation.component';
import { HttpClientModule } from '@angular/common/http';
import { ObservableMediaProvider } from '@angular/flex-layout';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

describe('WeatherFormComponent', () => {
  let component: WeatherFormComponent;
  let fixture: ComponentFixture<WeatherFormComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [
        BrowserAnimationsModule,
        FormsModule,
        ReactiveFormsModule,
        HttpClientModule,
        MatSelectModule,
        MatProgressSpinnerModule,
        MatButtonModule,
        MatCardModule,
        MatGridListModule
      ],
      declarations: [
        WeatherFormComponent,
        WeatherObservationComponent
      ],
      providers: [ObservableMediaProvider]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(WeatherFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
