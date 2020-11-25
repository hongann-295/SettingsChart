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

using System;
using System.Linq;
using System.Web.Mvc;
using Christoc.Modules.SettingsChart2.Components;
using Christoc.Modules.SettingsChart2.Models;
using DotNetNuke.Web.Mvc.Framework.Controllers;
using DotNetNuke.Web.Mvc.Framework.ActionFilters;
using DotNetNuke.Entities.Users;
using DotNetNuke.Framework.JavaScriptLibraries;
using Newtonsoft.Json;
using System.Collections.Generic;
using NPOI.HSSF.UserModel;
using System.IO;
using NPOI.SS.UserModel;

namespace Christoc.Modules.SettingsChart2.Controllers
{
    [DnnHandleError]
    public class ItemController : DnnController
    {

        public ActionResult Delete(int itemId)
        {
            ItemManager.Instance.DeleteItem(itemId, ModuleContext.ModuleId);
            return RedirectToDefaultRoute();
        }

        public ActionResult Edit(int itemId = -1)
        {
            DotNetNuke.Framework.JavaScriptLibraries.JavaScript.RequestRegistration(CommonJs.DnnPlugins);

            var userlist = UserController.GetUsers(PortalSettings.PortalId);
            var users = from user in userlist.Cast<UserInfo>().ToList()
                        select new SelectListItem { Text = user.DisplayName, Value = user.UserID.ToString() };

            ViewBag.Users = users;

            var item = (itemId == -1)
                 ? new Item { ModuleId = ModuleContext.ModuleId }
                 : ItemManager.Instance.GetItem(itemId, ModuleContext.ModuleId);

            return View(item);
        }

        [HttpPost]
        [DotNetNuke.Web.Mvc.Framework.ActionFilters.ValidateAntiForgeryToken]
        public ActionResult Edit(Item item)
        {
            if (item.ItemId == -1)
            {
                item.CreatedByUserId = User.UserID;
                item.CreatedOnDate = DateTime.UtcNow;
                item.LastModifiedByUserId = User.UserID;
                item.LastModifiedOnDate = DateTime.UtcNow;

                ItemManager.Instance.CreateItem(item);
            }
            else
            {
                var existingItem = ItemManager.Instance.GetItem(item.ItemId, item.ModuleId);
                existingItem.LastModifiedByUserId = User.UserID;
                existingItem.LastModifiedOnDate = DateTime.UtcNow;
                existingItem.ItemName = item.ItemName;
                existingItem.ItemDescription = item.ItemDescription;
                existingItem.AssignedUserId = item.AssignedUserId;

                ItemManager.Instance.UpdateItem(existingItem);
            }

            return RedirectToDefaultRoute();
        }

        [ModuleAction(ControlKey = "Edit", TitleKey = "AddItem")]
        public ActionResult Index()
        {
            var items = ItemManager.Instance.GetItems(ModuleContext.ModuleId);
            return View(items);
        }

        [HttpGet]
        public JsonResult GetSettingsChart()
        {
            var settingsChart = ItemManager.Instance.GetSettings();
            return Json(new { data = JsonConvert.SerializeObject(settingsChart, Formatting.Indented) }, JsonRequestBehavior.AllowGet);

        }

        //[HttpGet]
        //public JsonResult GetResults()
        //{
        //    var listCharts = ItemManager.Instance.GetCharts();
        //    return Json(new { data = JsonConvert.SerializeObject(listCharts, Formatting.Indented) }, JsonRequestBehavior.AllowGet);

        //}

