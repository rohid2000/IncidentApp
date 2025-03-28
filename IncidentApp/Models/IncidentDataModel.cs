using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IncidentApp.Models
{
    public class IncidentDataModel
    {
        public int id { get; set; }
        public string description { get; set; }
        public string status { get; set; }
        public string priority { get; set; }
        public int userId { get; set; }
    }
}
