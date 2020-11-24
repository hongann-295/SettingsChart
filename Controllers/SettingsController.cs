/*
' Copyright (c) 2020 Christoc.com
'  All rights reserved.
' 
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED
' TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
' THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF
' CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
' DEALINGS IN THE SOFTWARE.
' 
*/

using DotNetNuke.Web.Mvc.Framework.Controllers;
using DotNetNuke.Collections;
using System.Web.Mvc;
using DotNetNuke.Security;
using DotNetNuke.Web.Mvc.Framework.ActionFilters;
using Christoc.Modules.SettingsChart2.Components;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Collections;
using System.Linq;
using System.Data;
using System;
using System.Web.UI.WebControls;

namespace Christoc.Modules.SettingsChart2.Controllers
{
    [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.Edit)]
    [DnnHandleError]
    public class SettingsController : DnnController
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Settings(int moduleId)
        {
            var settingsChart = new Models.GetSettingsChart();
            var getCities = ItemManager.Instance.Cities();
            var selectCity = new List<SelectListItem>();
            foreach (var element in getCities)
            {
                selectCity.Add(new SelectListItem
                {
                    Value = element.IdCity.ToString(),
                    Text = element.CityName
                });
            }
            SelectList selectCities = new SelectList(selectCity, "Value", "Text");
            ViewBag.Cities = selectCities;

            var getfieldX = ItemManager.Instance.GetFields();
            var selectX = new List<SelectListItem>();
            foreach (var element in getfieldX)
            {
                selectX.Add(new SelectListItem
                {
                    Value = element.name,
                    Text = element.name
                });
            }
            SelectList selectListX = new SelectList(selectX, "Value", "Text");
            ViewBag.ListFieldX = selectListX;

            var getfield = ItemManager.Instance.GetFields();
            var selectLists = new List<SelectListItem>();
            foreach (var element in getfield)
            {
                selectLists.Add(new SelectListItem
                {
                    Value = element.name,
                    Text = element.name
                });
            }
            MultiSelectList selectList = new
                MultiSelectList(selectLists, "Value", "Text");
            ViewBag.ListField = selectList;

            //var organizations = ItemManager.Instance.GetModule(moduleId);
            //return Json(new { data = JsonConvert.SerializeObject(organizations, Formatting.Indented) }, JsonRequestBehavior.AllowGet);

            return View(settingsChart);
        }

        [HttpGet]
        public JsonResult GetPersons(int id)
        {
            var person = ItemManager.Instance.GetPersons(id);
            return Json(new { data = JsonConvert.SerializeObject(person, Formatting.Indented) }, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="supportsTokens"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        [DotNetNuke.Web.Mvc.Framework.ActionFilters.ValidateAntiForgeryToken]
        public ActionResult Settings(Models.GetSettingsChart settingsChart)
        {
            //settingsChart.ChonY;
            ModuleContext.Configuration.ModuleSettings["SettingsChart_LoaiBieuDo"] = settingsChart.LoaiBieuDo.ToString();
            ModuleContext.Configuration.ModuleSettings["SettingsChart_TenBieuDo"] = settingsChart.TenBieuDo.ToString();
            ModuleContext.Configuration.ModuleSettings["SettingsChart_MoTaBieuDo"] = settingsChart.MoTaBieuDo.ToString();
            ModuleContext.Configuration.ModuleSettings["SettingsChart_TenX"] = settingsChart.TenX.ToString();
            ModuleContext.Configuration.ModuleSettings["SettingsChart_TenY"] = settingsChart.TenY.ToString();
            ModuleContext.Configuration.ModuleSettings["SettingsChart_ChonX"] = settingsChart.ChonX.ToString();
            ModuleContext.Configuration.ModuleSettings["SettingsChart_ChonY"] = String.Join(",", settingsChart.ChonY); // settingsChart.ChonY.ToString();
            ModuleContext.Configuration.ModuleSettings["SettingsChart_ChonCungChuDe"] = String.Join(",", settingsChart.ChonCungChuDe); //settingsChart.ChonCungChuDe.ToString();

           
            //var dataY = ItemManager.Instance.GetPeople().ToList();
            //var dataResultYy = String.Join(",", settingsChart.ChonY);
            //var result = " ";

            //var dataChuDe = ItemManager.Instance.GetPeople().ToList();
            //var dataTopic = String.Join(",", settingsChart.ChonCungChuDe);
            //for(var i = 0; i < dataY.Count(); i++)
            //{
            //    if(dataResultYy == "Age")
            //    {
            //        result = dataY[i].Age.ToString();
            //    }
            //    if(dataResultYy == "Name")
            //    {
            //        result = dataY[i].Name;
            //    }


            //    //for(var j = 0; j < dataChuDe.Count(); j++)
            //    //{

            //    //}
            //}
            

            //var dataResultY = (from s in dataY select s.Name);
            //for(var i = 0; i < dataResultY.Count(); i++)
            //{
            //    if(dataResultYy == dataResultY.ToString())
            //    {
            //        dataResultY = dataResultYy[i].ToString();
            //    }
            //}
            //foreach (var item in dataResultY)
            //{
            //    dataResultYy = item;
            //}

            //ModuleContext.Configuration.ModuleSettings["SettingsChart_ChonY"] = String.Join(",", result);

            //Name, Gender
            // An, Duy
            // truy vaans towi bang HighChartGender lay cac cot co gia tri cua An va Duy
            // for (var id in An, Duy)
            // get select Name, Gender from HighChartGender where Id=id

            //var dataChuDe = ItemManager.Instance.GetPeople().ToList();
            //var dataResultChuDe = (from s in dataY select s.Name).ToList();
            //ModuleContext.Configuration.ModuleSettings["SettingsChart_ChonCungChuDe"] = String.Join(",", dataResultChuDe); 


            return RedirectToDefaultRoute();
        }
    }
}