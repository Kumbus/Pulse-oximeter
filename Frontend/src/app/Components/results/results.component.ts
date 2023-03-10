import { HttpResponse } from '@angular/common/http';
import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatSort, Sort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { JwtHelperService } from '@auth0/angular-jwt';
import { Result } from 'src/app/Interfaces/result';
import { ResultsService } from 'src/app/Services/results.service';
import { Options } from '@angular-slider/ngx-slider';
import { FormControl, FormGroup } from '@angular/forms';

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

  range: FormGroup = new FormGroup({
      start: new FormControl(),
      end: new FormControl(),
    });

  orderBy: string = "date"

  ngOnInit(): void {
    const token = localStorage.getItem("token");
    const decodedToken = this.jwtHelper.decodeToken(token!);

    this._resultsService.pagedUserResults(decodedToken.Id, 1, 10, this.orderBy).subscribe((response: HttpResponse<any>) => {

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

    console.log(this.range)

      const start = this.range.get('start')?.value?.toISOString().slice(0,-5)
      const end = this.range.get('end')?.value?.toISOString().slice(0,-5)

    this._resultsService.pagedUserResults(decodedToken.Id, event.pageIndex + 1, event.pageSize, this.orderBy, start, end).subscribe((response: HttpResponse<any>) => {
      const paginationParameters = JSON.parse(response.headers.get('x-pagination')!)
      setTimeout(() => {
        this.paginator.length = paginationParameters.TotalItemCount
        this.pageSize = event.pageSize;
        this.pageIndex = event.pageIndex;
      })

      this.dataSource.data = response.body
    })
  }

  applyDate = () => {
    const start = this.range.get('start')!.value.toISOString().slice(0,-5)
    const end = this.range.get('end')?.value?.toISOString().slice(0,-5)

    const token = localStorage.getItem("token");
    const decodedToken = this.jwtHelper.decodeToken(token!);

    this._resultsService.pagedUserResults(decodedToken.Id, 1, this.pageSize, this.orderBy, start, end).subscribe((response: HttpResponse<any>) => {
      const paginationParameters = JSON.parse(response.headers.get('x-pagination')!)
      setTimeout(() => {
        this.paginator.length = paginationParameters.TotalItemCount
        this.pageIndex = 0;
        this.paginator.pageIndex = 0
      })

      this.dataSource.data = response.body
    })
  }

  onSortChange = (event: Sort) => {
    this.orderBy = event.active + " " + event.direction

    const token = localStorage.getItem("token");
    const decodedToken = this.jwtHelper.decodeToken(token!);

    const start = this.range.get('start')?.value?.toISOString().slice(0,-5)
    const end = this.range.get('end')?.value?.toISOString().slice(0,-5)

    this._resultsService.pagedUserResults(decodedToken.Id, this.pageIndex + 1 , this.pageSize, this.orderBy, start, end).subscribe((response: HttpResponse<any>) => {
      const paginationParameters = JSON.parse(response.headers.get('x-pagination')!)
      setTimeout(() => {
        this.paginator.length = paginationParameters.TotalItemCount
        this.pageIndex = paginationParameters.PageNumber - 1;
        this.paginator.pageIndex = paginationParameters.PageNumber - 1
      })
      this.dataSource.data = response.body
    })
  }
}
