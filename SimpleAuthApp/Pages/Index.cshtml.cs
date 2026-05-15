using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SimpleAuth.Model;

namespace SimpleAuthApp.Pages;

public class IndexModel : PageModel
{
    public async Task<IActionResult> OnGet()
    {
        var r1 = await HttpContext.AuthenticateAsync(AuthConsts.AuthenticationType);
        var r2 = await HttpContext.AuthenticateAsync(AuthConsts.AuthenticationType2);

        var isLogin = r1.Succeeded || r2.Succeeded;
        
        if (!isLogin)
        {
            return RedirectToPage("/Account/Login");
        }
        
        return Page();
    }
}