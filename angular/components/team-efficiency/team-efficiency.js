angular.module('app.team', [])
    .controller('TeamEfficiencyController', ['$q', 'EmployeeList', 'EmployeeSales', 'EmployeeTeamSales', 'EmployeeAverageSales', 'EmployeeQuarterSales',
    function ($q, EmployeeList, EmployeeSales, EmployeeTeamSales, EmployeeAverageSales, EmployeeQuarterSales) {
        this.startDate = new Date(1996, 0, 1);

        this.endDate = new Date(1998, 7, 1);

        this.currentEmployee = null;

        this.employeeTeamSales = EmployeeTeamSales.query();

        this.employeeAverageSales = EmployeeAverageSales.query();

        this.employeeQuarterSales = EmployeeQuarterSales.query();

        this.employeeList = EmployeeList.query();

        this.changeCurrentEmployee = function(employee) {
            this.currentEmployee = employee;

            this.currentEmployeeQuarterSales = this.employeeQuarterSales.filter(function(sale) {
                return sale.EmployeeID == employee.EmployeeID;
            })[0].Sales;

            this.currentEmployeeAverageSales = this.employeeAverageSales.filter(function(sale){
                return sale.EmployeeID == employee.EmployeeID;
            });

            var dataSource = new kendo.data.DataSource({
                data: this.currentEmployeeAverageSales,
                aggregate: [{
                    field: 'EmployeeSales',
                    aggregate: 'average'
                }]
            });

            dataSource.read();

            var aggregates = dataSource.aggregates();

            this.currentEmployeeAverageSalesNumber = aggregates.EmployeeSales ? aggregates.EmployeeSales.average : 0;
        };

        $q.all([this.employeeQuarterSales.$promise, this.employeeList.$promise, this.employeeAverageSales.$promise]).then(function() {
            this.changeCurrentEmployee(this.employeeList[0]);
        }.bind(this));
    }]);
