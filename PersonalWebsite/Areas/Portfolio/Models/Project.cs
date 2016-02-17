using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PersonalWebsite.Areas.Portfolio.Models
{
    public class Project
    {
        public string Title { get; set; }
        public string ImageSrc { get; set; }
        public string Description { get; set; }
        public KeyValuePair<string, string> Modal { get; set; }

        public Project(string title, string modalTitle, string modalView,
            string imageSrc = "~/images/360-temp.png", string description = "")
        {
            this.Title = title;
            this.ImageSrc = imageSrc;
            this.Description = description;
            this.Modal = new KeyValuePair<string, string>("modal-" + modalTitle, modalView);
        }
    }
}