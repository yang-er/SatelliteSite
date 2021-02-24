﻿using Microsoft.AspNetCore.Mvc.DataTables;
using Plag.Backend.Models;

namespace SatelliteSite.PlagModule.Models
{
    [DtWrapUrl("/dashboard/plagiarism/{SetId}/reports/{Id}")]
    public class SubmissionListModel
    {
        public SubmissionListModel(string setid, Comparison comparison)
        {
            Id = comparison.Id;
            SubmissionIdAnother = comparison.SubmissionIdAnother;
            SubmissionAnother = comparison.SubmissionNameAnother;
            Pending = !(comparison.Finished ?? false);
            SetId = setid;
            ExclusiveCategory = comparison.ExclusiveCategory;

            if (!Pending)
            {
                BiggestMatch = comparison.BiggestMatch;
                TokensMatched = comparison.TokensMatched;
                Percent = comparison.Percent;
                PercentIt = comparison.PercentIt;
                PercentSelf = comparison.PercentSelf;
            }
        }

        [DtIgnore]
        public string SetId { get; }

        [DtIcon(9, "fas fa-external-link-alt")]
        [DtWrapUrl("/plagiarism-reports/{Id}")]
        public string Id { get; }

        [DtIcon(8, "fa fa-file-code")]
        [DtWrapUrl("/dashboard/plagiarism/{SetId}/submissions/{SubmissionIdAnother}/source-code")]
        public int SubmissionIdAnother { get; }

        [DtDisplay(0, "excl.", Searchable = true, Sortable = true)]
        public int ExclusiveCategory { get; }

        [DtDisplay(1, "SID", "s{SubmissionIdAnother}: {SubmissionAnother}", Searchable = true, Sortable = true)]
        public string SubmissionAnother { get; }

        [DtCellCss(Class = "text-variant")]
        [DtBoolSelect("queued", "finished")]
        [DtDisplay(2, "status", Sortable = true)]
        public bool Pending { get; }

        [DtCoalesce("N/A")]
        [DtDisplay(3, "tot.", Sortable = true)]
        public int? TokensMatched { get; }

        [DtCoalesce("N/A")]
        [DtDisplay(4, "big.", Sortable = true)]
        public int? BiggestMatch { get; }

        [DtCoalesce("N/A")]
        [DtDisplay(5, "percent", "{Percent:F2}%", DefaultAscending = "desc", Sortable = true)]
        public double? Percent { get; }

        [DtCoalesce("N/A")]
        [DtDisplay(6, "this", "{PercentSelf:F2}%", Sortable = true)]
        public double? PercentSelf { get; }

        [DtCoalesce("N/A")]
        [DtDisplay(7, "that", "{PercentIt:F2}%", Sortable = true)]
        public double? PercentIt { get; }
    }
}
