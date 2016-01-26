namespace CarFinder.Models
{
    public class Car
    {
        public int ID { get; set; }
        public string Make { get; set; }
        public string Make_Display { get; set; }
        public string Model_Name { get; set; }
        public string Model_Trim { get; set; }
        public string Model_Year { get; set; }

        public string body_style { get; set; }
        public string weight_kg { get; set; }
        public string transmission_type { get; set; }
        public string drive_type { get; set; }
        public string engine_fuel { get; set; }
        public string fuel_capacity_l { get; set; }
        public string engine_num_cyl { get; set; }
        public string engine_power_ps { get; set; }
        public string engine_power_rpm { get; set; }
        public string doors { get; set; }
        public string seats { get; set; }

        public string[] ImageUrls { get; set; }
        public Recall[] Recalls { get; set; }
    }
}