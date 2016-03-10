using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DA.DataBase.Entities
{
    [Table("AppStore")]
    public class AppStore
    {
        #region property
        private int appNo;
        /// <summary>
        /// 
        /// </summary>
        [Key]
        [Required]
        public int AppNo
        {
            get { return this.appNo; }
            set { this.appNo = value; }
        }

        private string appName;
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public string AppName
        {
            get { return appName; }
            set { appName = value; }
        }

        private int parentAppNo;
        
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public int ParentAppNo
        {
            get { return parentAppNo; }
            set { parentAppNo = value; }
        }

        private string typeName;
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public string TypeName
        {
            get { return typeName; }
            set { typeName = value; }
        }

        private string symbol;
        /// <summary>
        /// 
        /// </summary>
        public string Symbol
        {
            get { return symbol; }
            set { symbol = value; }
        }

        private string area;
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public string Area
        {
            get { return area; }
            set { area = value; }
        }

        private string templateUrl;
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public string TemplateUrl
        {
            get { return templateUrl; }
            set { templateUrl = value; }
        }

        private string controllerName;
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public string ControllerName
        {
            get { return controllerName; }
            set { controllerName = value; }
        }

        private string verNo;
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public string VerNo
        {
            get { return verNo; }
            set { verNo = value; }
        }
       
        private Int16 maintainUser;
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public Int16 MaintainUser
        {
            get { return maintainUser; }
            set { maintainUser = value; }
        }

        private Int16 systemUser;
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public Int16 SystemUser
        {
            get { return systemUser; }
            set { systemUser = value; }
        }

        private DateTime systemTime;
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public DateTime SystemTime
        {
            get { return systemTime; }
            set { systemTime = value; }
        }

        #endregion

        public AppStore()
        {
            //AppStores = new HashSet<AppStore>();

        }
        //public ICollection<AppStore> AppStores { get; set; }

    }
}
