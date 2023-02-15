import { Component, OnInit, ViewChild } from '@angular/core';
import { JwtHelperService } from '@auth0/angular-jwt';
import { Chart, ChartConfiguration, ChartEvent, ChartType } from 'chart.js';
import { BaseChartDirective } from 'ng2-charts';
import { Result } from 'src/app/Interfaces/result';
import { ResultsService } from 'src/app/Services/results.service';

import { default as Annotation } from 'chartjs-plugin-annotation';


@Component({
  selector: 'app-charts',
  templateUrl: './charts.component.html',
  styleUrls: ['./charts.component.scss']
})
export class ChartsComponent implements OnInit {

  isDataAvailable:boolean=false;
  constructor(private _resultsService: ResultsService, private jwtHelper: JwtHelperService) {
  }
  ngOnInit(): void {
    const token = localStorage.getItem("token");
    const decodedToken = this.jwtHelper.decodeToken(token!);
    console.log(decodedToken.Id)

    this._resultsService.userResults(decodedToken.Id).subscribe((data: any) => {
      data.forEach((element: Result) => {
        console.log(this.lineChartDataHeart.datasets)
        this.lineChartDataHeart.labels?.push(element.date)
        this.lineChartDataHeart.datasets[0].data.push(element.averageHeartRate)
        this.lineChartDataHeart.datasets[1].data.push(element.maximumHeartRate)
        this.lineChartDataHeart.datasets[2].data.push(element.minimumHeartRate)

        this.lineChartDataSpO2.labels?.push(element.date)
        this.lineChartDataSpO2.datasets[0].data.push(element.averageSpO2)
        this.lineChartDataSpO2.datasets[1].data.push(element.maximumSpO2)
        this.lineChartDataSpO2.datasets[2].data.push(element.minimumSpO2)
        this.isDataAvailable = true
      });

    })
  }

  public lineChartDataHeart: ChartConfiguration['data'] = {
    datasets: [
      {
        data: [],
        label: 'Average heartrate',
        backgroundColor: 'rgba(148,159,177,0.2)',
        borderColor: 'rgba(148,159,177,1)',
        pointBackgroundColor: 'rgba(148,159,177,1)',
        pointBorderColor: '#fff',
        pointHoverBackgroundColor: '#fff',
        pointHoverBorderColor: 'rgba(148,159,177,0.8)',
        //fill: 'origin',
      },
      {
        data: [],
        label: 'Maximium heartrate',
        backgroundColor: 'rgba(77,83,96,0.2)',
        borderColor: 'rgba(77,83,96,1)',
        pointBackgroundColor: 'rgba(77,83,96,1)',
        pointBorderColor: '#fff',
        pointHoverBackgroundColor: '#fff',
        pointHoverBorderColor: 'rgba(77,83,96,1)',
        //fill: 'origin',
      },
      {
        data: [],
        label: 'Minimum Heartrate',
        yAxisID: 'y1',
        backgroundColor: 'rgba(255,0,0,0.3)',
        borderColor: 'red',
        pointBackgroundColor: 'rgba(148,159,177,1)',
        pointBorderColor: '#fff',
        pointHoverBackgroundColor: '#fff',
        pointHoverBorderColor: 'rgba(148,159,177,0.8)',
        //fill: 'origin',
      }
    ],
    labels: []
  };

  public lineChartDataSpO2: ChartConfiguration['data'] = {
    datasets: [
      {
        data: [],
        label: 'Average SpO2',
        backgroundColor: 'rgba(148,159,177,0.2)',
        borderColor: 'rgba(148,159,177,1)',
        pointBackgroundColor: 'rgba(148,159,177,1)',
        pointBorderColor: '#fff',
        pointHoverBackgroundColor: '#fff',
        pointHoverBorderColor: 'rgba(148,159,177,0.8)',
        //fill: 'origin',
      },
      {
        data: [],
        label: 'Maximium SpO2',
        backgroundColor: 'rgba(77,83,96,0.2)',
        borderColor: 'rgba(77,83,96,1)',
        pointBackgroundColor: 'rgba(77,83,96,1)',
        pointBorderColor: '#fff',
        pointHoverBackgroundColor: '#fff',
        pointHoverBorderColor: 'rgba(77,83,96,1)',
        //fill: 'origin',
      },
      {
        data: [],
        label: 'Minimum SpO2',
        yAxisID: 'y1',
        backgroundColor: 'rgba(255,0,0,0.3)',
        borderColor: 'red',
        pointBackgroundColor: 'rgba(148,159,177,1)',
        pointBorderColor: '#fff',
        pointHoverBackgroundColor: '#fff',
        pointHoverBorderColor: 'rgba(148,159,177,0.8)',
        //fill: 'origin',
      }
    ],
    labels: []
  };

  public lineChartOptionsHeart: ChartConfiguration['options'] = {
    elements: {
      line: {
        tension: 0.5
      }
    },
    scales: {
      // We use this empty structure as a placeholder for dynamic theming.
      y:
        {
          position: 'left',
          min: 0,
          max: 220
        },
      y1: {
        position: 'right',
        min: 0,
        max: 220
      }
    },

    plugins: {
      legend: { display: true },
      annotation: {

      }
    }
  };

  public lineChartOptionsSpO2: ChartConfiguration['options'] = {
    elements: {
      line: {
        tension: 0.5
      }
    },
    scales: {
      // We use this empty structure as a placeholder for dynamic theming.
      y:
        {
          position: 'left',
          min: 80,
          max: 100
        },
      y1: {
        position: 'right',
        min: 80,
        max: 100
      }
    },

    plugins: {
      legend: { display: true },
      annotation: {

      }
    }
  };

  public lineChartType: ChartType = 'line';

  @ViewChild(BaseChartDirective) chartHeart?: BaseChartDirective;


  public chartClicked({ event, active }: { event?: ChartEvent, active?: {}[] }): void {
    console.log(event, active);
  }

  public chartHovered({ event, active }: { event?: ChartEvent, active?: {}[] }): void {
    console.log(event, active);
  }
}
