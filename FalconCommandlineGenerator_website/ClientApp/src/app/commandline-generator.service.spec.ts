import { TestBed } from '@angular/core/testing';

import { CommandlineGeneratorService } from './commandline-generator.service';

describe('CommandlineGeneratorService', () => {
  let service: CommandlineGeneratorService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(CommandlineGeneratorService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
