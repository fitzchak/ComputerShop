/// <reference path="../jquery.js" />
/// <reference path="../breeze.debug.js" /> 
/// <reference path="serverApi.js" /> 

if (ComputerShop === undefined) { var ComputerShop = {}; }

if (ComputerShop.Main === undefined) { ComputerShop.Main = {}; }

ComputerShop.Main = function () {

    $.get('/Content/templates.tmpl.html', function (templates) {
        $.template('computerTemplate', $(templates).find('script#computerTemplate').text());
        $.template('computerBrandTemplate', $(templates).find('script#computerBrandTemplate').text());
        templatesLoaded();
    });

    function templatesLoaded() {
        ComputerShop.ServerApi.GetComputers(function (computers) {
            $.tmpl('computerTemplate', computers).appendTo('div.computers-container');
        });

        ComputerShop.ServerApi.GetComputerBrands(function (computerBrands) {
            $.tmpl('computerBrandTemplate', computerBrands).appendTo('.computer-brands-container');
        });

        var serviceName = 'api/ComputerShop';

        var manager = new breeze.EntityManager(serviceName);

        var query = new breeze.EntityQuery()
            .from("Processors")
            .using(manager)
            .execute()
            .then(querySucceeded)
            .fail(function (error) {
                querySucceeded(error.detail);
            });
            
        function querySucceeded(data) {
            //logger.success("fetched cars");
            return data.results;
        }
    }

    $(document).ready(function () {
        documentReady();
    });

    function documentReady() {
        ComputerShop.ServerApi.GetComputerBrands(function (computerBrands) {
            var mapped = $.map(computerBrands, function (value) {
                return { value: value.Name };
            });

            $("#searchByBrand").autocomplete({
                source: mapped
            });
        });
    }
} ();



