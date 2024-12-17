import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EditAdComponent } from './edit-ad.component';

describe('EditAdComponent', () => {
  let component: EditAdComponent;
  let fixture: ComponentFixture<EditAdComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [EditAdComponent]
    });
    fixture = TestBed.createComponent(EditAdComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
