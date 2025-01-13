using System;
using System.ComponentModel.DataAnnotations;

namespace AuthDemoMFA.Models;

public class EnableMfaViewModel
{
    public string QrCodeUri { get; set; } = string.Empty;
    public string SecretKey { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Verification Code")]
    [StringLength(6, MinimumLength = 6, ErrorMessage = "The verification code must be 6 digits")]
    [RegularExpression("^[0-9]*$", ErrorMessage = "Code must contain only numbers")]
    public string VerificationCode { get; set; } = string.Empty;
    public string QrCodeBase64 { get; internal set; } = string.Empty;
}
