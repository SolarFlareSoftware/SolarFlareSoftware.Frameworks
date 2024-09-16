﻿/*
 * Copyright (C) 2023 Solar Flare Software, Inc.
 * 
 * Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:
 *
 * Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
 * Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.
 * Neither the name of the copyright holder nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission.
 *
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A 
 * PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT 
 * LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR 
 * TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.[8]
 * 
 */
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
