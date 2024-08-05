using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentEducationCenter.Data;
using StudentEducationCenter.Data.Models;
using StudentEducationCenter.ViewModels.User;

namespace StudentEducationCenter.Controllers
{
    [Authorize]
    public class UserController : BaseController
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly IUserStore<User> _userStore;
        private readonly ILogger<LoginViewModel> _logger;

        public UserController(ApplicationDbContext context,
            SignInManager<User> signInManager,
            UserManager<User> userManager,
            IUserStore<User> userStore,
            ILogger<LoginViewModel> logger) : base(context)
        {
            _context = context;
            _signInManager = signInManager;
            _userManager = userManager;
            _userStore = userStore;
            _logger = logger;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Register()
        {
            if (_signInManager.IsSignedIn(User))
            {
                return RedirectToAction("Index", "Home");
            }

            await PutInViewbag();

            return View(new RegisterViewModel());
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (await _context.Users.FirstOrDefaultAsync(x => x.Email == model.Email) != null)
                {
                    ModelState.AddModelError(string.Empty, "Вече има потребител, който е регистриран на този имейл.");
                    await PutInViewbag();
                    return View(model);
                }

                User user = CreateUser();
                user.FirstName = model.FirstName;
                user.MiddleName = model.MiddleName;
                user.LastName = model.LastName;
                user.CityId = model.CityId;
                user.Address = model.Address;
                user.PhoneNumber = model.PhoneNumber;
                user.LockoutEnabled = false;
                user.EmailConfirmed = true;
                var city = await _context.Cities.FirstOrDefaultAsync(x => x.Id == model.CityId);
                if (city != null)
                {
                    user.CityId = city.Id;
                    user.City = city;
                }
                else
                    ModelState.AddModelError(string.Empty, "Невалиден град");

                await _userStore.SetUserNameAsync(user, model.Email, CancellationToken.None);
                _userManager.Options.SignIn.RequireConfirmedAccount = false;
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    Parent parent = new Parent()
                    {
                        UserId = user.Id,
                        User = user,
                    };
                    await _context.Parents.AddAsync(parent);
                    await _context.SaveChangesAsync();
                    await _userManager.AddToRoleAsync(user, "Parent");
                    await _signInManager.SignInAsync(user, isPersistent: false);
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return RedirectToAction("Index", "Home");
            }
            await PutInViewbag();

            return View(model);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RegisterStaff()
        {
            if (_signInManager.IsSignedIn(User) && User.IsInRole("Admin"))
            {
                await PutInViewbag();
                await SortedCitiesInViewBag();

                var model = new RegisterStaffViewModel();
                return View(model);
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RegisterStaff(RegisterStaffViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (await _context.Users.FirstOrDefaultAsync(x => x.Email == model.Email) != null)
                {
                    ModelState.AddModelError(string.Empty, "Вече има потребител, който е регистриран на този имейл.");
                    await PutInViewbag();
                    return View(model);
                }

                User user = CreateUser();
                user.FirstName = model.FirstName;
                user.MiddleName = model.MiddleName;
                user.LastName = model.LastName;
                user.CityId = model.CityId;
                user.Address = model.Address;
                user.PhoneNumber = model.PhoneNumber;
                user.Email = model.Email;
                user.NormalizedEmail = model.Email.ToUpper();
                user.LockoutEnabled = false;
                user.EmailConfirmed = true;
                var city = await _context.Cities.FirstOrDefaultAsync(x => x.Id == model.CityId);
                if (city != null)
                {
                    user.CityId = city.Id;
                    user.City = city;
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Невалиден град");
                    await PutInViewbag();
                    return View(model);
                }



                await _userStore.SetUserNameAsync(user, model.Email, CancellationToken.None);
                _userManager.Options.SignIn.RequireConfirmedAccount = false;
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    if (model.Roles == "Teacher")
                    {
                        if (model.SpecialtyIds.Count <= 0)
                        {
                            ModelState.AddModelError(string.Empty, "За учител трябва да бъде избрана поне една специалност.");
                            await PutInViewbag();
                            return View(model);
                        }
                        await _userManager.AddToRoleAsync(user, model.Roles);
                        Teacher teacher = new Teacher(user);
                        await _context.Teachers.AddAsync(teacher);

                        foreach (var specialtyId in model.SpecialtyIds)
                        {
                            var s = await _context.Specialties.FirstOrDefaultAsync(x => x.Id == specialtyId);
                            if (s != null)
                            {
                                TeacherSpecialty ts = new TeacherSpecialty()
                                {
                                    TeacherId = teacher.Id,
                                    Teacher = teacher,
                                    SpecialtyId = s.Id,
                                    Specialty = s
                                };
                                teacher.TeacherSpecialties.Add(ts);
                                await _context.TeachersSpecialties.AddAsync(ts);

                            }
                        }
                        await _context.SaveChangesAsync();
                    }
                    else if (model.Roles == "Employee")
                    {
                        await _userManager.AddToRoleAsync(user, model.Roles);
                        Employee employee = new Employee(user);
                        await _context.Employees.AddAsync(employee);
                        await _context.SaveChangesAsync();
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                return RedirectToAction("Index", "Home");
            }
            return View(model);
        }

        [AllowAnonymous]
        public IActionResult Login()
        {
            if (!_signInManager.IsSignedIn(User))
            {
                return View(new LoginViewModel());
            }
            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _context.Users.FirstOrDefault(x => x.UserName == model.Email || x.Email == model.Email);
                if (user == null || user.UserName == null)
                {
                    ModelState.AddModelError(string.Empty, "Невалидни имейл или парола!");
                    return View(model);
                }

                var result = await _signInManager.PasswordSignInAsync(user.UserName, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User logged in.");
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Невалидни имейл или парола!");
                    return View(model);
                }
            }
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> DirectEdit()
        {
            int id = 0;
            string role = "";

            var teacher = await _context.Teachers
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.User.Id == GetUserId());
            if (teacher != null)
            {
                id = teacher.Id;
                role = "Teacher";
            }

            var employee = await _context.Employees
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.User.Id == GetUserId());
            if (employee != null)
            {
                id = employee.Id;
                role = "Employee";
            }

            var parent = await _context.Parents
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.User.Id == GetUserId());
            if (parent != null)
            {
                id = parent.Id;
                role = "Parent";
            }

            return RedirectToAction("Edit", new { id = id, role = role });
        }

