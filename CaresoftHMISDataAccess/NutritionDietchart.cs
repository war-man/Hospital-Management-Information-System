//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CaresoftHMISDataAccess
{
    using System;
    using System.Collections.Generic;
    
    public partial class NutritionDietchart
    {
        public int id { get; set; }
        public int user_id { get; set; }
        public int OPDIPDID { get; set; }
        public Nullable<System.DateTime> start_date { get; set; }
        public Nullable<System.TimeSpan> start_time { get; set; }
        public Nullable<System.DateTime> end_date { get; set; }
        public Nullable<System.TimeSpan> end_time { get; set; }
        public string meal_time { get; set; }
        public string item_name { get; set; }
        public string quantity { get; set; }
        public Nullable<int> format_id { get; set; }
        public string format_name { get; set; }
        public string total_calories { get; set; }
        public byte[] date_time { get; set; }
    }
}
