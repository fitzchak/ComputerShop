/// <reference path="../breeze.debug.js" /> 
/// <reference path="../breeze.stickit.js" /> 

(function ($, Backbone, dataservice) {

    // Get templates
    //var content = $("#content");
    var carTemplateSource = $("#car-template").html();
    var optionTemplateSource = $("#option-template").html();

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

    // Car BB View (extended by stickit)
    var CarView = Backbone.View.extend({
        bindings: {
            '#make-input': 'make',
            '#model-input': 'model',
            '#make-desc': 'make',
            '#model-desc': 'model'
        },
        events: {
            "click #options": "showOptions"
        },
        render: function () {
            this.$el.html(carTemplateSource);
            this.stickit();
            return this;
        },
        renderOptions: function () {
            var optionsHost = $("#optionsList", this.$el).empty();
            var options = this.model.get("options");
            if (options.length) {
                options.forEach(
                    function (option) {
                        var view = new OptionView({ model: option });
                        optionsHost.append(view.render().el);
                    });
                optionsHost.removeClass("hidden");
            } else {
                optionsHost.addClass("hidden");
            }
        },
        // A toggle to hide/show options
        // will load options from db if not already loaded
        showOptions: function () {
            var self = this;
            var optionsHost = $("#optionsList", self.$el);
            if (optionsHost.hasClass("hidden")) {
                dataservice.loadOptionsIfNotLoaded(self.model)
                    .then(function () {
                        self.renderOptions();
                    });
            } else {
                optionsHost.addClass("hidden");
            }
        }
    });

    // Option BB View (extended by stickit)
    var OptionView = Backbone.View.extend({
        bindings: {
            '#name-input': 'name',
            '#name-desc': 'name'
        },
        render: function () {
            this.$el.html(optionTemplateSource);
            this.stickit();
            return this;
        }
    });

    var enableSave = function () {
        var saveElements = $(".save");
        saveElements.removeClass("hidden");
        // only add the click handler once
        if (enableSave.initialized) { return; }
        saveElements.click(function () {
            dataservice.saveChanges();
        });
        enableSave.initialized = true;
    };

    function templatesLoaded() {
        getComputers();
        getComputerBrands();
    }


})(jQuery, Backbone, app.dataservice);