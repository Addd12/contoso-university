﻿using ContosoUniversity.Identity;
using ContosoUniversity.Web.Controllers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System;
using Xunit;
using Xunit.Abstractions;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using ContosoUniversity.Web.ViewModels;
using ContosoUniversity.Web.Enums;

namespace ContosoUniversity.Web.Tests.Controllers
{
    public class ManageControllerTests
    {
        private readonly ITestOutputHelper _output;
        private readonly FakeUserManager _fakeUserManager;
        private readonly FakeSignInManager _fakeSignInManager;
        ManageController _sut;

        public ManageControllerTests(ITestOutputHelper output)
        {
            _output = output;
            _fakeUserManager = new FakeUserManager();
            _fakeSignInManager = new FakeSignInManager();
            _sut = new ManageController(_fakeUserManager, _fakeSignInManager);
        }

        [Fact]
        public async Task Index_ReturnsAViewResult()
        {
            var context = new Mock<HttpContext>();
            _sut.ControllerContext = new ControllerContext();
            _sut.ControllerContext.HttpContext = new DefaultHttpContext();

            var result = await _sut.Index();

            Assert.IsType(typeof(ViewResult), result);
        }

        [Fact]
        public void ChangePassword_ReturnAViewResult()
        {
            var result = _sut.ChangePassword();

            Assert.IsType(typeof(ViewResult), result);
        }

        [Fact]
        public async Task ChangePasswordPost_ReturnAViewResult_WithInvalidModel()
        {
            var model = new ChangePasswordViewModel
            {
                OldPassword = "abc",
                NewPassword = "bcd",
                ConfirmPassword = "bcd"
            };
            _sut.ModelState.AddModelError("mymodelerror", "my model error message");

            var result = await _sut.ChangePassword(model);

            Assert.IsType(typeof(ViewResult), result);
            var modelState = ((ViewResult)result).ViewData.ModelState;
            Assert.True(modelState.ContainsKey("mymodelerror"));
        }

        [Fact]
        public async Task ChangePasswordPost_ReturnARedirectToActionResult_WithChangePasswordSuccess()
        {
            var model = new ChangePasswordViewModel
            {
                OldPassword = "abc",
                NewPassword = "bcd",
                ConfirmPassword = "bcd"
            };
            var context = new Mock<HttpContext>();
            _sut.ControllerContext = new ControllerContext();
            _sut.ControllerContext.HttpContext = new DefaultHttpContext();

            var result = await _sut.ChangePassword(model);

            Assert.IsType(typeof(RedirectToActionResult), result);
            Assert.Equal("Index", ((RedirectToActionResult)result).ActionName);
            Assert.Equal(ManageMessage.ChangePasswordSuccess, ((RedirectToActionResult)result).RouteValues["Message"]);
        }

        public class FakeUserManager : UserManager<ApplicationUser>
        {
            public FakeUserManager()
                : base(new Mock<IUserStore<ApplicationUser>>().Object,
                      new Mock<IOptions<IdentityOptions>>().Object,
                      new Mock<IPasswordHasher<ApplicationUser>>().Object,
                      new IUserValidator<ApplicationUser>[0],
                      new IPasswordValidator<ApplicationUser>[0],
                      new Mock<ILookupNormalizer>().Object,
                      new Mock<IdentityErrorDescriber>().Object,
                      new Mock<IServiceProvider>().Object,
                      new Mock<ILogger<UserManager<ApplicationUser>>>().Object)
            { }

            public override Task<ApplicationUser> GetUserAsync(ClaimsPrincipal principal)
            {
                return Task.FromResult(new ApplicationUser { UserName = "test@example.com" }); // base.GetUserAsync(principal);
            }

            public override Task<bool> HasPasswordAsync(ApplicationUser user)
            {
                return Task.FromResult(true);
            }

            public override Task<IdentityResult> ChangePasswordAsync(ApplicationUser user, string currentPassword, string newPassword)
            {
                return Task.FromResult(IdentityResult.Success);
            }
        }

        public class FakeSignInManager : SignInManager<ApplicationUser>
        {
            public FakeSignInManager()
                : base(new FakeUserManager(),
                      new HttpContextAccessor { HttpContext = new Mock<HttpContext>().Object },
                      new Mock<IUserClaimsPrincipalFactory<ApplicationUser>>().Object,
                      null,
                      null)
            { }

            public override Task SignInAsync(ApplicationUser user, bool isPersistent, string authenticationMethod = null)
            {
                return Task.FromResult(0);
            }
        }
    }
}