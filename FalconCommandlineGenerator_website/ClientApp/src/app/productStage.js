var ProductStage;
(function (ProductStage) {
    ProductStage[ProductStage["Build"] = 0] = "Build";
    ProductStage[ProductStage["Beta"] = 1] = "Beta";
    ProductStage[ProductStage["Rc"] = 2] = "Rc";
    ProductStage[ProductStage["Rtm"] = 3] = "Rtm";
    ProductStage[ProductStage["Hf"] = 4] = "Hf";
})(ProductStage || (ProductStage = {}));
var CatalogStage;
(function (CatalogStage) {
    CatalogStage[CatalogStage["Ci"] = 0] = "Ci";
    CatalogStage[CatalogStage["Stable"] = 1] = "Stable";
    CatalogStage[CatalogStage["Staging"] = 2] = "Staging";
    CatalogStage[CatalogStage["Production"] = 3] = "Production";
})(CatalogStage || (CatalogStage = {}));
//# sourceMappingURL=productStage.js.map