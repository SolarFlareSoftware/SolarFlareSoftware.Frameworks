namespace SolarFlareSoftware.Fw1.Core.Interfaces
{
    public interface IModelValidator<T> where T : IBaseModel
    {
        abstract void SetDatabaseActionContext(int actionContext);
        IValidationResult Validate(T model);
    }
}
