using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DA.DataBase.Models.Departments
{
    public class DepartmentViewModel
    {
        /// <summary>
        /// 部門編號
        /// </summary>
        public int GroupId { get; set; }
        /// <summary>
        /// 群組類別
        /// </summary>
        public string GroupTypeKey { get; set; }
        /// <summary>
        /// 部門編碼
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 部門名稱
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 錯誤訊息
        /// </summary>
        public int Counts { get; set; }
        /// <summary>
        /// 顯示顏色
        /// </summary>
        public string Color { get; set; }
        /// <summary>
        /// 圖示
        /// </summary>
        public string Icon { get; set; }

        public List<MainToolViewModel> MainTools { get; set; }
    }
}
