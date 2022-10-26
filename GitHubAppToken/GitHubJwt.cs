using System;
using System.Security.Cryptography;
using System.Text;

namespace GitHubAppToken
{
    public static class GitHubJwt
    {
        public static string Create(string pemRsaKey, long appId, int expMinutes = 10)
        {
            var key = PemKeyDecoder.Decode(pemRsaKey);
            var exp = DateTimeOffset.UtcNow.AddMinutes(expMinutes).ToUnixTimeSeconds();
            var iat = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            var p1 = Convert.ToBase64String(Encoding.UTF8.GetBytes("{\"typ\": \"JWT\",\"alg\": \"RS256\"}"));
            var p2 = Convert.ToBase64String(
                Encoding.UTF8.GetBytes($"{{\"exp\": {exp},\"iat\": {iat},\"iss\": {appId}}}"));
            var c = $"{p1}.{p2}";
            var signData = key.SignData(Encoding.UTF8.GetBytes(c), HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
            var p3 = Convert.ToBase64String(signData);
            return $"{p1}.{p2}.{p3}";
        }
    }
}