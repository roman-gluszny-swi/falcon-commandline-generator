"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.CatalogItem = void 0;
var enums_1 = require("./enums");
var CatalogItem = /** @class */ (function () {
    function CatalogItem() {
        this.versionNumbers = [];
        this.buildNumbers = [];
        this.name = "";
        this.catalogStage = enums_1.CatalogStage.Ci;
        this.productStage = enums_1.ProductStage.Build;
        this.versionNumber = "";
        this.buildNumber = '';
        this.versionNumbers = [];
        this.buildNumbers = [];
        this.note = "";
        this.updateOnly = false;
    }
    return CatalogItem;
}());
exports.CatalogItem = CatalogItem;
//# sourceMappingURL=catalogItem.js.map