using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Mail;
using System.Reflection;
using System.Runtime.Intrinsics.X86;

namespace WebApplication1.Models
{
    public class Employee
    {
        public int EmpID { get; set; }
        public string Name { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string MaidenName { get; set; }
        public string Birthdate { get; set; }
        public string Age { get; set; }
        public string BirthMonth { get; set; }
        public string AgeBracket { get; set; }
        public string Gender { get; set; }
        public string MaritalStatus { get; set; }
        public string SSS { get; set; }
        public string PHIC { get; set; }
        public string HDMF { get; set; }
        public string TIN { get; set; }
        public string HRANID { get; set; }
        public string ContactNumber { get; set; }
        public string EmailAddress { get; set; }

    }
}
