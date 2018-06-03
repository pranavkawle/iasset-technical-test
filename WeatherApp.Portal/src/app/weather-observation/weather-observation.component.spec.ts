import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { MatCardModule, MatGridListModule } from '@angular/material';

import { WeatherObservationComponent } from './weather-observation.component';
import { ObservableMediaProvider } from '@angular/flex-layout';

describe('WeatherObservationComponent', () => {
  let component: WeatherObservationComponent;
  let fixture: ComponentFixture<WeatherObservationComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [
        MatCardModule,
        MatGridListModule
      ],
      declarations: [WeatherObservationComponent],
      providers: [ObservableMediaProvider]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(WeatherObservationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
