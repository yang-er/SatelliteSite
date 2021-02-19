﻿using System.Text.Json.Serialization;

namespace Plag.Backend.Models
{
    public class Comparison
    {
        [JsonPropertyName("reportid")]
        public string Id { get; set; }

        [JsonPropertyName("submitid")]
        public int SubmissionIdAnother { get; set; }

        [JsonPropertyName("pending")]
        public bool Pending { get; set; }

        [JsonPropertyName("tokens_matched")]
        public int TokensMatched { get; set; }

        [JsonPropertyName("biggest_match")]
        public int BiggestMatch { get; set; }

        [JsonPropertyName("percent")]
        public double Percent { get; set; }

        [JsonPropertyName("percent_self")]
        public double PercentSelf { get; set; }

        [JsonPropertyName("percent_another")]
        public double PercentIt { get; set; }

        public Comparison() { }

        public Comparison(string id) { Id = id; }

        public Comparison(System.Guid id) { Id = id.ToString(); }
    }
}
