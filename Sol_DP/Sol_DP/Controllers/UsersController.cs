using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Sol_DP.Models;

namespace Sol_DP.Controllers
{
    public class UsersController : Controller
    {
        #region Declaration
        private readonly IDataProtector dataProtector = null;

        #endregion

        #region Constructor
        public UsersController(IDataProtectionProvider provider)
        {
            this.dataProtector = provider.CreateProtector("Enter the Secret Key");
        }

        #endregion

        #region Public Property
        [BindProperty(SupportsGet =true)]
        public string id { get; set; }
        #endregion 

        #region Private Method
        private List<UserModel> GetUserData()
        {
            return new List<UserModel>()
            {
                new UserModel()
                {
                    UserId=1,
                    FirstName="Kishor",
                    LastName="Naik"
                },
                new UserModel()
                {
                    UserId=2,
                    FirstName="Eshaan",
                    LastName="Naik"
                }
            };
        }
        #endregion 

        #region Actions
        [HttpGet]
        public IActionResult Index()
        {
            var listUserData = this.GetUserData();

            foreach(UserModel userModel in listUserData)
            {
                userModel.TokenId = dataProtector.Protect(userModel.UserId.ToString());
            }

            return View(listUserData);
        }


        [HttpGet]
        public IActionResult Details()
        {

            var userId = dataProtector.Unprotect(id);

            var listData = this.GetUserData();

            var userFilter =
                listData
                ?.AsEnumerable()
                ?.FirstOrDefault((leUserModel) => leUserModel.UserId == Convert.ToInt32(userId));

            return View(userFilter);
        }
        #endregion
    }
}