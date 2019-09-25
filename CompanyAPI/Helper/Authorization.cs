using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace CompanyAPI.Helper
{
    public class Authorization
    {
        public static Model.Payloads GetUser(HttpContext context)
        {
            string authHeader = context.Request.Headers["Authorization"].ToString();
            Model.Payloads retVal = new Model.Payloads();
            if (!string.IsNullOrEmpty(authHeader))
            {
                var payload64str = authHeader.Substring("Bearer ".Length).Trim().Split(".")[1];
                for (int i = 0; i < payload64str.Length % 4; i++)
                {
                    if (!(i == 2))
                        payload64str += "=";
                }
                var payloadstr = System.Text.Encoding.ASCII.GetString(Convert.FromBase64String(payload64str));
                retVal = Newtonsoft.Json.JsonConvert.DeserializeObject<Model.Payloads>(payloadstr);
            }
            return retVal;
        }
    }
}
