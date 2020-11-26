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

using System.Collections.Generic;
using DotNetNuke.Data;
using DotNetNuke.Framework;
using Christoc.Modules.SettingsChart2.Models;
using System;

namespace Christoc.Modules.SettingsChart2.Components
{
    interface IItemManager
    {
        void CreateItem(Item t);
        void DeleteItem(int itemId, int moduleId);
        void DeleteItem(Item t);
        IEnumerable<Item> GetItems(int moduleId);
        Item GetItem(int itemId, int moduleId);
        void UpdateItem(Item t);
        IEnumerable<GetCityAll> Cities();
        IEnumerable<GetField> GetFields();
        IEnumerable<GetPerson> GetPersons(int Id);
        IEnumerable<ModuleSettings> GetSettings();
        IEnumerable<GetChart> GetCharts();
        IEnumerable<GetPerson> GetPeople();
        IEnumerable<GetPersonCity> GetPersonCities();
    }

    class ItemManager : ServiceLocator<IItemManager, ItemManager>, IItemManager
    {
        public void CreateItem(Item t)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<Item>();
                rep.Insert(t);
            }
        }

        public void DeleteItem(int itemId, int moduleId)
        {
            var t = GetItem(itemId, moduleId);
            DeleteItem(t);
        }

        public void DeleteItem(Item t)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<Item>();
                rep.Delete(t);
            }
        }

        public IEnumerable<Item> GetItems(int moduleId)
        {
            IEnumerable<Item> t;
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<Item>();
                t = rep.Get(moduleId);
            }
            return t;
        }

        public Item GetItem(int itemId, int moduleId)
        {
            Item t;
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<Item>();
                t = rep.GetById(itemId, moduleId);
            }
            return t;
        }

        public void UpdateItem(Item t)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<Item>();
                rep.Update(t);
            }
        }

        protected override System.Func<IItemManager> GetFactory()
        {
            return () => new ItemManager();
        }

        public IEnumerable<GetCityAll> Cities()
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                return ctx.ExecuteQuery<GetCityAll>(System.Data.CommandType.StoredProcedure, String.Format("Sp_GetCiTy"));
            }
        }

        public IEnumerable<GetField> GetFields()
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                return ctx.ExecuteQuery<GetField>(System.Data.CommandType.StoredProcedure, String.Format("Sp_GetFeild"));
            }
        }

        public IEnumerable<GetPerson> GetPersons(int Id)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                return ctx.ExecuteQuery<GetPerson>(System.Data.CommandType.StoredProcedure, String.Format("Sp_GetPersonByIdCity {0}", Id));
            }
        }

        public IEnumerable<ModuleSettings> GetSettings()
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                return ctx.ExecuteQuery<ModuleSettings>(System.Data.CommandType.StoredProcedure, String.Format("Sp_GetSettingsChart2"));
            }
        }

        public IEnumerable<GetChart> GetCharts()
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                return ctx.ExecuteQuery<GetChart>(System.Data.CommandType.StoredProcedure, String.Format("Sp_GetGenderByCity"));
            }

        }

        public IEnumerable<GetPerson> GetPeople()
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                return ctx.ExecuteQuery<GetPerson>(System.Data.CommandType.StoredProcedure, String.Format("Sp_GetPersons"));
            }
        }

        public IEnumerable<GetPersonCity> GetPersonCities()
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                return ctx.ExecuteQuery<GetPersonCity>(System.Data.CommandType.StoredProcedure, String.Format("Sp_GetGenderByCity"));
            }
        }

    }
}
