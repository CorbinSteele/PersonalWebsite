using System;
using System.Runtime.Serialization;

namespace PersonalWebsite.Areas.CarFinder.Models
{
    [DataContract]
    public class Recall
    {
        [DataMember(Name = "Manufacturer")]
        public string Manufacturer { get; set; }
        [DataMember(Name = "NHTSACampaignNumber")]
        public string NHTSACampaignNumber { get; set; }
        //public string NHTSAActionNumber { get; set; }
        [DataMember(Name = "ReportReceivedDate")]
        public DateTime ReportReceivedDate { get; set; }
        [DataMember(Name = "Component")]
        public string Component { get; set; }
        [DataMember(Name = "Summary")]
        public string Summary { get; set; }
        [DataMember(Name = "Conequence")]
        public string Consequence { get; set; }
        [DataMember(Name = "Remedy")]
        public string Remedy { get; set; }
    }

    [DataContract]
    public class RecallsAPI
    {
        [DataMember(Name = "Results")]
        public Recall[] Results { get; set; }
    }
}