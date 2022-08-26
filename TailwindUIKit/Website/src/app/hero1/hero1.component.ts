
import { Component, OnInit } from "@angular/core";

@Component({
  selector: 'app-hero1',
  templateUrl: './hero1.component.html',
  styleUrls: ['./hero1.component.css']
})

export class Hero1Component implements OnInit {
  show: boolean = false

  showMenu(){
    this.show =! this.show

  }


constructor() {

  }
  ngOnInit(): void {}
}
