using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Net;
using System.Security.Principal;
using System.Threading;

namespace HttpConsumer.Sample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasicAuthController : ControllerBase
    {
        [BasicAuthentication]
        public User Index()
        {
            return new User { Name1 = "Nilesh" };
        }
    }
}

public class BasicAuthenticationAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var actionContext = context.HttpContext;
        var authToken = actionContext.Request.Headers["Authorization"];

        // decoding authToken we get decode value in 'Username:Password' format  
        var decodeauthToken = System.Text.Encoding.UTF8.GetString(
            Convert.FromBase64String(authToken));

        // spliting decodeauthToken using ':'   
        var arrUserNameandPassword = decodeauthToken.Split(':');

        // at 0th postion of array we get username and at 1st we get password  
        if (IsAuthorizedUser(arrUserNameandPassword[0], arrUserNameandPassword[1]))
        {
            // setting current principle  
            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(arrUserNameandPassword[0]), null);
        }
        else
        {
            actionContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
        }
    }

    public static bool IsAuthorizedUser(string Username, string Password)
    {   
        return Username == "nilesh" && Password == "demo";
    }
}
