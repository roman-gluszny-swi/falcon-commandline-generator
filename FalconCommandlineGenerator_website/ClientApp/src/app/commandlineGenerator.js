"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.CommandlineGenerator = void 0;
var catalogItem_1 = require("./catalogItem");
var enums_1 = require("./enums");
var CommandlineGenerator = /** @class */ (function () {
    function CommandlineGenerator() {
        this.catalogItems = {
            "products": [],
            "components": []
        };
        this.catalogStage = enums_1.FalconCatalogStage.Ci;
        this.installerCatalogStage = enums_1.FalconCatalogStage.Ci;
        this.autoUpdate = true;
        this.noTests = false;
        this.silent = false;
        this.commandline = "";
    }
    CommandlineGenerator.prototype.addItem = function (itemType) {
        this.catalogItems[itemType].push(new catalogItem_1.CatalogItem());
    };
    return CommandlineGenerator;
}());
exports.CommandlineGenerator = CommandlineGenerator;
//# sourceMappingURL=commandlineGenerator.js.map