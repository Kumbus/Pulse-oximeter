import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class DevicesService {

  apiUrl = "https://localhost:7212/api/Devices/"

  constructor(private http: HttpClient) {}

  userDevices = (id: string) => {
    return this.http.get(`${this.apiUrl}${id}`)
  }
}
