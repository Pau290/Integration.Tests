using System;
using System.Collections.Generic;

namespace Library
{
    public class DtoMain
    {
        public int Id { get; set; }
        
        public string Description { get; set; }
        
        public decimal Percent { get; set; }       

        public DtoFourth Fourth { get; set; }

        public List<DtoOne> Ones { get; set; }
    }
}
