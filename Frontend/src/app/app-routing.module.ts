import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { RegisterComponent } from './Components/register/register.component';
import { LoginComponent } from './Components/login/login.component';
import { ResultsComponent } from './Components/results/results.component';
import { IsLoggedGuard } from './Guards/is-logged.guard';
import { DevicesComponent } from './Components/devices/devices.component';
import { ChartsComponent } from './Components/charts/charts.component';
import { AddDeviceComponent } from './Components/add-device/add-device.component';

const routes: Routes = [
  {
    path: '',
    component: RegisterComponent
  },
  {
    path: 'login',
    component: LoginComponent
  },
  {
    path: 'results',
    component: ResultsComponent,
    canActivate: [IsLoggedGuard]
  },
  {
    path: 'devices',
    component: DevicesComponent,
    canActivate: [IsLoggedGuard]
  },
  {
    path: 'charts',
    component: ChartsComponent,
    canActivate: [IsLoggedGuard]
  },
  {
    path: 'add-device',
    component: AddDeviceComponent,
    canActivate: [IsLoggedGuard]
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
