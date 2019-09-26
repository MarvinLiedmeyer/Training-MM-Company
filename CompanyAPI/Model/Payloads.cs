using System;

namespace CompanyAPI.Model
{
    public class Payloads
    {
        public string jti { get; set; }
        public string sub { get; set; }
        public int type { get; set; }
        public DateTime exp {get;set;}
        public DateTime iat { get; set; }
        public int LocationID { get; set; }
        public string SiteID { get; set; }
        public bool IsAdmin { get; set; }
        public int TobitUserID { get; set; }
        public string PersonID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }



    }
}
