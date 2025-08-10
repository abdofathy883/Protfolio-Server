using Core.DTOs;
using Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Admin_Dashboard.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        private readonly UserManager<AppUser> _users;
        private readonly RoleManager<IdentityRole> _roles;

        public UsersController(
            UserManager<AppUser> users, 
            RoleManager<IdentityRole> roles
            )
        {
            _users = users;
            _roles = roles;
        }
        public IActionResult Index()
        {
            var users = _users.Users.ToList();
            return View(users);
        }
        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateUserDTO vm, string confirmPassword)
        {
            if (!ModelState.IsValid) return View(vm);

            if (vm.Password != confirmPassword)
            {
                ModelState.AddModelError("", "Password and confirmation password do not match");
                return View(vm);
            }

            var user = new AppUser
            {
                UserName = vm.Email,
                Email = vm.Email,
                FirstName = vm.FirstName,
                LastName = vm.LastName,
                EmailConfirmed = true
            };

            var result = await _users.CreateAsync(user, vm.Password);
            if (!result.Succeeded)
            {
                foreach (var e in result.Errors) ModelState.AddModelError("", e.Description);
                return View(vm);
            }

            if (!string.IsNullOrEmpty(vm.Role))
                await _users.AddToRoleAsync(user, vm.Role);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateUserDTO userDTO)
        {
            if (!ModelState.IsValid) return View(userDTO);

            var user = await _users.FindByIdAsync(userDTO.Id);
            if (user == null) return NotFound();

            user.Email = userDTO.Email;
            user.UserName = userDTO.Email.Contains("@") ? userDTO.Email.Split('@')[0] : userDTO.Email;
            user.FirstName = userDTO.FirstName;
            user.LastName = userDTO.LastName;

            var updateResult = await _users.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                foreach (var e in updateResult.Errors) ModelState.AddModelError("", e.Description);
                return View(user);
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(string id)
        {
            var user = await _users.FindByIdAsync(id);
            if (user == null) return NotFound();
            return View(user);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await _users.FindByIdAsync(id);
            if (user == null) return NotFound();

            var result = await _users.DeleteAsync(user);
            if (!result.Succeeded)
            {
                foreach (var e in result.Errors) ModelState.AddModelError("", e.Description);
                return View("Delete", user);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
