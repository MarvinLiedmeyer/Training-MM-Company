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
            string authHeader = context.Request.Headers["Authorization"];
            Model.Payloads retVal = new Model.Payloads();
            if (authHeader != null)
            {
                var authHeaderstr = context.Request.Headers["Authorization"].ToString();
                var payload64str = authHeaderstr.Substring("Bearer ".Length).Trim().Split(".")[1];
                var payloadstr = System.Text.Encoding.ASCII.GetString(Convert.FromBase64String(payload64str));
                retVal = Newtonsoft.Json.JsonConvert.DeserializeObject<Model.Payloads>(payloadstr);
            }
            return retVal;
        }
    }
}
