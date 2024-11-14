namespace Dice.Services;

using System.Security.Cryptography;
using Dice.Utils;

public class FairnessService : IFairnessService
{
    private const int Size = 256 / 8;

    public (int, string, string) GenerateFairNumber(int max)
    {
        int number = RandomNumber.Generate(max);
        byte[] key = GenerateKey();
        byte[] hmac = CalculateHMAC(number, key);
        return (number, BytesToString(key), BytesToString(hmac));
    }

    private static byte[] CalculateHMAC(int randomNumber, byte[] key)
    {
        using (var hmac = new HMACSHA3_256(key))
        {
            byte[] numberBytes = BitConverter.GetBytes(randomNumber);
            byte[] hash = hmac.ComputeHash(numberBytes);
            return hash;
        }
    }

    private static byte[] GenerateKey()
    {
        using (var rng = RandomNumberGenerator.Create())
        {
            byte[] key = new byte[Size];
            rng.GetBytes(key);
            return key;
        }
    }

    private static string BytesToString(byte[] bytes)
    {
        return Convert.ToHexString(bytes);
    }
}
