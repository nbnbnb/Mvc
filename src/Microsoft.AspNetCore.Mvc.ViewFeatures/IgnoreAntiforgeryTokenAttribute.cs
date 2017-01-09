// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Microsoft.AspNetCore.Mvc
{
    /// <summary>
    /// An filter that skips antiforgery token validation.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class IgnoreAntiforgeryTokenAttribute : Attribute, IAntiforgeryPolicy, IOrderedFilter
    {
        // NOTE: The default order of this attribute must match with that of ValidateAntiForgeryToken and
        // AutoValidateAntiForgeryToken attributes. This is so that in the scenarios where these attributes are used
        // at a broader scope, the most local ignore attribute (if any) should be considered.

        /// <inheritdoc />
        public int Order { get; set; } = 1000;
    }
}
