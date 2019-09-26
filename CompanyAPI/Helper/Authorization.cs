using System;
using Microsoft.AspNetCore.Http;

namespace CompanyAPI.Helper
{
    public class Authorization
    {
        public static Model.Payloads GetUser(HttpContext context)
        {
            var authHeader = context.Request.Headers["Authorization"].ToString();
            var retVal = new Model.Payloads();
            if (string.IsNullOrEmpty(authHeader)) return retVal;
            var payload64Str = authHeader.Substring("Bearer ".Length).Trim().Split(".")[1];
            for (var i = 0; i < payload64Str.Length % 4; i++)
            {
                if (i != 2)
                    payload64Str += "=";
            }
            var payloadstr = System.Text.Encoding.ASCII.GetString(Convert.FromBase64String(payload64Str));
            retVal = Newtonsoft.Json.JsonConvert.DeserializeObject<Model.Payloads>(payloadstr);
            return retVal;
        }
    }
}
