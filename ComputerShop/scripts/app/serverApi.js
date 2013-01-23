/// <reference path="../jquery.js" /> 
/// <reference path="util.js" /> 

if (ComputerShop === undefined) { var ComputerShop = {}; }

if (ComputerShop.ServerApi === undefined) { ComputerShop.ServerApi = {}; }
ComputerShop.ServerApi = function () {

    function GetComputers(callback) {
        ComputerShop.Util.CallServer('computerApi/', callback);
    }

    return {
        GetComputers: GetComputers
    }
}();