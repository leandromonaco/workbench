import { ComponentFixture, TestBed } from '@angular/core/testing';

import { Faq1Component } from './faq1.component';

describe('Faq1Component', () => {
  let component: Faq1Component;
  let fixture: ComponentFixture<Faq1Component>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ Faq1Component ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(Faq1Component);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
