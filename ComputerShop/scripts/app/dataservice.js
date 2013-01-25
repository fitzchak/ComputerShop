/// <reference path="../breeze.debug.js" />
app.dataservice = (function (breeze, logger) {

    /*** Breeze Configuration ***/

    // configure Breeze for Backbone (config'd for Web API by default)
    breeze.config.initializeAdapterInstances({ modelLibrary: "backbone" });

    // Declare the camel case name convention to be the norm
    breeze.NamingConvention.camelCase.setAsDefault();

    var op = breeze.FilterQueryOp;

    // service name is route to the Web API controller
    var serviceName = 'api/ComputerShop';

    /*** dataservice proper ***/

    // manager (aka context) is the service gateway and cache holder
    var manager = new breeze.EntityManager(serviceName);

    var getComputers = function (computerBrandId) {
        var chunk = breeze.EntityQuery
            .from("Computers");

        if (computerBrandId > 0) {
            chunk = chunk
                .where("computerBrand.id", "==", computerBrandId);
        }

        return chunk
            .expand("computerBrand, processor")
            .using(manager)
            .execute()
            .then(querySucceeded)
            .fail(queryFailed);

        function querySucceeded(data) {
            logger.success("fetched computers");
            return data.results;
        }
    };

    var getComputerBrandsFromCache = function () {
        return manager.getEntities("ComputerBrand");
    };

    var getProcessorsFromCache = function () {
        return manager.getEntities("Processor");
    };

    var getComputerBrands = function () {
        return breeze.EntityQuery
            .from("ComputerBrands")
            .using(manager)
            .execute()
            .then(querySucceeded)
            .fail(queryFailed);

        function querySucceeded(data) {
            logger.success("fetched computer brands");
            return data.results;
        }
    };

    var createComputerBrand = function () {
        var computerBrandType = manager.metadataStore.getEntityType("ComputerBrand");
        var newComputerBrand = computerBrandType.createEntity();
        return manager.addEntity(newComputerBrand);
    };

    var unitToStringDictionary =
        [
            { id: 0, value: "kB" },
            { id: 1, value: "MB" },
            { id: 2, value: "GB" },
            { id: 3, value: "TB" }
        ];

    var getUnits = function () {
        return unitToStringDictionary;
    };
    var convertEnumUnitToString = function (intValue) {
        return unitToStringDictionary[intValue].value;
    };

    var getNotAvailableValue = function () {
        return "NA";
    };

    var saveChanges = function (entity) {
        var msg = manager.hasChanges() ? "changes saved" : "nothing to save";
        return manager.saveChanges(entity)
            .then(function () { logger.success(msg); })
            .fail(saveFailed);
    };

    return {
        getComputers: getComputers,
        getComputerBrands: getComputerBrands,
        getComputerBrandsFromCache: getComputerBrandsFromCache,
        getProcessorsFromCache: getProcessorsFromCache,
        getUnits: getUnits,
        createComputerBrand: createComputerBrand,
        convertEnumUnitToString: convertEnumUnitToString,
        getNotAvailableValue: getNotAvailableValue,
        saveChanges: saveChanges
    };

    function queryFailed(error) {
        logger.error("Query failed: " + error.message);
    }

    function loadOptionsFailed(error) {
        logger.error("Load of options failed: " + error.message);
    }

    function saveFailed(error) {
        logger.error("Save failed: " + error.message);
    }

})(breeze, app.logger);