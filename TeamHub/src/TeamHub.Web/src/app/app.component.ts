import { HttpClient } from '@angular/common/http';
import { OnInit } from '@angular/core';
import { Component } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})

export class AppComponent implements OnInit
{

  title = 'Team Hub';
  employees: any;

    constructor(private http: HttpClient) { }

  ngOnInit()
  {
    this.getEmployees();
  }

  getEmployees()
  {
    this.http.get('https://localhost:7209/employees').subscribe(response => {
      this.employees = response;
    }, error => {
      console.log(error);
    })
  }
}
