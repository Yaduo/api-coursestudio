using System;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json.Linq;

namespace CourseStudio.Lib.Utilities.Captcha
{
	public static class GoogleReCaptchaHelper
    {
		public static bool IsReCaptchaPassed(string gRecaptchaResponse, string secret, string googleReCaptchauUrl)
        {
			if (gRecaptchaResponse == null) 
			{
				return false;
			}

            HttpClient httpClient = new HttpClient();
			//var res = httpClient.GetAsync(url + $"?secret={secret}&response={gRecaptchaResponse}").Result;
			var res = httpClient.GetAsync(googleReCaptchauUrl + "?secret=" + secret + "&response=" + gRecaptchaResponse).Result;
            if (res.StatusCode != HttpStatusCode.OK)
            {
                return false;
            }

            string JSONres = res.Content.ReadAsStringAsync().Result;
            dynamic JSONdata = JObject.Parse(JSONres);
            if (JSONdata.success != "true")
            {
                return false;
            }

            return true;
        }
    }
}
