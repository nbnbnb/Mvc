// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Mvc;

namespace BasicWebSite.Controllers
{
    [ValidateAntiForgeryToken]
    public class ValidateAntiForgeryTokenOnControllerController : Controller
    {
        [IgnoreAntiforgeryToken]
        [HttpGet]
        public IActionResult GetAction()
        {
            return Content("GetAction");
        }

        [HttpPost]
        public IActionResult PostAction()
        {
            return Content("PostAction");
        }
    }
}
