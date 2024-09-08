using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShelfWise.DataAccess.Repository.IRepository;
using ShelfWise.Models;
using ShelfWise.Models.ViewModels;
using ShelfWise.Utils;

namespace WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = StaticDetails.Role_Admin)]
    public class UserController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public UserController(IUnitOfWork unitOfWork, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult RoleManagement(string userId)
        {
            RoleManagementViewModel roleVM = new()
            {
                ApplicationUser = _unitOfWork.ApplicationUser.Get(u => u.Id == userId, includeProperties: "Company"),
                RoleList = _roleManager.Roles.Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Name
                }).ToList(),
                CompanyList = _unitOfWork.Company.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }).ToList()
            };
            roleVM.ApplicationUser.Role = _userManager.GetRolesAsync(_unitOfWork.ApplicationUser.Get(u => u.Id == userId)).GetAwaiter().GetResult().FirstOrDefault();
            return View(roleVM);
        }

        [HttpPost]
        public IActionResult RoleManagement(RoleManagementViewModel roleVM)
        {
            string oldRole = _userManager.GetRolesAsync(_unitOfWork.ApplicationUser.Get(u => u.Id == roleVM.ApplicationUser.Id)).GetAwaiter().GetResult().FirstOrDefault();

            ApplicationUser user = _unitOfWork.ApplicationUser.Get(u => u.Id == roleVM.ApplicationUser.Id);

            if (roleVM.ApplicationUser.Role != oldRole)
            {
                // Role is updated
                if (roleVM.ApplicationUser.Role == StaticDetails.Role_Company)
                {
                    user.CompanyId = roleVM.ApplicationUser.CompanyId;
                }
                if (oldRole == StaticDetails.Role_Company)
                {
                    user.CompanyId = null;
                }
                _unitOfWork.ApplicationUser.Update(user);
                _unitOfWork.Save();

                _userManager.RemoveFromRoleAsync(user, oldRole).GetAwaiter().GetResult();
                _userManager.AddToRoleAsync(user, roleVM.ApplicationUser.Role).GetAwaiter().GetResult();
            }
            else
            {
                if (oldRole == StaticDetails.Role_Company && user.CompanyId != roleVM.ApplicationUser.CompanyId)
                {
                    user.CompanyId = roleVM.ApplicationUser.CompanyId;
                    _unitOfWork.ApplicationUser.Update(user);
                    _unitOfWork.Save();
                }
            }
            return RedirectToAction("Index");
        }

        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            List<ApplicationUser> allUserList = _unitOfWork.ApplicationUser.GetAll(includeProperties: "Company").ToList();
            foreach (var user in allUserList)
            {
                user.Role = _userManager.GetRolesAsync(user).GetAwaiter().GetResult().FirstOrDefault();

                if (user.Company == null)
                {
                    user.Company = new()
                    {
                        Name = "",
                    };
                }
            }
            return Json(new { data = allUserList });
        }


        [HttpPost]
        public IActionResult LockUnlock([FromBody] string id)
        {
            string message;
            var user = _unitOfWork.ApplicationUser.Get(u => u.Id == id);
            if (user == null)
            {
                return Json(new { success = false, message = "Error while Locking/Unlocking" });
            }

            // User is locked
            if (user.LockoutEnd != null && user.LockoutEnd > DateTime.Now)
            {
                user.LockoutEnd = DateTime.Now;
                message = "UnLock successfully";
            }
            else
            {
                user.LockoutEnd = DateTime.Now.AddYears(6969);
                message = "Lock successfully";
            }
            _unitOfWork.ApplicationUser.Update(user);
            _unitOfWork.Save();
            return Json(new { success = true, message = message });
        }
        #endregion

    }
}
