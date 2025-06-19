using IncidentManagementWebApp.Enums;
using IncidentManagementWebApp.Interfaces;

namespace IncidentManagementWebApp.IncidentPropertyComparers
{
    public class IncidentPriorityComparer : IComparer<IIncident>
    {
        public int Compare(IIncident? x, IIncident? y)
        {
            IncidentPriority aPrio = ConverStringToPriority(x.Priority);
            IncidentPriority bPrio = ConverStringToPriority(y.Priority);

            return aPrio < bPrio ? 1 : -1;
        }

        private IncidentPriority ConverStringToPriority(string priority)
        {
            switch (priority)
            {
                case "Hoog":
                    return IncidentPriority.Hoog;
                case "Gemiddeld":
                    return IncidentPriority.Gemiddeld;
                case "Laag":
                    return IncidentPriority.Laag;
                default:
                    return IncidentPriority.Laag;
            }
        }
    }
}
