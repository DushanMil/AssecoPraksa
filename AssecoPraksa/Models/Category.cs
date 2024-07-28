﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AssecoPraksa.Models
{
    public class Category
    {
        [Required]
        public string Code { get; set; }
        [JsonPropertyName("parent-code")]
        public string ParentCode { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
