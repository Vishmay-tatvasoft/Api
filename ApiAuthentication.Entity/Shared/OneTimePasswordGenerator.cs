using System.Security.Cryptography;
using System.Text;

namespace ApiAuthentication.Entity.Shared;

public class OneTimePasswordGenerator
{
    public static string GenerateNumericOtp(int length = 6)
    {
        if (length <= 0) throw new ArgumentException("OTP length must be positive.");

        var otp = new StringBuilder(length);
        using var rng = RandomNumberGenerator.Create();

        byte[] randomNumber = new byte[1];

        for (int i = 0; i < length; i++)
        {
            rng.GetBytes(randomNumber);
            int digit = randomNumber[0] % 10; // 0–9
            otp.Append(digit);
        }

        return otp.ToString();
    }

    public static string GenerateAlphaNumericOtp(int length = 8)
    {
        const string chars = "ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz23456789"; // Avoid confusing chars
        if (length <= 0) throw new ArgumentException("OTP length must be positive.");

        var otp = new StringBuilder(length);
        using var rng = RandomNumberGenerator.Create();

        byte[] randomByte = new byte[1];

        for (int i = 0; i < length; i++)
        {
            rng.GetBytes(randomByte);
            int idx = randomByte[0] % chars.Length;
            otp.Append(chars[idx]);
        }

        return otp.ToString();
    }
}
