namespace Dice.Utils;

using System.Security.Cryptography;

public static class RandomNumber
{
    public static int Generate(int max)
    {
        if (max <= 0 || max > byte.MaxValue)
        {
            throw new ArgumentException("max should be between 1 and 255");
        }

        using (var rng = RandomNumberGenerator.Create())
        {
            int maxAcceptableValue = byte.MaxValue - (byte.MaxValue % (max + 1));

            int result;
            byte[] randomBytes = new byte[1];

            do
            {
                rng.GetBytes(randomBytes);
                result = randomBytes[0];
            } while (result > maxAcceptableValue);

            return result % (max + 1);
        }
    }
}
