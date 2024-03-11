using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace eDCR.Areas.SA.Models.BEL
{
    public class SoftwareBEL : GlobalBEL
    {

        public string SoftwareID { get; set; }
        public string SoftwareName { get; set; }
        public Boolean IsActive { get; set; }

        public string SoftwareShortName { get; set; }
    }
}
