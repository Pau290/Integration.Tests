using System;
using System.Collections.Generic;

namespace Library
{
    public class DtoFourth
    {
        public string Description { get; set; }

        public DtoThird Third { get; set; }

        public DtoSecond Second { get; set; }

        public List<DtoSecond> SecondList { get; set; }
    }
}
