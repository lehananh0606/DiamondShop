using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using Service.Commons;
using Service.IServices;
using Service.Utils;
using Service.ViewModels.Response.User;
using ShopRepository.Enums;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DiamondShopSystem.Authorization

{
    public class PermissionAuthorizeAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        private string[] _roles;
        public PermissionAuthorizeAttribute(params string[] roles)
        {
            this._roles = roles;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {

            if (context.HttpContext.User.Identity.IsAuthenticated)
            {
                IUserService accountService = context.HttpContext.RequestServices.GetService<IUserService>();
                var currentController = context.RouteData.Values["controller"];
                var currentActionName = context.RouteData.Values["action"];
                string email = context.HttpContext.User.Claims.First(x => x.Type.ToLower() == ClaimTypes.Email).Value;
                string accountId = context.HttpContext.User.Claims.First(x => x.Type.ToLower() == JwtRegisteredClaimNames.Sid).Value;

                UserResponse existedAccount = accountService.GetAccountAsync(int.Parse(accountId), context.HttpContext.User.Claims).Result;
                //if (existedAccount.IsConfirmed == false && currentController.ToString().ToLower().Equals("accounts") && currentActionName.ToString().ToLower().Equals("updateaccount") ||
                //    existedAccount.IsConfirmed == false && currentController.ToString().ToLower().Equals("stores") && currentActionName.ToString().ToLower().Equals("getstoreprofile"))
                //{
                //    return;
                //}

                //if (existedAccount.IsConfirmed == false)
                //{
                //    context.Result = new ObjectResult("Unauthorized")
                //    {
                //        StatusCode = 401,
                //        Value = new
                //        {
                //            Message = JsonConvert.DeserializeObject<List<ErrorDetail>>(ErrorUtil.GetErrorString("Unauthorized", "You have not changed your password for the first time after registering. " +
                //                                                                                                                "Please change the new password before using this function."))
                //        }
                //    };
                //}

                bool isActiveAccount = existedAccount.Status.ToLower().Equals(CustomerStatus.Status.ACTIVE.ToString().ToLower()) ? true : false;

                if (isActiveAccount == false)
                {
                    context.Result = new ObjectResult("Unauthorized")
                    {
                        StatusCode = 401,
                        Value = new
                        {
                            Message = JsonConvert.DeserializeObject<List<ErrorDetail>>(ErrorUtil.GetErrorString("Unauthorized", "You are not allowed to access this API."))
                        }
                    };
                }
                var expiredClaim = long.Parse(context.HttpContext.User.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Exp).Value);
                var expiredDate = DateUtil.ConvertUnixTimeToDateTime(expiredClaim);
                if (expiredDate <= DateTime.UtcNow)
                {
                    context.Result = new ObjectResult("Unauthorized")
                    {
                        StatusCode = 401,
                        Value = new
                        {
                            Message = JsonConvert.DeserializeObject<List<ErrorDetail>>(ErrorUtil.GetErrorString("Unauthorized", "You are not allowed to access this API."))
                        }
                    };
                }
                else
                {
                    var roleClaim = context.HttpContext.User.Claims.FirstOrDefault(x => x.Type.ToLower() == "role");
                    if (this._roles.FirstOrDefault(x => x.ToLower().Equals(roleClaim.Value.ToLower())) == null)
                    {
                        context.Result = new ObjectResult("Forbidden")
                        {
                            StatusCode = 403,
                            Value = new
                            {
                                Message = JsonConvert.DeserializeObject<List<ErrorDetail>>(ErrorUtil.GetErrorString("Forbidden", "You are not allowed to access this function!"))
                            }
                        };
                    }
                    else
                    {
                        return;
                    }
                }
            }
            else
            {
                context.Result = new ObjectResult("Unauthorized")
                {
                    StatusCode = 401,
                    Value = new
                    {
                        Message = JsonConvert.DeserializeObject<List<ErrorDetail>>(ErrorUtil.GetErrorString("Unauthorized", "You are not allowed to access this API."))
                    }
                };
            }
        }
    }
}
