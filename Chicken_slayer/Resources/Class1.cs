using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chicken_slayer.Resources
{
    internal static class FirestoreHelper
    {
        static string fireconfig = @"
{
  ""type"": ""service_account"",
  ""project_id"": ""leaderboard-935a2"",
  ""private_key_id"": ""de28f8217ebb99cff0d3912cb1267387e0dfc2d8"",
  ""private_key"": ""-----BEGIN PRIVATE KEY-----\nMIIEvgIBADANBgkqhkiG9w0BAQEFAASCBKgwggSkAgEAAoIBAQCozogBWEq6HOhE\nMOh7/QJ9JOQ5cGv55zJATRfku6VlylxgXaty5OPUA0/2ZEsJDXnU0KFsgh+TUv9M\nxm/9VrJFo8sa0OQqmacgd9Y60aDf4+srA7rEgM5cPjVGj+zVwvkXbhrgeQKAd7Vk\nK41VH2EnLQunNLmqv/TtQhoeTrd23N8g0CUxzP5rdbCV54LbiiQND+Mwf+DsAZWT\ncRPBUCs3uW7zhoGCLi2SQwl2jzUGF3ZQZBulEo/zjAKoDyafgnEDMn7la7VKeDaV\nQf6ko2c1jkjx8jBYY/a8vOuyboSIQ6l+XY//FYkzh/YmW2P+6vKyLuhGz++x1FY3\nHFEpiX6LAgMBAAECggEAEMmmK1wSGNUFh4j41e8HiSKOGX6qSyQVX0ex3jfuVWOW\nl3U5UI43o77glbeYe47KuvUBclhOg5XeRehfyTYO0wlco/VHFUvkNXQCUVjGD8rY\neS9zXe96/Ll2QU1qJEGyCNGuYpYmPyVCKNm1ju2b+XI4lakssP2xTspWhp6vDpon\nJTQrNSmeDvu3utKvZzxA4jGxoPTpofkZKac97hIwtJg5RwQ574hhJNJk3GzCeuEy\nvSMbfv9aDMKxaPStfE4qG6n8Pu+8B3/zsVNXXKJpMqAAYyHls0MDMF4lvk//M5wh\nMXhEu+alAw2Amyrj+Gt7t6Zw8fVhZbfrVhyz9/KFYQKBgQDi1KfpARXNlp7i2iX2\npF4U4zdJloC2X94n4YFFqLZW+K2MP66p0g8sX2GWHexUl4Ag1vU6cgcyuu9P/UMb\nc/GwEvyoEZ+/9TMI84bHWMIJCqepdWHiiqMOcgjKtiV7SMNlSuxCnVlGqwJcKYz9\nC7dPQAE7wG10/Oi8lUOdx0JjJwKBgQC+g7U2UydeXt8mVvOkZflOmpVc0gSIMvq9\nHhvgPPvKPR/kHW1IUOn32bixM25ZhHlTFrmb9J8agawCfpD3rlQjkJ564Ucc3ION\na6jWlB04AuJbXW2sx7lNFqWPYu/84guIAs5KG2+dn6T5hHIt4diyL5JsQAA/AfrM\ndQE/mPMX/QKBgQCuFHYoo+SPuyCFCNZJyytI7Hfpyc9OJ5TfFnmXdDs70+2czOLH\nAacTbr/97UkKQpramR3qlMhhboVq5fsyfnDlqCyu75eiQhPAsuSk6MFBHyQ9MRDG\nQ4+P7oRAzjlFvDn46t8W4Js7WNKe2GSTbwJnl79ak/ts5QvYbtVNqTaR0QKBgANd\nxGfvVDTArGS7CJcRDjSrWpRHSbk2XWPWElwF99T8+4FGW9X5SAkPGcpXxE1kn9gu\nAJ2W1vDa9bUdk2Ys+GOgfIxjSqRh6Rkom9UezN1ef61pmzSAWu/XxJkiAGeRfu6q\nTgbVMUWxunKGOpZA9VlAqe+rudVjLZFLR/hdtX3pAoGBAIl0IW993au5uxXRatEZ\nj3ZW1CHGXZPq9ziv1l42zNBwmYnxeHcMhMhrsxNVlRjB5GMpnG9SeYBuUrjJW7Ww\n2JBc0zJxgH0ezwpu4ZJq0EcET3YUypFQI1RqWAE0GbtORModFQ6CHI2j+56h9BSC\nZ558ty1RMgMMGcrRQXiy0S/h\n-----END PRIVATE KEY-----\n"",
  ""client_email"": ""firebase-adminsdk-96hw7@leaderboard-935a2.iam.gserviceaccount.com"",
  ""client_id"": ""108075012586689510163"",
  ""auth_uri"": ""https://accounts.google.com/o/oauth2/auth"",
  ""token_uri"": ""https://oauth2.googleapis.com/token"",
  ""auth_provider_x509_cert_url"": ""https://www.googleapis.com/oauth2/v1/certs"",
  ""client_x509_cert_url"": ""https://www.googleapis.com/robot/v1/metadata/x509/firebase-adminsdk-96hw7%40leaderboard-935a2.iam.gserviceaccount.com"",
  ""universe_domain"": ""googleapis.com""
}
";
        static string filepath = "";
        public static FirestoreDb Database { get; private set; }

        public static void SetEnvironmentVarible()
        {
            filepath = Path.Combine(Path.GetTempPath(), Path.GetFileNameWithoutExtension(Path.GetRandomFileName())) + ".json";
            File.WriteAllText(filepath, fireconfig);
            File.SetAttributes(filepath, FileAttributes.Hidden);
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", filepath);
            Database = FirestoreDb.Create("leaderboard-935a2");
            File.Delete(filepath);
        }
    }
}

