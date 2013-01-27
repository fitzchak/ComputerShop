/// <reference path="~/scripts/breeze.debug.js" />
/// <reference path="~/scripts/backbone.stickit.js" />
(function ($, Backbone, dataservice) {

    // Get templates
    var menuContent;
    var menuTemplate;
    var content;

    var computerEditTemplate;
    var nameDescriptionEditTemplate;

    $(document).ready(function () {
        documentReady();
    });

    function documentReady() {
        content = $(".main-content");
        menuContent = $("#menu-wrapper");
        loadTemplates();
    }

    var loadTemplates = function () {
        $.get('/Content/templates.tmpl.html', function (templates) {
            computerEditTemplate = $(templates).find('script#computerEditTemplate').html();
            nameDescriptionEditTemplate = $(templates).find('script#nameDescriptionEditTemplate').html();
            menuTemplate = $(templates).find('script#adminMenuTemplate').html();

            templatesLoaded();
        });
    };

    var menuView = Backbone.View.extend({
        bindings: {
            '#items': {
                observe: 'items',
                updateMethod: 'html',
                onGet: function (items) {

                    var result = '';

                    items.forEach(function (item) {
                        result += '<li><a href="#" accesskey="1" class="' + item.name + '">' + item.name + '</a></li>';
                    });

                    return result;
                }
            }
        },
        events: {
            'click #items': 'clicked'
        },
        render: function () {
            this.$el.html(menuTemplate);
            this.stickit();

            return this;
        },
        clicked: function (data) {
            var passedName = data.toElement.className;

            var items = this.model.get('items');
            var foundFunction;

            items.forEach(function (item) {
                if (item.name == passedName) {
                    foundFunction = item.func;
                }
            });

            foundFunction();
        }
    });

    var nameDescriptionEditView = Backbone.View.extend({
        bindings: {
            '#id': 'id',
            '#name': 'name',
            '#description': 'description'
        },
        events: {
            'click #save': 'save',
            'click #cancel': 'cancel'
        },
        render: function () {
            this.$el.html(nameDescriptionEditTemplate);

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
            'click #save': 'save',
            'click #cancel': 'cancel',
            'click #delete': 'deleteAction'
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
        },
        deleteAction: function () {
            this.model.entityAspect.setDeleted();
            dataservice.saveChanges([this.model]);

            getComputers();
        }
    });

    var getProcessors = function () {
        var processors = dataservice.getProcessorsFromCache();
        showNameDescriptionItemViews(processors);
    };

    var getComputerBrands = function () {
        var brands = dataservice.getComputerBrandsFromCache();
        showNameDescriptionItemViews(brands);
    };

    function showNameDescriptionItemViews(items) {
        content.empty();
        items.forEach(
                function (computerBrand) {
                    var view = new nameDescriptionEditView({ model: computerBrand });
                    content.append(view.render().el);
                }
            );
    }

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

    var showMenu = function () {

        var MenuItems = Backbone.Model.extend({

        });

        var menuItems = new MenuItems(
            {
                items: [
                    { name: "Computers", func: getComputers },
                    { name: "Brands", func: getComputerBrands },
                    { name: "Processors", func: getProcessors }
                ]
            });

        menuContent.empty();

        var view = new menuView({ model: menuItems });
        menuContent.append(view.render().el);
    };

    function templatesLoaded() {
        getComputers();
        showMenu();
    }
})(jQuery, Backbone, app.dataservice);