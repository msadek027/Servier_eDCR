﻿using eDCR.Areas.SA.Models.BEL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace eDCR.Models
{
    public class LoginRegistrationModel : UserInRoleBEL
    {
        public string UserID { get; set; }
        public string Password { get; set; }



    }
}