import { Component, Input, Host, ViewChildren, ViewChild, QueryList } from '@angular/core';
import { CatalogItem } from '../catalogItem';
import { CommandlineGenerator } from '../commandlineGenerator';
import { AppComponent } from '../app.component';
import { CatalogStage, ProductStage } from '../enums';
import { CheckboxComponent, ComboboxV2Component, TableComponent } from '@solarwinds/nova-bits';
import { CommandlineGeneratorService } from '../commandline-generator.service';
import { OnInit } from '@angular/core';

interface ICatalogStageItem {
  id: CatalogStage;
  name: string;
}

interface IProductStageItem {
  id: ProductStage;
  name: string;
}

@Component({
  selector: 'app-catalog-items',
  templateUrl: './catalog-items.component.html',
  styleUrls: ['./catalog-items.component.css']
})
export class CatalogItemsComponent implements OnInit {
  @Input() itemType: string;

  commandlineGenerator: CommandlineGenerator;
  commandlineGeneratorService: CommandlineGeneratorService;

  @ViewChildren("itemNameCombo") private itemNameCombo: QueryList<ComboboxV2Component>;
  @ViewChildren("catalogStageCombo") private catalogStageCombo: QueryList<ComboboxV2Component>;
  @ViewChildren("productStageCombo") private productStageCombo: QueryList<ComboboxV2Component>;
  @ViewChildren("versionNumberCombo") private versionNumberCombo: QueryList<ComboboxV2Component>;
  @ViewChildren("buildNumberCombo") private buildNumberCombo: QueryList<ComboboxV2Component>;
  @ViewChildren("updateCheckbox") private updateCheckbox: QueryList<CheckboxComponent>;
  @ViewChild(TableComponent) table: TableComponent<CatalogItem>;
  displayedColumns = ["name", "catalogStage", "productStage", "versionNumbers", "buildNumbers", "note", "updateOnly", "deleteButton"];
  
  allItemNames: string[];

  productStages: IProductStageItem[] = [
    { id: ProductStage.Build, name: "Build" },
    { id: ProductStage.Beta, name: "Beta" },
    { id: ProductStage.Rc, name: "RC" },
    { id: ProductStage.Rtm, name: "RTM" },
    { id: ProductStage.Hf, name: "HF" },
  ];

  catalogStages: ICatalogStageItem[] = [
    { id: CatalogStage.Ci, name: "CI" },
    { id: CatalogStage.Stable, name: "Stable" },
    { id: CatalogStage.Staging, name: "Staging" },
    { id: CatalogStage.Production, name: "Production" },
  ];

  constructor(@Host() parent: AppComponent, commandlineGeneratorService: CommandlineGeneratorService) {
    this.commandlineGeneratorService = commandlineGeneratorService;
    this.commandlineGenerator = parent.commandlineGenerator;
  }
    ngOnInit(): void {
      this.commandlineGeneratorService.getItemNames(this.itemType).subscribe(result => { this.allItemNames = result }, error => console.error(error));
  }

    //Add new catalog item to list
  onAddClick(): void {
    this.commandlineGenerator.catalogItems[this.itemType].push(new CatalogItem());
    console.log("New item added to " + this.itemType);
    this.regenerateCommandline();
  }

  //Delete product
  onDeleteClick(row: CatalogItem): void {
    const index = this.getItemIndex(row);
    this.commandlineGenerator.catalogItems[this.itemType].splice(index, 1);
    console.log("Item deleted from " + this.itemType);
    this.regenerateCommandline();
  };

  onCatalogItemNameChange(row: CatalogItem) {
    const index = this.getItemIndex(row);
    var itemNameComboSelectedValue = ((this.itemNameCombo.toArray()[index].value) as any);
    this.commandlineGenerator.catalogItems[this.itemType][index].name = itemNameComboSelectedValue;
    this.updateVersionNumbers(row);
  }

