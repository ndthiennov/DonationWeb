using DonationAppDemo.DTOs;
using System.Security.Cryptography;

namespace DonationAppDemo.Helper
{
    public class DataEncryptionExtensions
    {
        public static HashSaltDto HMACSHA512(string code)
        {
            byte[] codeHash, codeKey;
            using (var hmac = new HMACSHA512())
            {
                codeKey = hmac.Key;
                codeHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(code));
            }
            return new HashSaltDto
            {
                hashedCode = codeHash,
                keyCode = codeKey
            };
        }
        public static bool MatchCodeHashHMACSHA512(string code, byte[] hashedCode, byte[] keyCode)
        {
            using (var hmac = new HMACSHA512(keyCode))
            {
                var codeHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(code));
                for (int i = 0; i < codeHash.Length; i++)
                {
                    if (codeHash[i] != hashedCode[i])
                    {
                        return false;
                    }
                }
                return true;
            }
        }
    }
}
