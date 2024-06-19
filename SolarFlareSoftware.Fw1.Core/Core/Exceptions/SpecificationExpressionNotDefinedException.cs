using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolarFlareSoftware.Fw1.Core.Core.Exceptions
{
    public class SpecificationExpressionNotDefinedException: Exception
    {
        public SpecificationExpressionNotDefinedException(string specificationType)
            : base(string.Format("No Expression was defined in the '{0}' specification being processed.", specificationType))
        {

        }
    }
}
