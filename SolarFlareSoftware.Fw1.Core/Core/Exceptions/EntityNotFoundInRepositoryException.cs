using System;

namespace SolarFlareSoftware.Fw1.Core
{
    public class EntityNotFoundInRepositoryException : Exception
    {
        public EntityNotFoundInRepositoryException(string entityType) 
            : base(string.Format("The {0} object could not be found in the database.", entityType))
        {

        }
    }
}
