/// <reference path="../breeze.debug.js" /> 
/// <reference path="../breeze.stickit.js" /> 

(function ($, Backbone, dataservice) {

    // Get templates
    var computerContent;
    var computerTemplate;
    var computerBrandMenuContent;
    var computerBrandMenuTemplate;

    $(document).ready(function () {
        documentReady();
    });

    function documentReady() {
        computerContent = $(".computers-container");
        computerBrandMenuContent = $(".computer-brand-container");

        loadTemplates();
    }

    var loadTemplates = function () {
        $.get('/Content/templates.tmpl.html', function (templates) {
            computerTemplate = $(templates).find('script#computerTemplate').html();
            computerBrandMenuTemplate = $(templates).find('script#computerBrandMenuTemplate').html();
            templatesLoaded();
        });
    }

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

    var ComputerView = Backbone.View.extend({
        bindings: {
            '#name': 'name',
            '#brand-name': {
                observe: 'computerBrand',
                onGet: function (value, attrNames) {
                    // onGet called after title *or* author model attributes change.
                    return value.get('name');
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
        computerBrandMenuContent.empty();

        dataservice.getComputerBrands()
            .then(gotComputerBrands);

        function gotComputerBrands(computerBrands) {
            computerBrands.forEach(
                function (computerBrand) {
                    var view = new computerBrandMenuView({ model: computerBrand });
                    computerBrandMenuContent.append(view.render().el);
                }
            );

            var mapped = $.map(computerBrands, function (value) {
                return { value: value.attributes.name };
            });

            $("#searchByBrand").autocomplete({
                source: mapped
            });
        }
    };

    var getComputers = function (computerBrandId) {
        computerContent.empty();

        dataservice.getComputers(computerBrandId)
            .then(gotComputers);

        function gotComputers(computers) {
            computers.forEach(
                function (computer) {
                    var view = new ComputerView({ model: computer });
                    computerContent.append(view.render().el);
                });
        }
    };
    
    function templatesLoaded() {
        getComputers();
        getComputerBrands();
    }


})(jQuery, Backbone, app.dataservice);