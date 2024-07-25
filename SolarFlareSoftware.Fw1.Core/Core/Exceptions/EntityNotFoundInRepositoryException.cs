//Copyright 2020-2024 Solar Flare Software, Inc. All Rights Reserved. Permission to use, copy, modify,
//and distribute this software and its documentation for educational, research, and not-for-profit purposes,
//without fee and without a signed licensing agreement is hereby prohibited. Contact Solar Flare Software, Inc.
//at 6834 Lincoln Way W, Saint Thomas, PA 17252 or at sales@solarflaresoftware.com for licensing opportunities.
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
