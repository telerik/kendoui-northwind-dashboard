
angular.module('app.regional', [])
    .controller('RegionalSalesController', ['$q', 'Customers', 'OrderDetails', 'scale', function ($q, Customers, OrderDetails, scale) {
        this.selectedCountry = 'USA';

        this.startDate = new Date(1996, 0, 1);

        this.endDate = new Date(1998, 7, 1);

        this.customers = Customers.query();

        this.marketDataSource = new kendo.data.DataSource();

        this.orderDetails = OrderDetails.query();

        this.refresh = function() {
            this.currentCustomers = this.customersForCountry();
            this.currentOrders = this.ordersForCountry();
            this.marketDataSource.data(this.marketShare());
        };

        $q.all([this.customers.$promise, this.orderDetails.$promise]).then(this.refresh.bind(this));

        this.customersForCountry = function() {
            return this.customers.filter(function(customer) {
                return customer.Country === this.selectedCountry;
            }.bind(this)).map(function(customer) {
                return customer.CompanyName;
            });
        };

        this.ordersForCountry = function() {
            return this.orderDetails.filter(function(order) {
                var orderDate = kendo.parseDate(order.orderDate);

                return order.country === this.selectedCountry && orderDate > this.startDate && orderDate < this.endDate;
            }.bind(this));
        };

        this.marketShare = function() {
            var sum = this.currentOrders.reduce(function(total, order) {
                return total + order.price;
            }, 0);
            return [
                { country: "All", price: 854648.019191742 },
                { country: this.selectedCountry, price: sum  }
            ];
        };

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
            this.refresh();
        }.bind(this);
    }]);
