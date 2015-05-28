using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Dashboard.DataService.Models
{
    public class Employee
    {
        public long SysNo { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }

        public int Age { get; set; }
        
        public DateTime CreateTime { get; set; }

        public DateTime UpdateTime { get; set; }
    }
}
