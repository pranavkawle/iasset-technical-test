export interface WeatherObservation {
    observation: string;
    datetime: string;
    observationDateTime: Date;
    stationName: string;
    icao: string;
    countryCode: string;
    elevation: number;
    lat: number;
    lng: number;
    temperature: number;
    dewPoint: number;
    humidity: number;
    clouds: string;
    cloudsCode: string;
    weatherCondition: string;
    hectoPascAltimeter: number;
    windDirection: number;
    windSpeed: number;
    visibility: string;
}