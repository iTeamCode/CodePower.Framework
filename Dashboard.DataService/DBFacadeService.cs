using Dashboard.DataService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.DataService
{
    public static class DBFacadeService
    {
        public static List<Employee> GetAllEmployees()
        {
            var list = new List<Employee>();
            using (var dbContext = new DashboardDBContext())
            {
                list = dbContext.Employees.SqlQuery("SELECT * FROM dbo.Employee").ToList();
                dbContext.Dispose();
            }
            return list;
        }

        public static Employee CreateEmployee(Employee employee)
        {
            Employee tempEmp; 
            using (var dbContext = new DashboardDBContext())
            {
                dbContext.Employees.Add(new Employee()
                {
                    Code = employee.Code,
                    Name = employee.Name,
                    Age = employee.Age,
                    Type = employee.Type,
                    CreateTime = DateTime.Now,
                    UpdateTime = DateTime.Now
                });
                dbContext.SaveChanges();

                var list = dbContext.Employees.SqlQuery("SELECT TOP 1 * FROM dbo.Employee ORDER BY SysNo DESC").ToList();
                tempEmp = list.Count == 1 ? list[0] : null;
            }
            return tempEmp;
        }

        public static Employee UpdateEmployee(Employee employee)
        {
            Employee tempEmp;
            using (var dbContext = new DashboardDBContext())
            {
                var entityList = (
                    from emloyee in dbContext.Employees
                    where emloyee.SysNo == employee.SysNo
                    select emloyee
                ).ToList();

                tempEmp = entityList.Count == 1 ? entityList[0] : null;

                tempEmp.Code = employee.Code;
                tempEmp.Name = employee.Name;
                tempEmp.Age = employee.Age;
                tempEmp.Type = employee.Type;
                tempEmp.UpdateTime = DateTime.Now;
                
                dbContext.SaveChanges();
            }
            return tempEmp;
        }

        public static int DeleteEmployee(int sysNo)
        {
            int count = 0;
            using (var dbContext = new DashboardDBContext())
            {
                var entityList = (
                    from emloyee in dbContext.Employees
                    where emloyee.SysNo == sysNo
                    select emloyee
                ).ToList();

                var emloyeeEntity = entityList.Count == 1 ? entityList[0] : null;

                dbContext.Employees.Remove(emloyeeEntity);

                count = dbContext.SaveChanges();
            }
            return count;
        }
    }
}
