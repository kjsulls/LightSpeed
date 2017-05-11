using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using LightSpeed.GlobalSearch.Models;
using Newtonsoft.Json.Linq;

namespace LightSpeed.GlobalSearch.Controllers
{
    public class GlobalSearchController : Controller
    {
        private static JObject _content = new JObject();

        public GlobalSearchController()
        {
            if (_content.HasValues)
                return;

            var path = AppDomain.CurrentDomain.BaseDirectory + @"Scripts\content.js";
            using (var reader = new StreamReader(path))
            {
                var json = reader.ReadToEnd();
                _content = JObject.Parse(json);
            }
        }

        [HttpGet]
        [ChildActionOnly]
        public ActionResult SearchWidget()
        {
            return PartialView("_Search");
        }

        [HttpGet]
        public ActionResult Search(string text)
        {
            text = text.ToLower();

            var results = new SearchResults();
            if (text.Length > 0)
            {
                results = GetResults(text);
            }
            
            return PartialView("_SearchResults", results);
        }

        private SearchResults GetResults(string text)
        {
            var results = new SearchResults();
            var root = _content["root"];
            var counter = 1;

           foreach (var c in root.Children())
            {
                var type = (string)c["type"];
                var name = (string)c["name"];

                if (type.ToLower() == "user" && name.ToLower().Contains(text))
                {
                    results.Users.Add(new ResultItem { Id = counter, Value = name });
                }

                if (type.ToLower() == "group" && name.ToLower().Contains(text))
                {
                    results.Groups.Add(new ResultItem { Id = counter, Value = name });
                }
                counter++;
            }

            return results;
        }
    }
}