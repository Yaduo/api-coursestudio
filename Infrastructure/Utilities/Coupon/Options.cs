using System;
namespace CourseStudio.Lib.Utilities.Coupon
{
    public class Options
    {
		public int Parts { get; set; }
        public int PartLength { get; set; }
        public string Plaintext { get; set; }
        
        public Options()
        {
            this.Parts = 4;
            this.PartLength = 4;
            this.Plaintext = "COUP";
        }
    }
}
