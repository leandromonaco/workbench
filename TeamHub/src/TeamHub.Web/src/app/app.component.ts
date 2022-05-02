import { HttpHeaders, HttpClient } from '@angular/common/http';
import { OnInit } from '@angular/core';
import { Component } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})

export class AppComponent implements OnInit
{

  data = {
    title: 'Team Hub'
  };

  employees: any;

  constructor(private http: HttpClient) { }

  async ngOnInit()
  {
    var token:string = await this.getToken();
    this.getEmployees(token);
  }

  getEmployees(token)
  {
    var httpHeaders = new HttpHeaders({
        'Authorization': 'Bearer ' + token
      });

    this.http.get('https://localhost:7209/employees', {headers: httpHeaders }).subscribe(response => {
      this.employees = response;
    }, error => {
      console.log(error);
    })
  }


   async getToken(): Promise<any>
   {
     return await this.http.get('https://localhost:7209/token', { responseType: 'text' }).toPromise();
   }

   onInputClicked()
   {
     alert('input clicked');
   }

   onKeyUp(newTitle:string){
     this.data.title = newTitle;
   }

}
