var app = angular.module('app', ['ngNewRouter', 'ngResource', 'app.regional', 'kendo.directives']).controller('AppController', ['$router', AppController]);

app.factory('Customers', ['$resource', function($resource) {
    return $resource('./Content/customers.json');
}])
.factory('CountryCustomers', ['$resource', function($resource) {
    return $resource('./Content/country-customers.json');
}])
.factory('scale', function() {
    return chroma.scale(["#ade1fb", "#097dc6"]).domain([1, 100]);
})
.factory('OrderDetails', ['$resource', function($resource) {
    return $resource('./Content/order-details.json');
}]);

AppController.$routeConfig = [
    { path: '/', redirectTo: '/regional-sales' },
    { path: '/regional-sales', component: 'regionalSales' }
];

function AppController ($router) {

}
