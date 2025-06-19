using IncidentManagementWebApp.Enums;
using IncidentManagementWebApp.Interfaces;

namespace IncidentManagementWebApp.IncidentPropertyComparers
{
    public class IncidentStatusComparer : IComparer<IIncident>
    {
        public int Compare(IIncident? x, IIncident? y)
        {
            IncidentStatus aStatus = this.ConverStringToStatus(x.Status);
            IncidentStatus bStatus = this.ConverStringToStatus(y.Status);

            return aStatus > bStatus ? 1 : -1;
        }

        private IncidentStatus ConverStringToStatus(string status)
        {
            switch (status)
            {
                case "Gemeld":
                    return IncidentStatus.Gemeld;
                case "Geregistreerd":
                    return IncidentStatus.Geregistreerd;
                case "InBehandeling":
                    return IncidentStatus.InBehandeling;
                case "Afgehandeld":
                    return IncidentStatus.Afgehandeld;
                default:
                    return IncidentStatus.Gemeld;
            }
        }
    }
}
