using System;

namespace AuthDemoMFA.Services;

public interface IMfaService
{
    string GenerateSecretKey();
    string GenerateQrCodeUri(string secretKey, string username);
    bool VerifyCode(string secretKey, string code);
}
