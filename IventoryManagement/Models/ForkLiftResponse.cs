using System;

namespace IventoryManagement.Models
{
    /// <summary>
    /// ForkliftRespone Model
    /// </summary>
    public class ForkLiftResponse
    {
        public string Name { get; set; }
        public string ModelNumber { get; set; }
        public DateTime ManufacturingDate { get; set; }
        public int Age { get; set; }
    }
}
