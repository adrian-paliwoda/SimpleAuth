using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SimpleAuth.Model;

namespace SimpleAuthApp.Page.Account;

public class Login : PageModel
{
    [BindProperty]
    public Credential Credential { get; set; }
    
    [BindProperty]
    public bool IsLogin { get; set; }

    public async Task OnGet()
    {
        var r1 = await HttpContext.AuthenticateAsync(AuthConsts.AuthenticationType);
        var r2 = await HttpContext.AuthenticateAsync(AuthConsts.AuthenticationType2);

        IsLogin = r1.Succeeded || r2.Succeeded;
    }

    public async Task<IActionResult> OnPost()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        if (Credential is { UserName: "admin", Password: "admin" })
        {
            var claims = new List<Claim>()
            {
                new(ClaimTypes.Name, Credential.UserName),
                new(ClaimTypes.Upn, Credential.UserName),
                new(ClaimTypes.Email, Credential.UserName + "@test.com"),
            };
            
            var claims2 = new List<Claim>()
            {
                new(ClaimTypes.Name, "TEST 2"),
            };
            
            var claims3 = new List<Claim>()
            {
                new(ClaimTypes.Name, "TEST 3"),
            };

            var identity1 = new ClaimsIdentity(claims, AuthConsts.AuthenticationType);
            var identity2 = new ClaimsIdentity(claims2, AuthConsts.AuthenticationType);
            var identity3 = new ClaimsIdentity(claims3, AuthConsts.AuthenticationType2);
            var identities = new List<ClaimsIdentity>()
            {
                identity1,
                identity2,
                // identity3,
            };
            
            var principal = new ClaimsPrincipal(identities);
            var principal2 = new ClaimsPrincipal(identity3);
            
            await HttpContext.SignInAsync(AuthConsts.AuthenticationType, principal);
            await HttpContext.SignInAsync(AuthConsts.AuthenticationType2, principal2);
            
            var merged = new ClaimsPrincipal(identities);
            merged.AddIdentities(principal2.Identities);
            HttpContext.User = merged; 

            return RedirectToAction("/Index");
        }

        return Page();
    }
    
    public async Task OnPostLogoutAsync()
    {
        await HttpContext.SignOutAsync(AuthConsts.AuthenticationType);
        IsLogin = false;
    }
}