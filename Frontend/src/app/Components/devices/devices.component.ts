import { Component, OnInit, ViewChild } from '@angular/core';
import {animate, state, style, transition, trigger} from '@angular/animations';
import { MatTableDataSource } from '@angular/material/table';
import { JwtHelperService } from '@auth0/angular-jwt';
import { Result } from 'src/app/Interfaces/result';
import { ResultsService } from 'src/app/Services/results.service';
import { DevicesService } from 'src/app/Services/devices.service';

@Component({
  selector: 'app-devices',
  templateUrl: './devices.component.html',
  styleUrls: ['./devices.component.scss'],
  animations: [
    trigger('detailExpand', [
      state('collapsed', style({height: '0px', minHeight: '0'})),
      state('expanded', style({height: '*'})),
      transition('expanded <=> collapsed', animate('225ms cubic-bezier(0.4, 0.0, 0.2, 1)')),
    ]),
  ]
})
export class DevicesComponent implements OnInit{

  constructor(private _resultsService: ResultsService, private jwtHelper: JwtHelperService, private _devicesService: DevicesService) {}

  resultsDisplayedColumns: string[] = ['date', 'averageHeartRate', 'maximumHeartRate', 'minimumHeartRate', 'averageSpO2', 'maximumSpO2', 'minimumSpO2', 'add'];
  columnsToDisplay = ['device'];
  columnsToDisplayWithExpand = [...this.columnsToDisplay, 'expand'];
  dataSources: {[key: string]: MatTableDataSource<Result>} = {}
  devicesDataSource: MatTableDataSource<any> = new MatTableDataSource()
  expandedElement: Result | null | undefined

  ngOnInit(): void {
    const token = localStorage.getItem("token");
    const decodedToken = this.jwtHelper.decodeToken(token!);


    this._devicesService.userDevices(decodedToken.Id).subscribe((data: any) => {
      this.devicesDataSource.data = data
      data.forEach((element:any) => {
        this._resultsService.deviceResults(element.id).subscribe((data: any) => {
          this.dataSources[element.id] = new MatTableDataSource(data)
          console.log(this.dataSources)
        })
      });
    })


  }

  addToAccount = (id: string) => {
    const token = localStorage.getItem("token");
    const decodedToken = this.jwtHelper.decodeToken(token!);

    this._resultsService.addToAccount(decodedToken.Id, id).subscribe()
  }

}
