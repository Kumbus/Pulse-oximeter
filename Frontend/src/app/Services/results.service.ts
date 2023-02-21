import { HttpClient} from '@angular/common/http';
import { SafeKeyedRead } from '@angular/compiler';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ResultsService {

  private readonly apiUrl = "https://localhost:7212/api/Measurements/"

  constructor(private http: HttpClient) { }

  userResults = (id: string) => {
    return this.http.get(`${this.apiUrl}user/${id}`)
  }

  /*pagedUserResults = (id: string, pageNumber: number, pageSize: number) => {
    return this.http.get(`${this.apiUrl}user/paged/${id}?PageNumber=${pageNumber}&PageSize=${pageSize}`, {observe: 'response'})
  }*/

  pagedUserResults = (id: string, pageNumber: number, pageSize: number, startDate?: string, endDate?: string) => {
    if(startDate && endDate)
      return this.http.get(`${this.apiUrl}user/paged/${id}?MinDate=${startDate}&MaxDate=${endDate}&PageNumber=${pageNumber}&PageSize=${pageSize}`, {observe: 'response'})
    else if (startDate)
      return this.http.get(`${this.apiUrl}user/paged/${id}?MinDate=${startDate}&PageNumber=${pageNumber}&PageSize=${pageSize}`, {observe: 'response'})

    return this.http.get(`${this.apiUrl}user/paged/${id}?PageNumber=${pageNumber}&PageSize=${pageSize}`, {observe: 'response'})
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
