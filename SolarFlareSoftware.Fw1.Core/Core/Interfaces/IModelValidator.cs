namespace SolarFlareSoftware.Fw1.Core.Interfaces
{
    public interface IModelValidator<T> where T : IBaseModel
    {
        IValidationResult Validate(T model);
    }
}
