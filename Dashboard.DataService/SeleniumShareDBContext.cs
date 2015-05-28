using Dashboard.DataService.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.DataService
{
    public class DashboardDBContext : DbContext
    {
        public DbSet<Employee> Employees { get; set; }

        public DashboardDBContext()
            : base("Name=DashboardConnection")
        {
            Database.SetInitializer<DashboardDBContext>(null);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //add Mapping.
            modelBuilder.Configurations.Add(new EmployeeMap());
            base.OnModelCreating(modelBuilder);
        }
    }
}
