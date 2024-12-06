using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IventoryManagementTest.Models
{
    public class ForkLiftResponse
    {
        public string Name { get; set; }
        public string ModelNumber { get; set; }
        public DateTime ManufacturingDate { get; set; }
        public int Age { get; set; }
    }
}
