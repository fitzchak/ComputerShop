/// <reference path="../jquery.js" /> 
/// <reference path="util.js" /> 

if (ComputerShop === undefined) { var ComputerShop = {}; }

if (ComputerShop.ServerApi === undefined) { ComputerShop.ServerApi = {}; }
ComputerShop.ServerApi = function () {

    function GetComputers(callback) {
        ComputerShop.Util.CallServer('api/ComputerShop/computers/', callback);
    }

    function GetComputerBrands(callback) {
        ComputerShop.Util.CallServer('api/ComputerShop/computerBrands/', callback);
    }

    return {
        GetComputers: GetComputers,
        GetComputerBrands: GetComputerBrands
    }
}();