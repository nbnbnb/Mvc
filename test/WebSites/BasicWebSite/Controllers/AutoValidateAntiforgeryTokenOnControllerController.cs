// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Mvc;

namespace BasicWebSite.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class AutoValidateAntiforgeryTokenOnControllerController : Controller
    {
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

        [IgnoreAntiforgeryToken]
        [HttpPost]
        public IActionResult IgnoredPostAction()
        {
            return Content("IgnoredPostAction");
        }
    }
}
