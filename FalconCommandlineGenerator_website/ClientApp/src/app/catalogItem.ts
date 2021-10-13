import { CatalogStage, ProductStage } from './enums';

export class CatalogItem {
    name: string;
    catalogStage: CatalogStage;
    productStage: ProductStage;
    versionNumber: string;
    buildNumber: string;
    versionNumbers: string[] = [];
    buildNumbers: string[] = [];
    note: string;
    updateOnly: boolean;

  constructor() {
    this.name = "";
    this.catalogStage = CatalogStage.Ci;
    this.productStage = ProductStage.Build;
    this.versionNumber = "";
    this.buildNumber = '';
    this.versionNumbers = [];
    this.buildNumbers = [];
    this.note = "";
    this.updateOnly = false;
  }
}
