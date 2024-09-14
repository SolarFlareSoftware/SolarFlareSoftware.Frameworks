//Copyright 2020-2024 Solar Flare Software, Inc. All Rights Reserved. Permission to use, copy, modify,
//and distribute this software and its documentation for educational, research, and not-for-profit purposes,
//without fee and without a signed licensing agreement is hereby prohibited. Contact Solar Flare Software, Inc.
//at 6834 Lincoln Way W, Saint Thomas, PA 17252 or at sales@solarflaresoftware.com for licensing opportunities.
using SolarFlareSoftware.Fw1.Core.Interfaces;
using System.Collections.Generic;

namespace SolarFlareSoftware.Fw1.Core.Validation
{
    /// <summary>
    /// This class implements IValidationResult and is intended to be used to pass errors encountered beyond the Controller level 
    /// back to the calling view in order to allow the errors to be displayed on view.
    /// </summary>
    public class ValidationResult : IValidationResult
    {
        public Dictionary<string, string> ValidationErrors { get; private set; }

        public ValidationResult()
        {
            ValidationErrors = new Dictionary<string, string>();
        }

        public ValidationResult(Dictionary<string, string> modelState)
        {
            ValidationErrors = modelState;
        }

        public bool IsValid => ValidationErrors.Count == 0;

        public void AddError(string key, string errorMessage)
        {
            ValidationErrors.Add(key, errorMessage);
        }
    }
}