  onVersionNumberChange(row: CatalogItem) {
    const index = this.getItemIndex(row);
    this.buildNumberCombo.toArray()[index].clearValue();
    this.commandlineGenerator.catalogItems[this.itemType][index].buildNumber = "";

    var versionNumberSelectedValue = ((this.versionNumberCombo.toArray()[index].value) as any);
    this.commandlineGenerator.catalogItems[this.itemType][index].versionNumber = versionNumberSelectedValue;

    var catalogItem = this.commandlineGenerator.catalogItems[this.itemType][index];
    console.log("Retrieving build numbers");
    this.commandlineGeneratorService.getBuildNumbers(catalogItem, this.itemType).subscribe(result => {
      catalogItem.buildNumbers = result;
      this.buildNumberCombo.toArray()[index].writeValue(result[0]);
      this.commandlineGenerator.catalogItems[this.itemType][index].buildNumber = result[0];
      this.onBuildNumberChange(row);
    },
      error => console.error(error));
  }

  onCatalogStageChange(row: CatalogItem) {
    const index = this.getItemIndex(row);
    var catalogStageComboSelectedValue = ((this.catalogStageCombo.toArray()[index].value) as any);
    this.commandlineGenerator.catalogItems[this.itemType][index].catalogStage = catalogStageComboSelectedValue.id;
    this.updateVersionNumbers(row);
  }

  onProductStageChange(row: CatalogItem) {
    const index = this.getItemIndex(row);
    var productStageComboSelectedValue = ((this.productStageCombo.toArray()[index].value) as any);
    this.commandlineGenerator.catalogItems[this.itemType][index].productStage = productStageComboSelectedValue.id;
    this.updateVersionNumbers(row);
  }

  onBuildNumberChange(row: CatalogItem) {
    const index = this.getItemIndex(row);
    var buildNumberSelectedValue = ((this.buildNumberCombo.toArray()[index].value) as any);
    this.commandlineGenerator.catalogItems[this.itemType][index].buildNumber = buildNumberSelectedValue;

    var catalogItem = this.commandlineGenerator.catalogItems[this.itemType][index];
    this.commandlineGeneratorService.getNote(catalogItem, this.itemType).subscribe(result => {
      catalogItem.note = result;
    },
      error => console.error(error));
    this.regenerateCommandline();
  }

  onUpdateChanged(row: CatalogItem) {
    const index = this.getItemIndex(row);
    var updateValue = !((this.updateCheckbox.toArray()[index].checked) as any);
    this.commandlineGenerator.catalogItems[this.itemType][index].updateOnly = updateValue;
    this.regenerateCommandline();
  }

  //Regenerate commandline
  regenerateCommandline() {
    this.commandlineGeneratorService.generateCommandline(this.commandlineGenerator).subscribe(result => { this.commandlineGenerator.commandline = result[0] }, error => console.error(error));
    console.log("Regenerating commandline");
    this.table.renderRows();
  }

  //retrieves candidates of version numbers with currently set parameters
  updateVersionNumbers(row: CatalogItem) {
    const index = this.getItemIndex(row);
    var catalogItem = this.getItemAtIndex(row);
    console.log("Retrieving version numbers");
    this.commandlineGeneratorService.getVersionNumbers(catalogItem, this.itemType).subscribe(result => {
      catalogItem.versionNumbers = result;
      this.versionNumberCombo.toArray()[index].writeValue(result[0]);
      this.onVersionNumberChange(row);
    },
      error => console.error(error));
  }

  //displayWith function for catalogStage model
  displayCatalogStage(catalogStage: ICatalogStageItem): string {
    return catalogStage?.name || this.catalogStages[catalogStage.id].name;
  }

  //displayWith function for productStage model
  displayProductStage(productStage: IProductStageItem): string {
    return productStage?.name || this.productStages[productStage.id].name;
  }

  //returns list of added catalogItems with specified type (product/component)
  getItemsOfType() {
    return this.commandlineGenerator.catalogItems[this.itemType];
  }

  getItemIndex(row: CatalogItem): number {
    return this.commandlineGenerator.catalogItems[this.itemType].indexOf(row);
  }

  getItemAtIndex(row: CatalogItem) {
    const index = this.getItemIndex(row);
    return this.commandlineGenerator.catalogItems[this.itemType][index];
  }
}
