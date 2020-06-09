﻿using Microsoft.AspNetCore.Mvc.DataTables;

namespace SatelliteSite.Models
{
    [DtWrapUrl("/submission/{Id}")]
    public class SubmissionListModel
    {
        [DtDisplay(0, "Uid", Sortable = true, DefaultAscending = "asc")]
        public string Uid { get; set; }

        [DtDisplay(1, "ID", Sortable = true, DefaultAscending = "asc")]
        public string Id { get; set; }

        [DtDisplay(2, "lang", Sortable = true)]
        public string Language { get; set; }
    }
}