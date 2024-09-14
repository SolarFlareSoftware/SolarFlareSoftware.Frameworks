//Copyright 2020-2024 Solar Flare Software, Inc. All Rights Reserved. Permission to use, copy, modify,
//and distribute this software and its documentation for educational, research, and not-for-profit purposes,
//without fee and without a signed licensing agreement is hereby prohibited. Contact Solar Flare Software, Inc.
//at 6834 Lincoln Way W, Saint Thomas, PA 17252 or at sales@solarflaresoftware.com for licensing opportunities.
using SolarFlareSoftware.Fw1.Core.Interfaces;
using System;
using System.Diagnostics.CodeAnalysis;

namespace SolarFlareSoftware.Fw1.Core.Events
{
    public class RepositoryPreSaveEventArgs<T> : EventArgs where T : IBaseModel
    {
        public T Model { get; }
        public RepositoryPreSaveEventArgs(T model)
        {
            Model = model;
        }
        public bool CancelSave { get; set; } = false;
    }
}