        [HttpGet]
        public JsonResult GetResults()
        {
            var listCharts = ItemManager.Instance.GetCharts();
            Gender gender = new Gender();
            gender.Cities1 = new List<City>();
            gender.Cities2 = new List<City>();
            gender.Cities3 = new List<City>();
            foreach (var item in listCharts)
            {
                City city = new City();
                if (item.Gender == "Male")
                {
                    city.IdCity = item.IdCity;
                    city.CityName = item.CityName;
                    city.Amount = item.Amount;
                    gender.Cities1.Add(city);
                }
                if (item.Gender == "Female")
                {
                    city.IdCity = item.IdCity;
                    city.CityName = item.CityName;
                    city.Amount = item.Amount;
                    gender.Cities2.Add(city);
                }
                if (item.Gender == "Other")
                {
                    city.IdCity = item.IdCity;
                    city.CityName = item.CityName;
                    city.Amount = item.Amount;
                    gender.Cities3.Add(city);
                }
            }
            return Json(new { gender = JsonConvert.SerializeObject(gender, Formatting.Indented) }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult Articles_Export_ToExcel(int page, string orderBy, string filter)
        {
            var tempArticles = ItemManager.Instance.GetPersonCities();

            //Create new Excel Workbook
            var workbook = new HSSFWorkbook();

            //Create new Excel Sheet
            var sheet = workbook.CreateSheet();

            var row0 = sheet.CreateRow(0);

            HSSFFont myFont = (HSSFFont)workbook.CreateFont();
            myFont.FontHeightInPoints = (short)12;
            myFont.FontName = "Times New Roman";

            HSSFCellStyle borderedCellStyle = (HSSFCellStyle)workbook.CreateCellStyle();
            borderedCellStyle.SetFont(myFont);
            borderedCellStyle.WrapText = true;
            borderedCellStyle.VerticalAlignment = VerticalAlignment.Center;

            HSSFCellStyle borderedCellStyleContent = (HSSFCellStyle)workbook.CreateCellStyle();
            borderedCellStyleContent.SetFont(myFont);
            borderedCellStyleContent.WrapText = true;
            //borderedCellStyleContent.Alignment = HorizontalAlignment.Center; //chieu ngang
            borderedCellStyleContent.VerticalAlignment = VerticalAlignment.Center;

            //(Optional) set the width of the columns
            sheet.SetColumnWidth(0, 2500);//Title
            sheet.SetColumnWidth(1, 2500);//Section
            sheet.SetColumnWidth(2, 2500);//Views
            sheet.SetColumnWidth(3, 2500);
            //sheet.SetColumnWidth(4, 2500);
            //sheet.SetColumnWidth(5, 2500);
            //sheet.SetColumnWidth(6, 256 * 40);

            //Create a header row

            var headerRow = sheet.CreateRow(0);
            headerRow.CreateCell(0).SetCellValue("IdCity");
            headerRow.CreateCell(1).SetCellValue("CityName");
            headerRow.CreateCell(2).SetCellValue("Gender");
            headerRow.CreateCell(3).SetCellValue("Amount");
            //headerRow.CreateCell(4).SetCellValue("Gender");
            //headerRow.CreateCell(5).SetCellValue("Age");
            //headerRow.CreateCell(6).SetCellValue("Desciptions");

            for (int c = 0; c < headerRow.Cells.Count; c++)
            {
                headerRow.Cells[c].CellStyle = borderedCellStyle;
            }

            //(Optional) freeze the header row so it is not scrolled
            sheet.CreateFreezePane(0, 1, 0, 1);

            int rowNumber = 1;

            //Populate the sheet with values from the grid data

            foreach (var objOrg in tempArticles)
            {
                //Create a new Row
                var row = sheet.CreateRow(rowNumber++);

                //Set the Values for Cells
                
                row.CreateCell(0).SetCellValue(objOrg.IdCity);
                row.CreateCell(1).SetCellValue(objOrg.CityName);
                row.CreateCell(2).SetCellValue(objOrg.Gender);
                row.CreateCell(3).SetCellValue(objOrg.Amount);
                //row.CreateCell(4).SetCellValue(objOrg.Gender);
                //row.CreateCell(5).SetCellValue(objOrg.Age);
                //row.CreateCell(6).SetCellValue(objOrg.Desciptions);

                for (int c = 0; c < row.Cells.Count; c++)
                {
                    row.Cells[c].CellStyle = borderedCellStyleContent;
                }
            }


            //Write the Workbook to a memory stream
            MemoryStream output = new MemoryStream();
            workbook.Write(output);

            //Return the result to the end user
            return File(output.ToArray(),   //The binary data of the XLS file
             "application/vnd.ms-excel",//MIME type of Excel files
             "OrganizationList.xls");    //Suggested file name in the "Save as" dialog which will be displayed to the end user
        }


    }
}
