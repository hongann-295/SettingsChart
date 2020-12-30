using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Christoc.Modules.SettingsChart2.Models
{
    static class Help
    {
       public static List<int> GetX(string dadas, List<Models.GetPerson> peoples)
        {
           /* var people = new List<object>();*/

            if (dadas == "Id")
            {
                return peoples.Select(p =>  p.Id).ToList();
            }
            return null;
        }


    }
}