import { HttpResponse } from '@angular/common/http';
import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
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

  pageSize = 10;
  pageIndex = 0;
  pageSizeOptions = [10, 25, 50];

  ngOnInit(): void {
    const token = localStorage.getItem("token");
    const decodedToken = this.jwtHelper.decodeToken(token!);

    this._resultsService.pagedUserResults(decodedToken.Id, 1, 10).subscribe((response: HttpResponse<any>) => {

      const paginationParameters = JSON.parse(response.headers.get('x-pagination')!)
      setTimeout(() => {
        this.paginator.length = paginationParameters.TotalItemCount
      })


      this.dataSource.data = response.body
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

  onPageChange = (event: PageEvent) => {
   const token = localStorage.getItem("token");
    const decodedToken = this.jwtHelper.decodeToken(token!);
    console.log(event)


    this._resultsService.pagedUserResults(decodedToken.Id, event.pageIndex + 1, event.pageSize).subscribe((response: HttpResponse<any>) => {
      const paginationParameters = JSON.parse(response.headers.get('x-pagination')!)
      setTimeout(() => {
        this.paginator.length = paginationParameters.TotalItemCount
        this.pageSize = event.pageSize;
        this.pageIndex = event.pageIndex;
      })

      this.dataSource.data = response.body

      console.log(this.paginator.length)

    })


  }

}
