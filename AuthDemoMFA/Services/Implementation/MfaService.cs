using System;
using AuthDemoMFA.Services;
using OtpNet;

namespace AuthDemoMFA.Services.Implementation;

public class MfaService : IMfaService
{
    public string GenerateSecretKey()
    {
        var key = KeyGeneration.GenerateRandomKey(20);
        return Base32Encoding.ToString(key);
    }

    public string GenerateQrCodeUri(string secretKey, string username)
    {
        var issuer = "AuthDemo";
        var encodedIssuer = Uri.EscapeDataString(issuer);
        var encodedUsername = Uri.EscapeDataString(username);
        return $"otpauth://totp/{encodedIssuer}:{encodedUsername}?secret={secretKey}&issuer={encodedIssuer}";
    }

    public bool VerifyCode(string secretKey, string code)
    {
        if (string.IsNullOrWhiteSpace(secretKey) || string.IsNullOrWhiteSpace(code) || code.Length != 6)
            return false;

        try
        {
            var keyBytes = Base32Encoding.ToBytes(secretKey);
            var totp = new Totp(keyBytes);
            // Create a window that allows one step before and one after
            var window = new VerificationWindow(previous: 1, future: 1);
            return totp.VerifyTotp(code, out _, window);
        }
        catch
        {
            return false;
        }
    }
}