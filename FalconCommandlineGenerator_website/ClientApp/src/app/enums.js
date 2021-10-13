"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.FalconCatalogStage = exports.CatalogStage = exports.ProductStage = void 0;
var ProductStage;
(function (ProductStage) {
    ProductStage[ProductStage["Build"] = 1] = "Build";
    ProductStage[ProductStage["Beta"] = 2] = "Beta";
    ProductStage[ProductStage["Rc"] = 3] = "Rc";
    ProductStage[ProductStage["Rtm"] = 4] = "Rtm";
    ProductStage[ProductStage["Hf"] = 5] = "Hf";
})(ProductStage = exports.ProductStage || (exports.ProductStage = {}));
var CatalogStage;
(function (CatalogStage) {
    CatalogStage[CatalogStage["Ci"] = 1] = "Ci";
    CatalogStage[CatalogStage["Stable"] = 2] = "Stable";
    CatalogStage[CatalogStage["Staging"] = 3] = "Staging";
    CatalogStage[CatalogStage["Production"] = 4] = "Production";
})(CatalogStage = exports.CatalogStage || (exports.CatalogStage = {}));
var FalconCatalogStage;
(function (FalconCatalogStage) {
    FalconCatalogStage[FalconCatalogStage["Ci"] = 1] = "Ci";
    FalconCatalogStage[FalconCatalogStage["Stable"] = 2] = "Stable";
    FalconCatalogStage[FalconCatalogStage["Staging"] = 3] = "Staging";
    FalconCatalogStage[FalconCatalogStage["Production_Internal"] = 4] = "Production_Internal";
    FalconCatalogStage[FalconCatalogStage["Production"] = 5] = "Production";
})(FalconCatalogStage = exports.FalconCatalogStage || (exports.FalconCatalogStage = {}));
//# sourceMappingURL=enums.js.map