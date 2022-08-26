import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppComponent } from './app.component';
import { Hero1Component } from './hero1/hero1.component';
import { Faq1Component } from './faq1/faq1.component';
import { Team1Component } from './team1/team1.component';
import { Testimonials1Component } from './testimonials1/testimonials1.component';

@NgModule({
  declarations: [
    AppComponent,
    Hero1Component,
    Faq1Component,
    Team1Component,
    Testimonials1Component
  ],
  imports: [
    BrowserModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
