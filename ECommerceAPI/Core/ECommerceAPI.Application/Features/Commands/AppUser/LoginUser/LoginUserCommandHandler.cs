using ECommerceAPI.Application.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ECommerceAPI.Application.Features.Commands.AppUser.LoginUser;

public class LoginUserCommandHandler : IRequestHandler<LoginUserCommandRequest, LoginUserCommandResponse>
{
    readonly UserManager<Domain.Entities.Identity.AppUser>  _userManager ;
    readonly SignInManager<Domain.Entities.Identity.AppUser> _signInManager;

    public LoginUserCommandHandler(UserManager<Domain.Entities.Identity.AppUser> userManager, SignInManager<Domain.Entities.Identity.AppUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }


    public async Task<LoginUserCommandResponse> Handle(LoginUserCommandRequest request, CancellationToken cancellationToken)
    {
        // Giriş yaparken Username kullandıysa
        Domain.Entities.Identity.AppUser? user = await _userManager.FindByNameAsync(request.UsernameOrEmail);
        // Giriş yaparken Email kullandıysa
        if(user == null)
            user = await _userManager.FindByEmailAsync(request.UsernameOrEmail);
        
        if(user == null)
            throw new UserNotFoundException();
        
        SignInResult result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
        if (result.Succeeded)
        {
            // Yetkiler belirlenecek
        }

        return new();
    }
}