        [Authorize(Roles = "Admin, Parent, Employee, Teacher")]
        public async Task<IActionResult> Edit(int id, string role)
        {
            if (id == 0 || role == "")
            {
                return RedirectToAction("Index", "Home");
            }
            await PutInViewbag();
            if (role == "Teacher")
            {
                var teacher = await _context.Teachers
                    .Include(x => x.TeacherSpecialties)
                    .ThenInclude(x => x.Specialty)
                    .Include(x => x.User)
                    .ThenInclude(x => x.City)
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (teacher != null
                    && (User.IsInRole("Admin") || teacher.User.Id == GetUserId())
                    && teacher.User.PhoneNumber != null)

                {
                    EditUserViewModel model = new EditUserViewModel
                    {
                        Role = role,
                        Id = teacher.Id,
                        FirstName = teacher.User.FirstName,
                        MiddleName = teacher.User.MiddleName,
                        LastName = teacher.User.LastName,
                        CityId = teacher.User.CityId,
                        Address = teacher.User.Address,
                        PhoneNumber = teacher.User.PhoneNumber,
                        SpecialtyIds = teacher.TeacherSpecialties.Select(x => x.SpecialtyId).ToList(),
                    };
                    return View(model);
                }
            }
            else if (role == "Employee")
            {
                var employee = await _context.Employees
                    .Include(x => x.User)
                    .ThenInclude(x => x.City)
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (employee != null
                    && (User.IsInRole("Admin") || employee.User.Id == GetUserId())
                    && employee.User.PhoneNumber != null)

                {
                    EditUserViewModel model = new EditUserViewModel
                    {
                        Role = role,
                        Id = employee.Id,
                        FirstName = employee.User.FirstName,
                        MiddleName = employee.User.MiddleName,
                        LastName = employee.User.LastName,
                        CityId = employee.User.CityId,
                        Address = employee.User.Address,
                        PhoneNumber = employee.User.PhoneNumber,
                    };
                    return View(model);
                }
            }
            else if (role == "Parent")
            {
                var parent = await _context.Parents
                    .Include(x => x.User)
                    .ThenInclude(x => x.City)
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (parent != null
                    && (User.IsInRole("Admin") || parent.User.Id == GetUserId())
                    && parent.User.PhoneNumber != null)
                {
                    EditUserViewModel model = new EditUserViewModel
                    {
                        Role = role,
                        Id = parent.Id,
                        FirstName = parent.User.FirstName,
                        MiddleName = parent.User.MiddleName,
                        LastName = parent.User.LastName,
                        CityId = parent.User.CityId,
                        Address = parent.User.Address,
                        PhoneNumber = parent.User.PhoneNumber,
                    };
                    return View(model);
                }
            }
            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Edit(EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                User? user = null;

                if (model.Role == "Employee")
                {
                    var employee = await _context.Employees
                        .Include(x => x.User)
                        .FirstOrDefaultAsync(x => x.Id == model.Id);
                    if (employee != null)
                    {
                        user = employee.User;
                    }
                }
                else if (model.Role == "Parent")
                {
                    var parent = await _context.Parents
                        .Include(x => x.User)
                        .FirstOrDefaultAsync(x => x.Id == model.Id);
                    if (parent != null)
                    {
                        user = parent.User;
                    }
                }
                else if (model.Role == "Teacher")
                {
                    var teacher = await _context.Teachers
                        .Include(x => x.User)
                        .Include(x => x.TeacherSpecialties)
                        .ThenInclude(x => x.Specialty)
                        .FirstOrDefaultAsync(x => x.Id == model.Id);
                    if (teacher != null)
                    {
                        user = teacher.User;
                        var teacherSpecialties = teacher.TeacherSpecialties.ToList();
                        if (model.SpecialtyIds.Count <= 0)
                        {
                            await PutInViewbag();
                            ModelState.AddModelError(string.Empty, "За учителя трябва да е избрана поне една специалност!");
                            return View(model);
                        }

                        foreach (var ts in teacherSpecialties)
                        {
                            var specialty = await _context.Specialties.FirstOrDefaultAsync(x => x == ts.Specialty);
                            if (specialty != null)
                            {
                                specialty.TeachersSpecialty.Remove(ts);
                            }
                            teacher.TeacherSpecialties.Remove(ts);
                            _context.TeachersSpecialties.Remove(ts);
                        }
                        foreach (var specialtyId in model.SpecialtyIds)
                        {
                            var specialty = await _context.Specialties.FirstOrDefaultAsync(x => x.Id == specialtyId);
                            if (specialty != null)
                            {
                                var tc = new TeacherSpecialty()
                                {
                                    SpecialtyId = specialtyId,
                                    Specialty = specialty,
                                    TeacherId = teacher.Id,
                                    Teacher = teacher
                                };
                                await _context.TeachersSpecialties.AddAsync(tc);
                                specialty.TeachersSpecialty.Add(tc);
                                teacher.TeacherSpecialties.Add(tc);
                            }
                        }
                    }

                }

                if (user != null)
                {
                    user.FirstName = model.FirstName;
                    user.MiddleName = model.MiddleName;
                    user.LastName = model.LastName;
                    user.Address = model.Address;
                    var city = await _context.Cities.FirstOrDefaultAsync(x => x.Id == model.CityId);
                    if (city == null)
                    {
                        ModelState.AddModelError(string.Empty, "Невалиден град");
                        await PutInViewbag();
                        return View(model);
                    }
                    user.CityId = city.Id;
                    user.City = city;
                    user.PhoneNumber = model.PhoneNumber;

                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index", "Home");
                }
            }
            await PutInViewbag();
            ModelState.AddModelError(string.Empty, "Невалиден потребител");
            return View(model);
        }

        private User CreateUser()
        {
            try
            {
                return Activator.CreateInstance<User>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(User)}'. " +
                    $"Ensure that '{nameof(User)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }
        private async Task PutInViewbag()
        {
            await SortedCitiesInViewBag();

            ViewBag.Specialties = await _context.Specialties.OrderBy(x => x.Name).Select(x => new
            {
                SpecialtyId = x.Id,
                SpecialtyName = x.Name
            }).ToListAsync();
        }
    }
}