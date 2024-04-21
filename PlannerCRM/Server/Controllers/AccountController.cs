using PlannerCRM.Shared.DTOs.EmployeeProfilePictureDto;

namespace PlannerCRM.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly AppDbContext _dbCcontext;

    public AccountController(
        UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager,
        AppDbContext dbContext)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _dbCcontext = dbContext;
    }

    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync(EmployeeLoginDto dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email);

        if (user is null)
        {
            return NotFound(LoginFeedBack.USER_NOT_FOUND);
        }

        if (dto.Email != ConstantValues.ADMIN_EMAIL)
        {
            var employee = await _dbCcontext.Employees
                .SingleAsync(em => em.Email == dto.Email);

            if (employee is null || employee.IsArchived || employee.IsDeleted)
            {
                return NotFound(LoginFeedBack.USER_NOT_FOUND);
            }
        }

        var userPasswordIsCorrect = await _userManager.CheckPasswordAsync(user, dto.Password);

        if (!userPasswordIsCorrect)
        {
            return BadRequest(LoginFeedBack.WRONG_PASSWORD);
        }

        await _signInManager.SignInAsync(user, false);

        return Ok(LoginFeedBack.CONNECTED);
    }

    [Authorize]
    [HttpGet("logout")]
    public async Task LogoutAsync() =>
        await _signInManager.SignOutAsync();

    [HttpGet("user/role")]
    public async Task<string> GetUserRoleAsync()
    {
        if (User.Identity.IsAuthenticated && User is not null)
        {
            var user = await _userManager.FindByEmailAsync(User.Identity.Name);
            var roles = await _userManager.GetRolesAsync(user);

            return roles.Single();
        }
        else
        {
            return string.Empty;
        }
    }

    private async Task<string> GetCurrentUserIdAsync()
    {
        var user = await _userManager.FindByEmailAsync(User.Identity.Name);
        var employee = await _dbCcontext.Users.SingleAsync(em => em.UserName == User.Identity.Name);

        return user is not null && employee is not null ? employee.Id : string.Empty;
    }

    private async Task<ProfilePictureDto> GetCurrentUserProfilePicAsync()
    {
        if (User.Identity.Name == ConstantValues.ADMIN_EMAIL)
        {
            return new();
        }

        var profilePic = await _dbCcontext.ProfilePictures
            .SingleAsync(pp => _dbCcontext.Employees
                .Any(em => pp.EmployeeInfo.Email == em.Email && em.Email == User.Identity.Name)) ?? new();

        return new ProfilePictureDto()
        {
            ImageType = profilePic.ImageType,
            Thumbnail = profilePic.Thumbnail
        };
    }

    private async Task<string> GetCurrentUserFullName()
    {
        if (User.Identity.Name == ConstantValues.ADMIN_EMAIL)
        {
            return ConstantValues.ADMIN_EMAIL;
        }

        if (User.Identity.IsAuthenticated)
        {
            return (await _dbCcontext.Employees
                .SingleAsync(em => em.Email == User.Identity.Name))
                .FullName;
        }

        return string.Empty;
    }

    [HttpGet("current/user/info")]
    public async Task<CurrentUser> GetCurrentUserInfo()
    {
        if (User.Identity.IsAuthenticated)
        {
            return new CurrentUser
            {
                Id = await GetCurrentUserIdAsync(),
                IsAuthenticated = User.Identity.IsAuthenticated,
                UserName = User.Identity.Name,
                FullName = await GetCurrentUserFullName(),
                Role = await GetUserRoleAsync(),
                ProfilePicture = await GetCurrentUserProfilePicAsync(),
                Claims = User.Claims
                    .ToDictionary(c => c.Type, c => c.Value)
            };
        }

        return new();
    }
}