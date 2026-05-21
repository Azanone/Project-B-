using System;
using OtpNet;
using QRCoder;

public class TwoFactorService
{
    public (string Base32Secret, string QrCodeUri) EnableTwoFactor(string userEmail)
    {
        byte[] secretBytes = KeyGeneration.GenerateRandomKey(20);
        string base32Secret = Base32Encoding.ToString(secretBytes);
        
        string issuer = Uri.EscapeDataString("YourAppNameOrCompany");
        string account = Uri.EscapeDataString(userEmail);
        string qrCodeUri = $"otpauth://totp/{issuer}:{account}?secret={base32Secret}&issuer={issuer}&digits=6&period=30";
        
        return (base32Secret, qrCodeUri);
    }

    public string GenerateQrCodeBase64(string qrCodeUri)
    {
        using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
        {
            using (QRCodeData qrCodeData = qrGenerator.CreateQrCode(qrCodeUri, QRCodeGenerator.ECCLevel.Q))
            {
                using (PngByteQRCode qrCode = new PngByteQRCode(qrCodeData))
                {
                    byte[] qrCodeAsPngByteArr = qrCode.GetGraphic(20);
                    return Convert.ToBase64String(qrCodeAsPngByteArr);
                }
            }
        }
    }

    public bool VerifyToken(string userBase32Secret, string userEnteredCode)
    {
        byte[] secretBytes = Base32Encoding.ToBytes(userBase32Secret);
        var totp = new Totp(secretBytes);
        
        long timeStepMatched;
        bool isValid = totp.VerifyTotp(
            userEnteredCode, 
            out timeStepMatched, 
            VerificationWindow.RfcSpecifiedNetworkDelay
        );
        
        return isValid;
    }
}