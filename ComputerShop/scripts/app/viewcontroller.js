/// <reference path="~/scripts/breeze.debug.js" />
/// <reference path="~/scripts/backbone.stickit.js" /> 

(function ($, Backbone, dataservice) {

    // Get templates
    var mainContent;
    var computerTemplate;
    var computerBrandSidebarContent;
    var computerBrandMenuTemplate;

    $(document).ready(function () {
        documentReady();
    });

    function documentReady() {
        mainContent = $(".main-content");
        computerBrandSidebarContent = $(".sidebar-content");

        loadTemplates();
    }

    var loadTemplates = function () {
        $.get('/Content/templates.tmpl.html', function (templates) {
            computerTemplate = $(templates).find('script#computerTemplate').html();
            computerBrandMenuTemplate = $(templates).find('script#computerBrandMenuTemplate').html();
            templatesLoaded();
        });
    };
    var computerBrandMenuView = Backbone.View.extend({
        bindings: {
            '#name': 'name'
        },
        events: {
            'click .computer-brand-box': 'filterComputers'
        },
        render: function () {
            this.$el.html(computerBrandMenuTemplate);
            this.stickit();
            return this;
        },
        filterComputers: function () {
            var self = this;

            getComputers(self.model.id);
        }
    });

    var computerView = Backbone.View.extend({
        bindings: {
            '#description': 'description',
            '#brand-name': {
                observe: 'computerBrand',
                onGet: function (value) {
                    // onGet called after title *or* author model attributes change.
                    return value.get('name');
                }
            },
            '#model-name': {
                observe: 'computerModel'

            },
            '#processor-name': {
                observe: 'processor',
                onGet: function (value) {
                    if (value == null) {
                        return dataservice.getNotAvailableValue();
                    }
                    return value.get('name');
                }
            },
            '#ram-capacity': {
                observe: ['ramCapacity', 'ramUnit'],
                onGet: function (values) {

                    return values[0] + ' ' + dataservice.convertEnumUnitToString(values[1]);
                }
            },
            '#hdd-capacity': {
                observe: ['harddiskCapacity', 'harddiskCapacityUnit'],
                onGet: function (values) {
                    if (values[0] == "" || values[1] == "") {
                        return dataservice.getNotAvailableValue();
                    }

                    return values[0] + ' ' + dataservice.convertEnumUnitToString(values[1]);
                }
            },
            '#price': {
                observe: 'price',
                onGet: function (value) {
                    if (value == "") {
                        return dataservice.getNotAvailableValue();
                    }

                    return value + ' eur';
                }
            }
        },
        events: {
            //'click .computer-brand-box': 'filterComputers'
        },
        render: function () {
            this.$el.html(computerTemplate);

            this.stickit();
            return this;
        }

    });

    var getComputerBrands = function () {
        computerBrandSidebarContent.empty();

        dataservice.getComputerBrands()
            .then(gotComputerBrands);

        function gotComputerBrands(computerBrands) {

            var allComputerBrands = dataservice.createComputerBrand();
            allComputerBrands.id = -1;
            allComputerBrands.set('name', 'all');

            var view = new computerBrandMenuView({ model: allComputerBrands });
            computerBrandSidebarContent.append(view.render().el);

            computerBrands.forEach(
                function (computerBrand) {
                    var view = new computerBrandMenuView({ model: computerBrand });
                    computerBrandSidebarContent.append(view.render().el);
                }
            );

            var mapped = $.map(computerBrands, function (value) {
                return { value: value.attributes.name, id: value.id };
            });

            $("#searchByBrand").autocomplete({
                source: mapped
            });
        }
    };

    var getComputers = function (computerBrandId) {
        
        dataservice.getComputers(computerBrandId)
            .then(gotComputers);

        function gotComputers(computers) {
            mainContent.empty();
            
            computers.forEach(
                function (computer) {
                    var view = new computerView({ model: computer });
                    mainContent.append(view.render().el);
                });
        }
    };

    function templatesLoaded() {
        getComputers();
        getComputerBrands();
    }


})(jQuery, Backbone, app.dataservice);