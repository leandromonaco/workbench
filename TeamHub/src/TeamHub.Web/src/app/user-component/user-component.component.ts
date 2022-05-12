import { Component, Input, OnInit } from '@angular/core';
import { Employee } from '../model/employee';

@Component({
  selector: 'app-user-component',
  templateUrl: './user-component.component.html',
  styleUrls: ['./user-component.component.css']
})
export class UserComponentComponent implements OnInit {

  @Input()
  employee:Employee;

  constructor() { }

  ngOnInit(): void {
  }

}
