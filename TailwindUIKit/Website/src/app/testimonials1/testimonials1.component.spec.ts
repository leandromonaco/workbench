import { ComponentFixture, TestBed } from '@angular/core/testing';

import { Testimonials1Component } from './testimonials1.component';

describe('Testimonials1Component', () => {
  let component: Testimonials1Component;
  let fixture: ComponentFixture<Testimonials1Component>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ Testimonials1Component ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(Testimonials1Component);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
