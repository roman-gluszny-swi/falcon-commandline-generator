import { Component, Input, Host, ViewChild } from '@angular/core';
import { CommandlineGenerator } from '../commandlineGenerator';
import { AppComponent } from '../app.component';
import { FalconCatalogStage } from '../enums';
import { ComboboxV2Component } from '@solarwinds/nova-bits';
import { CommandlineGeneratorService } from '../commandline-generator.service';

interface IFalconCatalogStageItem {
  id: FalconCatalogStage;
  name: string;
}

@Component({
  selector: 'app-commandline',
  templateUrl: './commandline.component.html',
  styleUrls: ['./commandline.component.css']
})
export class CommandlineComponent{
  @Input() itemType: string;

  commandlineGenerator: CommandlineGenerator;
  commandlineGeneratorService: CommandlineGeneratorService;

  @ViewChild("falconCatalogStageCombo") private falconCatalogStageCombo: ComboboxV2Component;
  @ViewChild("installerCatalogStageCombo") private installerCatalogStageCombo: ComboboxV2Component;

  falconCatalogStages: IFalconCatalogStageItem[] = [
    { id: FalconCatalogStage.Ci, name: "CI" },
    { id: FalconCatalogStage.Stable, name: "Stable" },
    { id: FalconCatalogStage.Staging, name: "Staging" },
    { id: FalconCatalogStage.Production_Internal, name: "Production_Internal" },
    { id: FalconCatalogStage.Production, name: "Production" }
  ];

  constructor(@Host() parent: AppComponent, commandlineGeneratorService: CommandlineGeneratorService) {
    this.commandlineGeneratorService = commandlineGeneratorService;
    this.commandlineGenerator = parent.commandlineGenerator;
  }

  onFalconCatalogStageChange() {
    var falconCatalogStageComboSelectedValue = ((this.falconCatalogStageCombo.value) as any);
    this.commandlineGenerator.catalogStage = falconCatalogStageComboSelectedValue.id;
    this.regenerateCommandline();
  }

  onInstallerCatalogStageChange() {
    var installerCatalogStageComboSelectedValue = ((this.installerCatalogStageCombo.value) as any);
    this.commandlineGenerator.installerCatalogStage = installerCatalogStageComboSelectedValue.id;
    this.regenerateCommandline();
  }

  onRunInstallClick() {
    this.commandlineGeneratorService.runInstaller(this.commandlineGenerator);
    console.log("Downloading a running installer");
  }

  //displayWith function for falconCatalogStage model
  displayFalconCatalogStage(falconCatalogStage: IFalconCatalogStageItem): string {
    return falconCatalogStage?.name || this.falconCatalogStages[this.commandlineGenerator.catalogStage].name;
  }

  //displayWith function for installerCatalogStage model
  displayInstallerCatalogStage(falconCatalogStage: IFalconCatalogStageItem): string {
    return falconCatalogStage?.name || this.falconCatalogStages[this.commandlineGenerator.installerCatalogStage].name;
  }
  
  //Regenerate commandline
  regenerateCommandline() {
    this.commandlineGeneratorService.generateCommandline(this.commandlineGenerator).subscribe(result => { this.commandlineGenerator.commandline = result[0] }, error => console.error(error));
     console.log("Regenerating commandline");
  }
}
