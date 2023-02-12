import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { JwtHelperService } from '@auth0/angular-jwt';
import { Result } from 'src/app/Interfaces/result';
import { ResultsService } from 'src/app/Services/results.service';

@Component({
  selector: 'app-results',
  templateUrl: './results.component.html',
  styleUrls: ['./results.component.scss']
})
export class ResultsComponent implements OnInit{

  constructor(private _resultsService: ResultsService, private jwtHelper: JwtHelperService) {}
  displayedColumns: string[] = ['date', 'averageHeartRate', 'maximumHeartRate', 'minimumHeartRate', 'averageSpO2', 'maximumSpO2', 'minimumSpO2'];
  dataSource: MatTableDataSource<Result> = new MatTableDataSource<Result>();

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  ngOnInit(): void {
    const token = localStorage.getItem("token");
    const decodedToken = this.jwtHelper.decodeToken(token!);
    console.log(decodedToken.Id)

    this._resultsService.userResults(decodedToken.Id).subscribe((data: any) => {
      this.dataSource.data = data
      this.dataSource.paginator = this.paginator;
      this.dataSource.sort = this.sort;
    })
  }

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();

    if (this.dataSource.paginator) {
      this.dataSource.paginator.firstPage();
    }
  }

}
