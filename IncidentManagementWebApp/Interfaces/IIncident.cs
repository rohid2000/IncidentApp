using System.ComponentModel.DataAnnotations;

namespace IncidentManagementWebApp.Interfaces
{
    public class IIncident
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public string? Priority { get; set; }
        public string Username { get; set; }
        public DateTime ReportDate { get; set; }
        public string Location { get; set; }
    }
}
