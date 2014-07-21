using KendoUI.Northwind.Dashboard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace KendoUI.Northwind.Dashboard.Controllers
{
    public class TeamEfficiencyController : ApiController
    {
        //
        // GET: /TeamEfficiency/
        public List<EmployeeViewModel> GetEmployeesList()
        {
            var employees = new NorthwindEntities().Employees.Select(e => new EmployeeViewModel
            {
                EmployeeID = e.EmployeeID,
                FirstName = e.FirstName,
                LastName = e.LastName,
                EmployeeName = e.FirstName + " " + e.LastName,
                Notes = e.Notes,
                Title = e.Title,
                HomePhone = e.HomePhone
            }).OrderBy(e => e.FirstName);

            return employees.ToList();
        }

    }
}
