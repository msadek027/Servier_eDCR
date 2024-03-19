using System.Collections.Generic;

namespace eDCR.Areas.DCR.Models.BEL
{
    public class DoctorDataUploadBEL
    {
        public virtual ICollection<DoctorDataUploadInfo> doctorDataUploadInfoList { get; set; }
    }

    public class DoctorDataUploadInfo
    {
        public string MonthNumber { get; set; }
        public string Year { get; set; }
        public string DoctorID { get; set; }
        public string DoctorName { get; set; }
        public string RegNo { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string Address4 { get; set; }
        public string Religion { get; set; }
        public string Specialization { get; set; }
        public string Potential { get; set; }
        public string Degrees { get; set; }
        public string Gender { get; set; }
        public string Designation { get; set; }
        public string MarketCode { get; set; }
        public string MorningLocation { get; set; }
        public string EveningLocation { get; set; }
        public string Market { get; set; }
        public string Region { get; set; }
        public string ProductGroup { get; set; }
        public string Adoption { get; set; }
        public string TeamTarget { get; set; }
    }


}