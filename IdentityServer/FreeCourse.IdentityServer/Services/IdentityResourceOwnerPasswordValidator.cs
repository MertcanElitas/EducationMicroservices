using FreeCourse.IdentityServer.Models;
using IdentityModel;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreeCourse.IdentityServer.Services
{
    public class IdentityResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public IdentityResourceOwnerPasswordValidator(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            var userModel = await _userManager.FindByEmailAsync(context.UserName);

            if (userModel == null)
            {
                var errors = new Dictionary<string, object>() { { "errors", new List<string> { "Email veya Şifre yanlış" } } };
                context.Result.CustomResponse = errors;

                return;
            }

            var passwordCheck = await _userManager.CheckPasswordAsync(userModel, context.Password);

            if (!passwordCheck)
            {
                var errors = new Dictionary<string, object>() { { "errors", new List<string> { "Email veya Şifre yanlış" } } };
                context.Result.CustomResponse = errors;

                return;
            }

            context.Result = new GrantValidationResult(userModel.Id.ToString(), OidcConstants.AuthenticationMethods.Password);
        }
    }
}
