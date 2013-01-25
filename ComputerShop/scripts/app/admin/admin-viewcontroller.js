/// <reference path="~/scripts/breeze.debug.js" />
/// <reference path="~/scripts/backbone.stickit.js" />
(function ($, Backbone, dataservice) {

    // Get templates
    var menuContent;
    var menuTemplate;
    var content;

    var computerEditTemplate;
    var computerBrandEditTemplate;

    $(document).ready(function () {
        documentReady();
    });

    function documentReady() {
        content = $(".main-content");
        menuContent = $("#menuList");
        loadTemplates();
    }

    var loadTemplates = function () {
        $.get('/Content/templates.tmpl.html', function (templates) {
            computerEditTemplate = $(templates).find('script#computerEditTemplate').html();
            computerBrandEditTemplate = $(templates).find('script#computerBrandEditTemplate').html();
            menuTemplate = $(templates).find('script#adminMenuTemplate').html();

            templatesLoaded();
        });
    };

    var menuView = Backbone.View.extend({
        bindings: {
            '#name': 'name'
        },
        events: {

        },
        render: function () {
            this.$el.html(menuTemplate);
            this.stickit();

            return this;
        }
    }
    );

    var computerBrandEditView = Backbone.View.extend({
        bindings: {
            '#id': 'id',
            '#name': 'name',
            '#description': 'description'
        },
        events: {

        },
        render: function () {
            this.$el.html(computerBrandEditTemplate);
            this.stickit();

            return this;
        }
    });

    var ComputerView = Backbone.View.extend({
        bindings: {
            '#id': 'id',
            'select#brand': {
                observe: 'computerBrand',
                selectOptions: {
                    collection: function () {
                        return dataservice.getComputerBrandsFromCache();
                    },
                    labelPath: 'attributes.name'
                }
            },
            '#description': 'description',

            '#model': 'computerModel',
            'select#processor': {
                observe: 'processor',
                selectOptions: {
                    collection: function () {
                        return dataservice.getProcessorsFromCache();
                    },
                    labelPath: 'attributes.name'
                }
            },
            '#ram-capacity': 'ramCapacity',
            'select#ram-unit': {
                observe: 'ramUnit',
                selectOptions: {
                    collection: function () {
                        return dataservice.getUnits();
                    },

                    labelPath: 'value',
                    valuePath: 'id'
                }
            },
            '#hdd-capacity': 'harddiskCapacity',
            'select#hdd-unit': {
                observe: 'harddiskCapacityUnit',
                selectOptions: {
                    collection: function () {
                        return dataservice.getUnits();
                    },

                    labelPath: 'value',
                    valuePath: 'id'
                }
            },
            '#price': 'price'
        },
        events: {
            //'click .computer-brand-box': 'filterComputers'
            'click #save': 'save',
            'click #cancel': 'cancel'
        },
        render: function () {
            this.$el.html(computerEditTemplate);
            var self = this;

            this.model.entityAspect.propertyChanged.subscribe(function (changeArgs) {
                if (changeArgs.propertyName == null || changeArgs.propertyName == 'timestamp') {
                    return;
                }
                // show save and cancel buttons
                $("#actions", self.$el).removeClass('hidden');

            });

            this.stickit();

            return this;
        },
        save: function () {
            dataservice.saveChanges([this.model]);
            $("#actions", self.$el).addClass('hidden');
        },
        cancel: function () {
            this.model.entityAspect.rejectChanges();
            $("#actions", self.$el).addClass('hidden');
        }


    });

    var getProcessors = function () {

    };

    var getComputerBrands = function () {
        content.empty();

        dataservice.getComputerBrands()
            .then(gotComputerBrands);

        function gotComputerBrands(computerBrands) {
            computerBrands.forEach(
                function (computerBrand) {
                    var view = new computerBrandEditView({ model: computerBrand });
                    content.append(view.render().el);
                }
            );
        }
    };

    var getComputers = function (computerBrandId) {

        dataservice.getComputers(computerBrandId)
            .then(gotComputers);

        function gotComputers(computers) {
            content.empty();

            computers.forEach(
                function (computer) {
                    var view = new ComputerView({ model: computer });
                    content.append(view.render().el);
                });
        }
    };

    var showMenu = function() {
        var menuItems = [
            { Name: "Computers", func: getComputers },
            { Name: "Brands", func: getComputerBrands },
            { Name: "Processors", func: getProcessors }
        ];

        menuContent.empty();

        menuItems.forEach(
                function (item) {
                    var view = new menuView({ model: item });
                    menuContent.append(view.render().el);
                });
    };

    function templatesLoaded() {
        getComputers();
        showMenu();
        //getComputerBrands();
    }


})(jQuery, Backbone, app.dataservice);