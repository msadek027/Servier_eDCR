using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using eDCR.Models;

namespace eDCR.Areas.SA.Models.BEL
{
    public class UserInRoleBEL 
    {
        public string UserID { get; set; }
        public string Password { get; set; } 
        public string RoleID { get; set; }
        public string RoleName { get; set; }
   
        public string EmpID { get; set; }
        public string EmpName { get; set; }
        public string SupervisorID { get; set; }
        public string SupervisorName { get; set; }
        public string EmploymentDate { get; set; }
        public string Designation { get; set; }

        public string LocCode { get; set; }
        public string LocName { get; set; }
        

        public bool IsActive { get;set; }


        public string BuyerID { get; set; }
        public string BuyerName { get; set; }
        public string YetAssigned { get; set; }

  

        public string DataOwner { get; set; }

        public string MPGroup { get; set; }
    }

public class UserInRoleBELDetail
{
    public virtual ICollection<UserInRoleBEL> ListAll { get; set; }
}


    public class UserInRoleAllRelationalData
    {
        
        public string SBU { get; set; }
        public string DivisionCode { get; set; }
        public string DivisionName { get; set; }
        public string DepotCode { get; set; }
        public string DepotName { get; set; }       
          
        public string RegionCode { get; set; }
        public string RegionName { get; set; }
        public string TerritoryCode { get; set; }
        public string TerritoryName { get; set; }
        public string MarketCode { get; set; }
        public string MarketName { get; set; }
        public string RsmId { get; set; }
        public string RsmName { get; set; }
        public string TmId { get; set; }
        public string TmName { get; set; }
        public string MpoCode { get; set; }
        public string MpoName { get; set; }
        public string Designation { get; set; }
        public string PhoneNo { get; set; }
        
        public string MPGroup { get; set; }


        public string UserID { get; set; }

        public string EmpID { get; set; }

        public string EmpName { get; set; }

     

     

        public string Password { get; set; }

        public string LoginDate { get; set; }

        public string LoginTime { get; set; }

        public string RowNumber { get; set; }

        public string AppWeb { get; set; }
        public string RsmPassword { get;  set; }
        public string TmPassword { get;  set; }
        public string MpoPassword { get;  set; }
        public string DsmId { get;  set; }
        public string DsmName { get;  set; }
        public string DsmPassword { get;  set; }
    }

      public class UserInLoginLog

      {
      
          public string EmpName { get; set; }

          public string EmpID { get; set; }

          public string Designation { get; set; }

          public string MarketName { get; set; }

          public string RegionName { get; set; }

          public string LoginDate { get; set; }

          public string LoginTime { get; set; }

          public string AppWeb { get; set; }

          
      }
}