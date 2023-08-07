namespace JourneyService.Exceptions
{
    [Serializable]
    public class LocationNotFoundException : Exception
    {
        public LocationNotFoundException(string location)
            : base($"No location with name `{location}` found.")
        { }
    }
}
