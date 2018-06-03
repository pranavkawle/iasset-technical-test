import { TestBed, inject, getTestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { WeatherService } from './weather.service';
import { Country } from './country';
import { City } from './city';
import { WeatherObservation } from './weather-observation';

describe('WeatherService', () => {
  let service: WeatherService;
  let httpMock: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [WeatherService],
      imports: [HttpClientTestingModule]
    });
    service = getTestBed().get(WeatherService);
    httpMock = TestBed.get(HttpTestingController);
  });

  it('should be created', inject([WeatherService], () => {
    expect(service).toBeTruthy();
  }));

  it('should get list of countries', () => {
    const expectedCountries: Array<Country> = [
      {
        countryCode: 'AF', countryName: 'Afghanistan', isoNumeric: '004', isoAlpha3: 'AFG', fipsCode: 'AF', continent: 'AS',
        continentName: 'Asia', capital: 'Kabul', areaInSqKm: 647500.0, population: 29121286, currencyCode: 'AFN',
        languages: 'fa-AF,ps,uz-AF,tk', geonameId: 1149361, west: 60.478443, north: 38.483418, east: 74.879448, south: 29.377472,
        postalCodeFormal: null
      },
      {
        countryCode: 'AD', countryName: 'Andorra', isoNumeric: '020', isoAlpha3: 'AND', fipsCode: 'AN', continent: 'EU',
        continentName: 'Europe', capital: 'Andorra la Vella', areaInSqKm: 468.0, population: 84000, currencyCode: 'EUR', languages: 'ca',
        geonameId: 3041565, west: 1.4071867141112762, north: 42.65604389629997, east: 1.7865427778319827, south: 42.42849259876837,
        postalCodeFormal: null
      },
      {
        countryCode: 'AE', countryName: 'United Arab Emirates', isoNumeric: '784', isoAlpha3: 'ARE', fipsCode: 'AE', continent: 'AS',
        continentName: 'Asia', capital: 'Abu Dhabi', areaInSqKm: 82880.0, population: 4975593, currencyCode: 'AED',
        languages: 'ar-AE,fa,en,hi,ur', geonameId: 290557, west: 51.58332824707031, north: 26.08415985107422, east: 56.38166046142578,
        south: 22.633329391479492, postalCodeFormal: null
      }
    ];

    service.getCountries().subscribe(result => {
      expect(result).not.toBeNull();
      expect(result.length).toBe(3);
      expect(result[0].countryName).toBe('Afghanistan');
      expect(result[0].countryCode).toBe('AF');
      expect(result[0].continentName).toBe('Asia');
      expect(result[1].countryName).toBe('Andorra');
      expect(result[1].countryCode).toBe('AD');
      expect(result[1].continentName).toBe('Europe');
      expect(result[2].countryName).toBe('United Arab Emirates');
      expect(result[2].countryCode).toBe('AE');
      expect(result[2].continentName).toBe('Asia');
    });

    const req = httpMock.expectOne('http://localhost:51353/api/get-countries');
    expect(req.request.method).toEqual('GET');
    req.flush(expectedCountries);
  });

  it('should get list of cities', () => {
    const expectedCities: Array<City> = [
      {
        lng: 149.128074645996, geonameId: 2172517, countryCode: 'AU', name: 'Canberra', toponymName: 'Canberra', lat: -35.2834624726481,
        fcl: 'P', fcode: 'PPLC'
      },
      {
        lng: 144.963322877884, geonameId: 2158177, countryCode: 'AU', name: 'Melbourne', toponymName: 'Melbourne', lat: -37.8139965641595,
        fcl: 'P', fcode: 'PPLA'
      },
      {
        lng: 151.207323074341, geonameId: 2147714, countryCode: 'AU', name: 'Sydney', toponymName: 'Sydney', lat: -33.8678499639382,
        fcl: 'P', fcode: 'PPLA'
      }
    ];

    service.getCities(0, 0, 0, 0).subscribe(result => {
      expect(result).not.toBeNull();
      expect(result.length).toBe(3);
      expect(result[0].name).toBe('Canberra');
      expect(result[0].countryCode).toBe('AU');
      expect(result[0].geonameId).toBe(2172517);
      expect(result[1].name).toBe('Melbourne');
      expect(result[1].countryCode).toBe('AU');
      expect(result[1].geonameId).toBe(2158177);
      expect(result[2].name).toBe('Sydney');
      expect(result[2].countryCode).toBe('AU');
      expect(result[2].geonameId).toBe(2147714);
    });

    const req = httpMock.expectOne('http://localhost:51353/api/get-cities?north=0&south=0&east=0&west=0');
    expect(req.request.method).toEqual('GET');
    req.flush(expectedCities);
  });

  it('should get observation', () => {
    const expectedWeatherObservation: WeatherObservation = {
      elevation: 3,
      lng: 151.16666666666666,
      observation: 'YSSY 271000Z 01006KT CAVOK 18/10 Q1027 NOSIG',
      icao: 'YSSY',
      clouds: 'clouds and visibility OK',
      dewPoint: 10,
      cloudsCode: 'CAVOK',
      datetime: '2018-05-27 10:00:00',
      observationDateTime: new Date('2018-05-27 10:00:00'),
      countryCode: 'AU',
      temperature: 18,
      humidity: 59,
      stationName: 'Sydney Airport',
      weatherCondition: 'n/a',
      windDirection: 10,
      hectoPascAltimeter: 1027,
      windSpeed: 6,
      lat: -33.95
    };

    service.getWeatherObservation(0, 0).subscribe(result => {
      expect(result).not.toBeNull();
      expect(result.stationName).toBe('Sydney Airport');
      expect(result.elevation).toBe(3);
      expect(result.lng).toBe(151.16666666666666);
      expect(result.observation).toBe('YSSY 271000Z 01006KT CAVOK 18/10 Q1027 NOSIG');
      expect(result.countryCode).toBe('AU');
    });

    const req = httpMock.expectOne('http://localhost:51353/api/get-observation?lat=0&lng=0');
    expect(req.request.method).toEqual('GET');
    req.flush(expectedWeatherObservation);
  });
});
