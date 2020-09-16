using BlazorApp1.Data;
using System;

namespace BlazorApp1.ViewModel
{
    public class SampleViewModel
    {
        public string Field1Id { get; set; } = "Field1";
        public int Field2Id { get; set; }
        public int? Field3Id { get; set; }
        public double Field4Id { get; set; }
        public double? Field5Id { get; set; }
        public float Field6Id { get; set; }
        public float? Field7Id { get; set; }
        public decimal Field8Id { get; set; }
        public decimal? Field9Id { get; set; }

        public DateTime Field10Id { get; set; }
        public DateTime? Field11Id { get; set; }

        [DateTime]
        public DateTime Field12Id { get; set; }

        [NullableDateTime]
        public DateTime? Field13Id { get; set; }

        [TextArea]
        public string Field14Id { get; set; }
       
    }
}
