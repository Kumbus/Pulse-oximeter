import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ResultsService {

  apiUrl = "https://localhost:7212/api/Measurements/"

  constructor(private http: HttpClient) { }

  userResults = (id: string) => {
    return this.http.get(`${this.apiUrl}user/${id}`)
  }

  deviceResults = (id: string) => {
    return this.http.get(`${this.apiUrl}device/${id}`)
  }

  addToAccount = (userId: string, deviceId: string) => {
    const body = {
      measurementId: deviceId,
      userId: userId
    }
    return this.http.put(`${this.apiUrl}`, body)
  }


}
