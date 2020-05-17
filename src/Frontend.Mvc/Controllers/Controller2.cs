﻿using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Microsoft.AspNetCore.Mvc
{
    public abstract class Controller2 : Controller
    {
        [TempData]
        public string? StatusMessage { get; set; }

        public bool InAjax { get; private set; }

        public bool IsWindowAjax { get; private set; }


        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            string? handlekey = null;
            if (context.HttpContext.Request.Query.TryGetValue(nameof(handlekey), out var qs))
                handlekey = qs.FirstOrDefault();
            else if (context.HttpContext.Request.HasFormContentType
                    && context.HttpContext.Request.Form.TryGetValue(nameof(handlekey), out qs))
                handlekey = qs.FirstOrDefault();

            IsWindowAjax = handlekey != null;

            if (IsWindowAjax || (HttpContext.Request.Headers.TryGetValue("X-Requested-With", out var val)
                    && val.FirstOrDefault() == "XMLHttpRequest"))
                InAjax = true;

            if (InAjax)
            {
                ViewData["InAjax"] = true;
            }
            else
            {
                ViewData["RefreshUrl"] = HttpContext.Request.Path.Value +
                    HttpContext.Request.QueryString.Value.Replace("&amp;", "&");
            }

            if (IsWindowAjax)
            {
                ViewData["HandleKey"] = handlekey;
                ViewData["HandleKey2"] = System.Guid.NewGuid().ToString().Substring(0, 6);
            }
        }


        public override void OnActionExecuted(ActionExecutedContext context)
        {
            base.OnActionExecuted(context);
            if (context.Result is RedirectToActionResult rtas)
                context.Result = new RedirectToActionWithAjaxSupportResult(rtas, InAjax);

            if (context.Result is ForbidResult && InAjax)
            {
                Response.StatusCode = 403;
                if (IsWindowAjax)
                    context.Result = Message("Access denined", "You do not have access to this resource.", MessageType.Danger);
                else
                    context.Result = new EmptyResult();
            }
        }

        [NonAction]
        public ShowMessageResult Message(
            string title, string message,
            MessageType? type = null)
        {
            return new ShowMessageResult
            {
                ViewData = ViewData,
                TempData = TempData,
                Title = title,
                Content = message,
                Type = type,
            };
        }


        [NonAction]
        public AskPostResult AskPost(
            string title, string message,
            string area, string ctrl, string act,
            object routeValues,
            MessageType? type = null)
        {
            if (!(routeValues is Dictionary<string, string> rvd))
            {
                var vtype = routeValues.GetType();
                if ((!vtype.FullName?.StartsWith("<>f__AnonymousType")) ?? true)
                    throw new System.ArgumentException(nameof(routeValues));

                rvd = vtype.GetProperties().ToDictionary(
                    keySelector: p => p.Name,
                    elementSelector: p => $"{p.GetValue(routeValues)}");
            }

            return new AskPostResult
            {
                ViewData = ViewData,
                TempData = TempData,
                Title = title,
                Content = message,
                Type = type,
                AreaName = area,
                ControllerName = ctrl,
                ActionName = act,
                RouteValues = rvd,
            };
        }


        #region public ShowWindowResult Window(viewName, model)...

        [NonAction]
        public ShowWindowResult Window()
        {
            return Window(null, null);
        }

        [NonAction]
        public ShowWindowResult Window(object model)
        {
            return Window(null, model);
        }

        [NonAction]
        public ShowWindowResult Window(string viewName)
        {
            return Window(viewName, null);
        }

        [NonAction]
        public ShowWindowResult Window(string? viewName, object? model)
        {
            ViewData.Model = model;

            return new ShowWindowResult
            {
                ViewName = viewName,
                ViewData = ViewData,
                TempData = TempData,
            };
        }

        #endregion


        [NonAction]
        public IActionResult StatusCodePage(int? code)
        {
            ViewBag.RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
            var statusCode = code ?? Response.StatusCode;
            Response.StatusCode = statusCode;
            ViewBag.StatusCode = statusCode;
            var statusName = Regex.Replace(((HttpStatusCode)Response.StatusCode).ToString(), "([a-z])([A-Z])", "$1 $2");
            ViewBag.StatusName = statusName;
            
            if (InAjax)
                return Content(
                    $"Sorry, an error has occured: {statusName}.\n" +
                    "Please contact a staff member for assistance.", "text/plain");
            else
                return View("Error");
        }
    }
}
