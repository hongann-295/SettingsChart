﻿/*
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
        public ActionResult Settings()
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

            //var getfield = ItemManager.Instance.GetFields();
            //var selectLists = new List<SelectListItem>();
            //foreach (var element in getfield)
            //{
            //    selectLists.Add(new SelectListItem
            //    {
            //        Value = element.name,
            //        Text = element.name
            //    });
            //}
            //MultiSelectList selectList = new MultiSelectList(selectLists, "Value", "Text");
            ViewBag.ListField = GetSelectListItems(null);

            return View(settingsChart);
        }


        private MultiSelectList GetSelectListItems(string[] selectedValues)
        {
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
            return new MultiSelectList(getfield, "name", "name", selectedValues);
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
            ModuleContext.Configuration.ModuleSettings["SettingsChart_LoaiBieuDo"] = settingsChart.LoaiBieuDo.ToString();
            ModuleContext.Configuration.ModuleSettings["SettingsChart_TenBieuDo"] = settingsChart.TenBieuDo.ToString();
            ModuleContext.Configuration.ModuleSettings["SettingsChart_MoTaBieuDo"] = settingsChart.MoTaBieuDo.ToString();
            ModuleContext.Configuration.ModuleSettings["SettingsChart_TenX"] = settingsChart.TenX.ToString();
            ModuleContext.Configuration.ModuleSettings["SettingsChart_TenY"] = settingsChart.TenY.ToString();
            ModuleContext.Configuration.ModuleSettings["SettingsChart_ChonX"] = settingsChart.ChonX.ToString();
            ModuleContext.Configuration.ModuleSettings["SettingsChart_ChonY"] = settingsChart.ChonY.ToString();
            ModuleContext.Configuration.ModuleSettings["SettingsChart_ChonCungChuDe"] = settingsChart.ChonCungChuDe.ToString();

            return RedirectToDefaultRoute();
        }
    }
}