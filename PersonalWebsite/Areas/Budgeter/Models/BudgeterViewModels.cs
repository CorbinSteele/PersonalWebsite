using PersonalWebsite.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PersonalWebsite.Areas.Budgeter.Models
{
    public class HouseholdView
    {
        [Required]
        public string Name { get; set; }
    }
    public class InviteMemberView : SimpleTempable
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
    }
    public class AddAccountView : SimpleTempable
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public double Balance { get; set; }
    }
}