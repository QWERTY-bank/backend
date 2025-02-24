﻿using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;

namespace Bank.Common.Api.DTOs
{
    public class ErrorResponse
    {
        [Required]
        public string Title { get; set; } = null!;
        [Required]
        public int Status { get; set; }
        [Required]
        public ImmutableDictionary<string, List<string>> Errors { get; set; } = null!;
    }
}