﻿using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace SatelliteSite.Models
{
    public class SetUploadModel
    {
        [Required]
        public string Language { get; set; }

        [Required]
        public IFormFileCollection Files { get; set; }
    }
}
