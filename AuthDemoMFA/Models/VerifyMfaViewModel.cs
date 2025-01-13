using System;
using System.ComponentModel.DataAnnotations;

namespace AuthDemoMFA.Models;

public class VerifyMfaViewModel
{
    [Required]
    [Display(Name = "Authentication Code")]
    [StringLength(6, MinimumLength = 6, ErrorMessage = "The authentication code must be 6 digits")]
    [RegularExpression("^[0-9]*$", ErrorMessage = "Code must contain only numbers")]
    public string Code { get; set; } = string.Empty;
}
