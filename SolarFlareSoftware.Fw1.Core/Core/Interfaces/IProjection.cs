using System;
using System.Linq.Expressions;

namespace SolarFlareSoftware.Fw1.Core.Interfaces
{
    public interface IProjection 
    {
        public Expression<Func<IBaseModel, IProjectedModel>> Projection { get; set; }
    }
}
