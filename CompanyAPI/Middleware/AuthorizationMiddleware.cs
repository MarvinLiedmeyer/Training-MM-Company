using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using CompanyAPI.Helper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace CompanyAPI.Middleware
{
    public class AuthorizationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RepoExceptionMiddleware> _logger;

        public AuthorizationMiddleware(RequestDelegate next, ILoggerFactory logger)
        {
            _next = next;
            _logger = logger.CreateLogger<RepoExceptionMiddleware>();
        }

        public async Task Invoke(HttpContext context)
        {
            int temp = Helper.Authorization.GetUser(context).LocationID;
            var authtokenheader = context.Request.Headers["Authorization"];
            if (authtokenheader.Count == 0)
            {
                context.Response.StatusCode = 401;
                context.Response.WriteAsync("Unauthorized");
            }
            else
            {
                var client = new RestClient();
                var request = new RestRequest($"https://chaynssvc.tobit.com/v0.5/{temp}/User");
                request.AddHeader("Authorization", authtokenheader);
                var cancellationTokenSource = new CancellationTokenSource();
                var restResponse = await client.ExecuteTaskAsync(request, cancellationTokenSource.Token);
                var myObject = JsonConvert.DeserializeObject<JObject>(restResponse.Content)["data"]["uacGroups"].Children();

                var authorized = myObject.ToList<JToken>().Find(e => e["id"].ToObject<int>() == 1) != null;

                if (authorized)
                {
                    await _next(context);
                }
                else
                {

                    context.Response.StatusCode = 401;
                    context.Response.WriteAsync("Unauthorized");
                }
            }


        }
    }
}
//eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCIsInZlciI6MSwia2lkIjoicmRoZW1kYmQifQ.eyJqdGkiOiJkYTcxYTJjNy00ZTliLTQ5NWYtOTUxZC04NTkyYjc1NDQ1NjQiLCJzdWIiOiIxMzEtOTk5OTgiLCJ0eXBlIjoxLCJleHAiOiIyMDE5LTA5LTI5VDEyOjUyOjI1WiIsImlhdCI6IjIwMTktMDktMjVUMTI6NTI6MjVaIiwiTG9jYXRpb25JRCI6MTY0OTg1LCJTaXRlSUQiOiI3Nzg5My0xMTg5MyIsIklzQWRtaW4iOmZhbHNlLCJUb2JpdFVzZXJJRCI6MjA3Mzk3MSwiUGVyc29uSUQiOiIxMzEtOTk5OTgiLCJGaXJzdE5hbWUiOiJMdWNhIiwiTGFzdE5hbWUiOiJKZXN1w59layJ9.i1E83PVj189gk3juwYODW7R_YcdpnEmtfl5-FTXX7Q2uyPXImn01Nmvap3wiTmvUNOaKlOuZq1cLJTP01jv9qDE_eyFZgFnZYka_8S0SL58IiUOW3tZHZ5wG7WeH9zbJSyA4f449scqx5ZTKMKgT3CHRYlCUZx0xY6lQlcrm2sek1rvXroV5Iiz13VFPPR7fHwOMkhNDY5tup_m_N5Piipff8HycQmZNfY7lN5p2QpSINJR6WOsGmC5t8Nkl4EZ3h3q4RtrXfAu3FMMa5rsGhnL4uY21_qRqThMfLdsFKeV0IKOeZD4YriG-vwaO-Q5CPQejgc4jEvhnrPK-vqCRIA
//eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCIsInZlciI6MSwia2lkIjoibWRjeG1kYmQifQ.eyJqdGkiOiIyM2MyZTA2Zi02NmYxLTRhNzgtOTZiZC02N2UyOTIzM2RjZWUiLCJzdWIiOiIxMzEtODU4MDkiLCJ0eXBlIjoxLCJleHAiOiIyMDE5LTA5LTI3VDA3OjE1OjMxWiIsImlhdCI6IjIwMTktMDktMjNUMDc6MTU6MzFaIiwiTG9jYXRpb25JRCI6MTY0OTg1LCJTaXRlSUQiOiI3Nzg5My0xMTg5MyIsIklzQWRtaW4iOnRydWUsIlRvYml0VXNlcklEIjoyMDYyMjEwLCJQZXJzb25JRCI6IjEzMS04NTgwOSIsIkZpcnN0TmFtZSI6Ik1hcnZpbiIsIkxhc3ROYW1lIjoiTGllZG1leWVyIn0.dueTY74lEqax0-qNQz-evbWikyDqD8hwysNy1hC5EZCN7pwG1xsD42YwLS_0S7U2PhUQkW0KvORMkUQFVB9TAXHsxa4qo1CrYSaZe_6g4T14mHcTpLJPvuzfwrXcTY64n4ddpSxjo2_q4NZgXmFbztYeLTBysN58_8T4kwzQcaXxmnQawCmkNUhfiOiYclw-eqUDtN8v1eZ-qDl_w3EEnNpo8WYFWnkY5nyHnJxIn_wK7S55IR_xV5kyWpz-Iz-_z5I9kE6bXHu8CVeRhQKOe1Yd37sQuNWRbW7WJB9aSKh_vad_IJtvNrMynuf4psBgn1KmOWFeSnCYKGhxIQf2nw