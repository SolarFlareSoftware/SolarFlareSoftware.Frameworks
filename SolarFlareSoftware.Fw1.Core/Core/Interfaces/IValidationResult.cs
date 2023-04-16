namespace SolarFlareSoftware.Fw1.Core.Interfaces
{
    public interface IValidationResult
    {
        Dictionary<string, string> ValidationErrors { get; }
        void AddError(string key, string errorMessage);
        bool IsValid { get; }
    }
}
