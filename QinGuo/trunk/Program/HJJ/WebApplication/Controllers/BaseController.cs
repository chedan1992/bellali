using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Commonlication
{
   
    public class BaseController : Controller
    {
        public DB db = new DB();

    }
}