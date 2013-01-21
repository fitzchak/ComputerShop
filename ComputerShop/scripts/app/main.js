/// <reference path="../jquery.js" /> 

if (ComputerShop === undefined) { var ComputerShop = {}; }

if (ComputerShop.Main === undefined) { ComputerShop.Main = {}; }

ComputerShop.Main = function() {

    var computerTemplate = '';

    $.get('/Content/templates.tmpl.html', function(templates) {
        $.template('computerTemplate', $(templates).find('script#computerTemplate').text());

        TemplatesLoaded();
    });
    
    //$.template('computerTemplate', ' <div><h2>${title}, ${ComputerBrand.Name}</h2><!--price: ${formatPrice(price)}--></div>');

    var host = 'http://localhost:7725/';

    function GetComputers(callback) {
        CallServer('computerApi/', callback);
    }

    function CallServer(path, callback) {
        $.ajax({
            url: host + path,
            type: 'GET',
            dataType: 'json',
            success: function (data) {
                callback(data);
            },
            error: function (x, y, z) {
                alert(x + '\n' + y + '\n' + z);
            }
        });
    }

    function TemplatesLoaded() {
        GetComputers(function (computers) {
            //$('#computerTemplate').tmpl(computers).appendTo('div.root.computer-container');
            $.tmpl('computerTemplate', computers).appendTo('div.root div.computer-container');
        });
        

    }

}();



