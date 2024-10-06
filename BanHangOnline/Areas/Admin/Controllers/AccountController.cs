using BanHangOnline.Models;
using BanHangOnline.Models.EF;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;


namespace BanHangOnline.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AccountController : Controller
    {
        // GET: Admin/Account
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private ApplicationDbContext db = new ApplicationDbContext();
        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public ActionResult Index(string searchText, int? page)
        {
            var pageSize = 10;
            if (page == null)
            {
                page = 1;
            }

            // Kiểm tra db.Users có null hay không
            if (db.Users == null)
            {
                return HttpNotFound("Users not found in the database.");
            }

            IEnumerable<ApplicationUser> items = db.Users.OrderByDescending(x => x.Id);

            if (!string.IsNullOrEmpty(searchText))
            {
                // Kiểm tra từng thuộc tính có null hay không trước khi sử dụng Contains
                items = items.Where(x => (x.UserName != null && x.UserName.Contains(searchText)) ||
                                         (x.Phone != null && x.Phone.Contains(searchText)) ||
                                         (x.Email != null && x.Email.Contains(searchText)));
            }

            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            var pagedList = items.ToPagedList(pageIndex, pageSize);
            ViewBag.size = pageSize;
            ViewBag.page = pageIndex;
            return View(pagedList);
        }


        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Create(string id)
        {
            ViewBag.Role = new SelectList(db.Roles.ToList(), "Name", "Name");
            if (id != null)
            {
                var user = db.Users.FirstOrDefault(x => x.Id == id);
                if (user == null)
                {
                    return HttpNotFound();
                }

                // Chuyển đổi từ ApplicationUser sang CreateAccountViewModel
                var model = new CreateAccountViewModel
                {
                    Username = user.UserName,
                    FullName = user.Fullname, // Nếu có thuộc tính FullName trong ApplicationUser
                    Phone = user.PhoneNumber,
                    Email = user.Email,
                    Role = user.Roles.FirstOrDefault()?.RoleId // hoặc RoleName tùy theo cấu trúc
                };

                return View(model);
            }
            return View();
        }


        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreateAccountViewModel model, string id)
        {
            if(id != null)
            {
                //id sẽ là 1 chuỗi 713cecc2-bbcb-49c8-a952-697ac6e7c883
                //up date thông tin người dùng gồm username, full name, phone, role được lấy ra thừ model
                // Lấy thông tin người dùng hiện tại từ cơ sở dữ liệu
                var user = await UserManager.FindByIdAsync(id);
                if (user == null)
                {
                    return HttpNotFound();
                }

                // Cập nhật thông tin người dùng
                user.UserName = model.Username;
                user.Fullname = model.FullName;
                user.Phone = model.Phone;

                // Kiểm tra role hiện tại của người dùng
                var currentRoles = await UserManager.GetRolesAsync(user.Id);
                var removeResult = await UserManager.RemoveFromRolesAsync(user.Id, currentRoles.ToArray());
                if (!removeResult.Succeeded)
                {
                    ModelState.AddModelError("", "Lỗi khi xóa vai trò hiện tại của người dùng.");
                    ViewBag.Role = new SelectList(db.Roles.ToList(), "Name", "Name");
                    return View(model);
                }

                // Thêm người dùng vào role mới
                var addResult = await UserManager.AddToRoleAsync(user.Id, model.Role);
                if (!addResult.Succeeded)
                {
                    ModelState.AddModelError("", "Lỗi khi thêm vai trò mới cho người dùng.");
                    ViewBag.Role = new SelectList(db.Roles.ToList(), "Name", "Name");
                    return View(model);
                }

                // Cập nhật người dùng vào cơ sở dữ liệu
                var updateResult = await UserManager.UpdateAsync(user);
                if (updateResult.Succeeded)
                {
                    return RedirectToAction("Index", "Account");
                }
                else
                {
                    AddErrors(updateResult);
                    ViewBag.Role = new SelectList(db.Roles.ToList(), "Name", "Name");
                    return View(model);
                }

            }
            else
            {
                if (ModelState.IsValid)
                {

                    // Kiểm tra xem email đã tồn tại chưa
                    var existingUser = await UserManager.FindByEmailAsync(model.Email);
                    if (existingUser != null)
                    {
                        // Email đã tồn tại, thêm lỗi vào ModelState
                        ModelState.AddModelError("Email", "Email đã được sử dụng, vui lòng chọn một email khác.");
                        ViewBag.Role = new SelectList(db.Roles.ToList(), "Name", "Name");
                        return View(model);
                    }

                    var user = new ApplicationUser { UserName = model.Username, Email = model.Email, Fullname = model.FullName, Phone = model.Phone };
                    var result = await UserManager.CreateAsync(user, model.Password);
                    if (result.Succeeded)
                    {
                        UserManager.AddToRole(user.Id, model.Role);
                        //await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                        // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                        // Send an email with this link
                        ////string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                        ////var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                        ////await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                        return RedirectToAction("Index", "Account");
                    }
                    AddErrors(result);
                }
            }
            ViewBag.Role = new SelectList(db.Roles.ToList(), "Name", "Name");


            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Edit(string id)
        {
            if (id != null)
            {
                var item = db.Users.FirstOrDefault(x => x.Id == id);
                View(item);
            }
            return View();
        }

        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
            }
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }
        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        private void AddErrors(IdentityResult result)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return Json(new { success = false, message = "ID không hợp lệ." });
            }

            var user = await UserManager.FindByIdAsync(id);
            if (user == null)
            {
                return Json(new { success = false, message = "Người dùng không tồn tại." });
            }

            var result = await UserManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false, message = "Xóa người dùng thất bại." });
            }
        }


    }
}