using Kr.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace KR.Web.Models
{
    public class UserData
    {
        public Guid UserId { get; set; }
        public string Password { get; set; }
        public string Login { get; set; }
    }
}
