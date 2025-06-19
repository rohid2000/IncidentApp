using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IncidentApp.Models
{
    public class IncidentDataModel
    {
        public int Id { get; set; }
        public required string Description { get; set; }
        public string Status { get; set; } = "Gemeld";
        public string? Priority { get; set; } = null;
        public int? UserId { get; set; }
        public DateTime ReportDate { get; set; }
        public string Location { get; set; }
    }
}
