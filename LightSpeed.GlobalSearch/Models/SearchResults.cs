using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LightSpeed.GlobalSearch.Models
{
    public class SearchResults
    {
        public List<ResultItem> Users { get; set; }
        public List<ResultItem> Groups { get; set; }

        public SearchResults()
        {
            Users = new List<ResultItem>();
            Groups = new List<ResultItem>();
        }
    }

    public class ResultItem
    {
        public int Id { get; set; }
        public string Value { get; set; }
    }
}