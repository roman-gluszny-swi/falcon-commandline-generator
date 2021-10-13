import { CatalogItem } from './catalogItem';
import { FalconCatalogStage } from './enums';


export class CommandlineGenerator {
  catalogItems: { [itemType: string]: CatalogItem[] } = {
    "products": [],
    "components": []
  }
  catalogStage: FalconCatalogStage = FalconCatalogStage.Ci;
  installerCatalogStage: FalconCatalogStage = FalconCatalogStage.Ci;
  autoUpdate: boolean = true;
  noTests: boolean = false;
  silent: boolean = false;
  commandline: string = "";
}


