using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Reshop.Application.Security.GoogleRecaptcha;
using System.IO;
using System.Net;

namespace Reshop.Application.Validations.Google
{
    public static class GoogleReCaptchaValidation
    {
        public static bool IsGoogleRecaptchaValid(IFormCollection form, string secretKey)
        {
            string urlToPost = "https://www.google.com/recaptcha/api/siteverify";
            string gRecaptchaResponse = form["g-recaptcha-response"];

            string postData = "secret=" + secretKey + "&response=" + gRecaptchaResponse;

            // send post data
            var request = (HttpWebRequest)WebRequest.Create(urlToPost);
            request.Method = "POST";
            request.ContentLength = postData.Length;
            request.ContentType = "application/x-www-form-urlencoded";

            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                streamWriter.Write(postData);
            }

            // receive the response now
            string result = string.Empty;
            using (var response = (HttpWebResponse)request.GetResponse())
            {
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    result = reader.ReadToEnd();
                }
            }

            // validate the response from Google reCaptcha
            var captchaResponse = JsonConvert.DeserializeObject<GoogleReCaptchaResponse>(result);

            return captchaResponse.Success; // true or false
        }
    }
}
