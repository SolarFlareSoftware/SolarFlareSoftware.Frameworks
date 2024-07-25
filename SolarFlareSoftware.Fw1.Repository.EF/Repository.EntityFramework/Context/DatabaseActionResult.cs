//Copyright 2020-2024 Solar Flare Software, Inc. All Rights Reserved. Permission to use, copy, modify,
//and distribute this software and its documentation for educational, research, and not-for-profit purposes,
//without fee and without a signed licensing agreement is hereby prohibited. Contact Solar Flare Software, Inc.
//at 6834 Lincoln Way W, Saint Thomas, PA 17252 or at sales@solarflaresoftware.com for licensing opportunities.
using SolarFlareSoftware.Fw1.Core.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolarFlareSoftware.Fw1.Repository.EF.Context
{
    public class DatabaseActionResult : IDatabaseActionResult
    {
        public bool Succeeded { get; set; }
        public int RecordsAffected { get; set; }
        public string? Message { get; set; }
        public Exception? Exception { get; set; } = null;
    }
}
