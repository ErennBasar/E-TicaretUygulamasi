import { TestBed } from '@angular/core/testing';

import { CustomToastr } from './custom-toastr';

describe('CustomToastr', () => {
  let service: CustomToastr;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(CustomToastr);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
