using System.ComponentModel.DataAnnotations;

namespace IncidentManagementWebApp.Interfaces
{
    public class IIncident
    {
        public int Id { get; set; }
        public string Description { get; set; }
        [EnumDataType(typeof(IncidentStatus))]
        public string Status { get; set; }

        [EnumDataType(typeof(IncidentPriority))]
        public string? Priority { get; set; }
        public string Username { get; set; }
        public string Location { get; set; }
    }

    public enum IncidentPriority
    {
        Hoog = 0,
        Gemiddeld = 1,
        Laag = 2
    }

    public enum IncidentStatus
    {
        Gemeld = 0,
        InBehandeling = 1,
        Afgehandeld = 2
    }
}
