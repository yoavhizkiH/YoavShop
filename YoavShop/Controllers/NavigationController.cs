using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YoavShop.ViewModels;

namespace YoavShop.Controllers
{
    public class NavigationController : Controller
    {
        public ActionResult Menu()
        {
            var menuViewModel = new List<MenuViewModel>
            {
                new MenuViewModel
                {
                    MenuID = 1,
                    Action = "Index",
                    Controller = "Home",
                    IsAction = true,
                    Class = "class",
                    Title = "Home"
                },
                new MenuViewModel
                {
                    MenuID = 2,
                    Action = "About",
                    Controller = "Home",
                    IsAction = true,
                    Class = "class",
                    Title = "About"
                },
                new MenuViewModel
                {
                    MenuID = 3,
                    Action = "Contact",
                    Controller = "Home",
                    IsAction = true,
                    Class = "class",
                    Title = "Contact"
                }
            };

            List<MenuViewModel> mvAdmin = new List<MenuViewModel>()
            {
                new MenuViewModel()
                {
                    MenuID = 3,
                    Action = "Index",
                    Controller = "Customer",
                    IsAction = true,
                    Class = "class",
                    Title = "Customers"
                },
                new MenuViewModel()
                {
                    MenuID = 4,
                    Action = "Index",
                    Controller = "Supplier",
                    IsAction = true,
                    Class = "class",
                    Title = "Suppliers"
                },
                new MenuViewModel()
                {
                    MenuID = 5,
                    Action = "Index",
                    Controller = "Product",
                    IsAction = true,
                    Class = "class",
                    Title = "Products"
                },
                new MenuViewModel()
                {
                    MenuID = 6,
                    Action = "Index",
                    Controller = "ProductCategorie",
                    IsAction = true,
                    Class = "class",
                    Title = "Product Categories"
                },
                new MenuViewModel()
                {
                    MenuID = 7,
                    Action = "Index",
                    Controller = "Transaction",
                    IsAction = true,
                    Class = "class",
                    Title = "Transactions"
                }
            };

            switch (GlobalVariables.Role)
            {
                case "Admin":
                    menuViewModel.AddRange(mvAdmin);
                    break;
                case "Customer":
                    menuViewModel.Add(new MenuViewModel()
                    {
                        MenuID = 8,
                        Action = "Details",
                        Controller = "Customer",
                        IsAction = true,
                        Class = "class",
                        Title = GlobalVariables.StoreUser.UserName
                    });
                    break;
                case "Supplier":
                    menuViewModel.Add(new MenuViewModel()
                    {
                        MenuID = 8,
                        Action = "Details",
                        Controller = "Supplier",
                        IsAction = true,
                        Class = "class",
                        Title = GlobalVariables.StoreUser.UserName
                    });
                    break;
            }

            if (GlobalVariables.Role == "")
            {
                menuViewModel.Add(new MenuViewModel()
                {
                    MenuID = 9,
                    Action = "LogIn",
                    Controller = "Home",
                    IsAction = true,
                    Class = "class",
                    Title = "Log in"
                });
            }
            else
            {
                menuViewModel.Add(new MenuViewModel()
                {
                    MenuID = 9,
                    Action = "LogOff",
                    Controller = "Home",
                    IsAction = true,
                    Class = "class",
                    Title = "Log Off"
                });
            }

            return PartialView("_Navigation", menuViewModel);
        }
    }
}