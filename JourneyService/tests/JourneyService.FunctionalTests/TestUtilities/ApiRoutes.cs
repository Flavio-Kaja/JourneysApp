namespace JourneyService.FunctionalTests.TestUtilities;
public class ApiRoutes
{
    public const string Base = "api";
    public const string Health = Base + "/health";

    // new api route marker - do not delete

    public static class TransportationTypes
    {
        public static string GetList => $"{Base}/transportationTypes";
        public static string GetRecord(Guid id) => $"{Base}/transportationTypes/{id}";
        public static string Delete(Guid id) => $"{Base}/transportationTypes/{id}";
        public static string Put(Guid id) => $"{Base}/transportationTypes/{id}";
        public static string Create => $"{Base}/transportationTypes";
        public static string CreateBatch => $"{Base}/transportationTypes/batch";
    }

    public static class Locations
    {
        public static string GetList => $"{Base}/locations";
        public static string GetRecord(Guid id) => $"{Base}/locations/{id}";
        public static string Delete(Guid id) => $"{Base}/locations/{id}";
        public static string Put(Guid id) => $"{Base}/locations/{id}";
        public static string Create => $"{Base}/locations";
        public static string CreateBatch => $"{Base}/locations/batch";
    }

    public static class Journeys
    {
        public static string GetList => $"{Base}/journeys";
        public static string GetRecord(Guid id) => $"{Base}/journeys/{id}";
        public static string Delete(Guid id) => $"{Base}/journeys/{id}";
        public static string Put(Guid id) => $"{Base}/journeys/{id}";
        public static string Create => $"{Base}/journeys";
        public static string CreateBatch => $"{Base}/journeys/batch";
    }
}
