
angular.module('app.regional', [])
    .controller('RegionalSalesController', ['Customers', 'OrderDetails', 'scale', function (Customers, OrderDetails, scale) {
        this.selectedCountry = 'USA';

        this.startDate = new Date(1996, 0, 1);

        this.endDate = new Date(1998, 7, 1);

        this.customers = Customers.query();

        this.orderDetails = OrderDetails.query();

        this.customerNames = function() {
            return this.customers.filter(function(customer) {
                return customer.Country === this.selectedCountry;
            }.bind(this)).map(function(customer) {
                return customer.CompanyName;
            }).join(', ');
        }.bind(this);

        this.totalOrders = function() {
            return this.orderDetails.filter(function(order) {
                var orderDate = kendo.parseDate(order.orderDate);

                return order.country === this.selectedCountry && orderDate > this.startDate && orderDate < this.endDate;
            }.bind(this)).length;
        }.bind(this);

        this.totalCustomers = function() {
            return this.customers.filter(function(customer) {
                return customer.Country === this.selectedCountry;
            }.bind(this)).length;
        }.bind(this);

        this.mapLayers = [{
            style: {
                fill: {
                    color: "#1996E4"
                },
                stroke: {
                    color: "#FFFFFF"
                }
            },
            type: "shape",
            dataSource: {
                type: "geojson",
                transport: {
                    read: {
                        dataType: "json",
                        url: "./Content/countries-sales.geo.json"
                    }
                }
            }
        }];

        this.shapeCreated = function(e) {
            var color = scale(e.shape.dataItem.properties.sales).hex();
            e.shape.fill(color);
        };

        this.shapeMouseEnter = function(e) {
            e.shape.options.set("fill.color", "#0c669f");
        };

        this.shapeMouseLeave = function(e) {
            if (!e.shape.dataItem.properties.selected) {
                var sales = e.shape.dataItem.properties.sales;
                var color = scale(sales).hex();
                e.shape.options.set("fill.color", color);
                e.shape.options.set("stroke.color", "white");
            }
        };

        this.selectedShape = null;

        this.shapeClick = function(e) {
            if (this.selectedShape) {
                var sales = this.selectedShape.dataItem.properties.sales;
                var color = scale(sales).hex();
                this.selectedShape.options.set("fill.color", color);
                this.selectedShape.options.set("stroke.color", "white");
                this.selectedShape.dataItem.properties.selected = false;
            }

            e.shape.options.set("fill.color", "#0c669f");
            e.shape.options.set("stroke.color", "black");
            e.shape.dataItem.properties.selected = true;
            this.selectedShape = e.shape;
            this.selectedCountry = this.selectedShape.dataItem.properties.name;
        }.bind(this);
    }]);
