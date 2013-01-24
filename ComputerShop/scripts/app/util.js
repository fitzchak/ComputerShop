/// <reference path="../jquery.js" /> 

if (ComputerShop === undefined) { var ComputerShop = {}; }

if (ComputerShop.Util === undefined) { ComputerShop.Util = {}; }
ComputerShop.Util = function () {

    var host = 'http://localhost:7725/';

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

    return {
        CallServer: CallServer
    };

} ();