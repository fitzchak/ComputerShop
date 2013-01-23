/// <reference path="../jquery.js" /> 
/// <reference path="serverApi.js" /> 

if (ComputerShop === undefined) { var ComputerShop = {}; }

if (ComputerShop.Main === undefined) { ComputerShop.Main = {}; }

ComputerShop.Main = function() {

    var computerTemplate = '';

    $.get('/Content/templates.tmpl.html', function(templates) {
        $.template('computerTemplate', $(templates).find('script#computerTemplate').text());

        TemplatesLoaded();
    });
    
    function TemplatesLoaded() {
        ComputerShop.ServerApi.GetComputers(function (computers) {
            $.tmpl('computerTemplate', computers).appendTo('div.computers-container');
            
        });
    }

}();



