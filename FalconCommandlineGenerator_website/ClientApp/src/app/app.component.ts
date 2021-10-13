import { Component } from '@angular/core';
import { CommandlineGenerator } from './commandlineGenerator';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'falconCommandlineGeneratorWebsite';
  commandlineGenerator: CommandlineGenerator;

  constructor() {
    this.commandlineGenerator = new CommandlineGenerator();
  }
}
