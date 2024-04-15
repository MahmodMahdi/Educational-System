using AutoMapper;
using BusinessLogicLayer.Services.UserService;
using BusinessLogicLayer.ViewModels;
using DataAccessLayer.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Educational_Website.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        public AccountController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager, IUserService userService, IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _userService = userService;
            _mapper = mapper;
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index(int pageNo = 1)
        {
            var Users = await _userManager.Users.Where(x => x.Role == "Instructor" || x.Role == "Admin" || x.Role == "User").ToListAsync();
            // Pagination
            int NoOfRecordsPerPage = 5;
            int NoOfPage = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(Users.Count) / Convert.ToDouble(NoOfRecordsPerPage)));
            int NoOfRecordsToSkip = (pageNo - 1) * NoOfRecordsPerPage;
            ViewBag.PageNo = pageNo;
            ViewBag.NoOfPages = NoOfPage;
            Users = Users.Skip(NoOfRecordsToSkip).Take(NoOfRecordsPerPage).ToList();
            return View(Users);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterUserViewModel newUserVM)
        {
            if (ModelState.IsValid)
            {
                // Mapping from VM to model
                var userModel = _mapper.Map<ApplicationUser>(newUserVM);
                var users = await _userManager.Users.ToListAsync();
                userModel.Role = "User";
                // save in db
                IdentityResult result = await _userManager.CreateAsync(userModel, newUserVM.Password!);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(userModel, "User");
                    // create cookies
                    await _signInManager.SignInAsync(userModel, false); // Store ID, Name, Roles // false as remember me
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    foreach (var errorItem in result.Errors)
                    {
                        ModelState.AddModelError("Password", errorItem.Description);
                    }
                }
            }
            return View(newUserVM);
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginUserViewModel loginUser)
        {
            if (ModelState.IsValid)
            {
                //check db
                ApplicationUser? userModel = await _userManager.FindByEmailAsync(loginUser.Email);
                if (userModel != null)
                {
                    bool found = await _userManager.CheckPasswordAsync(userModel, loginUser.Password);
                    if (found == true)
                    {
                        //cookie
                        await _signInManager.SignInAsync(userModel, loginUser.RememberMe);
                        return RedirectToAction("Index", "Department");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Password Wrong");
                    }
                }
            }
            return View(loginUser);
        }
        public IActionResult Logout()
        {
            _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult AddAdmin()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddAdmin(RegisterUserViewModel newUserVM)
        {
            if (ModelState.IsValid)
            {
                // Mapping from VM to model
                var userModel = _mapper.Map<ApplicationUser>(newUserVM);
                userModel.Role = "Admin";
                // save in db
                IdentityResult result = await _userManager.CreateAsync(userModel, newUserVM.Password!);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(userModel, "Admin");
                    // create cookies
                    await _signInManager.SignInAsync(userModel, false); // Store ID, Name, Roles // false as remember me
                    return RedirectToAction("Index", "Department");
                }
                else
                {
                    foreach (var errorItem in result.Errors)
                    {
                        ModelState.AddModelError("Password", errorItem.Description);
                    }
                }
            }
            return View(newUserVM);
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult AddInstructor()
        {
            return View();
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddInstructor(RegisterUserViewModel newUserVM)
        {
            if (ModelState.IsValid)
            {
                // Mapping from VM to model
                var userModel = _mapper.Map<ApplicationUser>(newUserVM);
                userModel.Role = "Instructor";
                // save in db
                IdentityResult result = await _userManager.CreateAsync(userModel, newUserVM.Password!);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(userModel, "Instructor");
                    // create cookies
                    await _signInManager.SignInAsync(userModel, false); // Store ID, Name, Roles // false as remember me
                    return RedirectToAction("Index", "Department");
                }
                else
                {
                    foreach (var errorItem in result.Errors)
                    {
                        ModelState.AddModelError("Password", errorItem.Description);
                    }
                }

            }
            return View(newUserVM);
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(ApplicationUser User)
        {
            var item = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == User.Id);
            if (item == null) return NotFound();
            var user = await _userManager.DeleteAsync(item!);
            TempData["successMessageForDeleteUser"] = "User deleted successfully!";
            if (user.Succeeded)
            {
                return RedirectToAction(nameof(Index));
            }
            foreach (var error in user.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            return RedirectToAction(nameof(Index));
        }
        [Authorize]
        public async Task<IActionResult> Search(string searchString)
        {

            var Search = await _userService.SearchUserAsync(searchString);
            ViewBag.Search = searchString;
            return View(Search);
        }
    }
}
