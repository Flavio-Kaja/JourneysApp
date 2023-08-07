using System.Reflection;

namespace JourneyService.Domain
{

    public static class Permissions
    {
        public const string CanCreateJourney = "CanCreateJourney";
        public const string CanReadJourney = "CanReadJourney";
        public const string CanUpdateJourney = "CanUpdateJourney";
        public const string CanDeleteJourney = "CanDeleteJourney";
        public const string CanFilterJourneys = "CanFilterJourneys";


        public const string CanCreateTransportation = "CanCreateTransportation";
        public const string CanReadTransportation = "CanReadTransportation";
        public const string CanUpdateTransportation = "CanUpdateTransportation";
        public const string CanDeleteTransportation = "CanDeleteTransportation";


        public const string CanViewMonthlyRouteDistance = "CanViewMonthlyRouteDistance";

        public static List<string> GetUserPermissions()
        {
            return new List<string>
        {
            CanCreateJourney,CanReadJourney,CanUpdateJourney,CanDeleteJourney
        };
        }
        public static List<string> List()
        {
            return typeof(Permissions)
                .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
                .Where(fi => fi.IsLiteral && !fi.IsInitOnly && fi.FieldType == typeof(string))
                .Select(x => (string)x.GetRawConstantValue())
                .ToList();
        }
    }

}
