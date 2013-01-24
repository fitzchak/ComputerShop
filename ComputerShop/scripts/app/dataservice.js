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
    var saveChanges = function () {
        var msg = manager.hasChanges() ? "changes saved" : "nothing to save";
        return manager.saveChanges()
            .then(function () { logger.success(msg); })
            .fail(saveFailed);
    };

    return {
        getComputers: getComputers,
        getComputerBrands: getComputerBrands
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