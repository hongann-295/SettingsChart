using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Christoc.Modules.SettingsChart2.Models
{
    public class GetSettingsChart
    {
        public int IdCity { get; set; }
        public string LoaiBieuDo { get; set; }
        public string TenBieuDo { get; set; }
        public string MoTaBieuDo { get; set; }
        public string TenX { get; set; }
        public string TenY { get; set; }
        public string ChonX { get; set; }
        public string[] ChonY { get; set; }
        public string[] ChonCungChuDe { get; set; }
    }
}