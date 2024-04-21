using Microsoft.AspNetCore.Components.Routing;
using Microsoft.JSInterop;
using PlannerCRM.Shared.Constants;

namespace PlannerCRM.Client.Utilities.Navigation;

public class NavigationLockService
{
    private readonly IJSRuntime _js;
    private readonly NavigationManager _navigationManager;
    private readonly CurrentUserInfoService _currentUserInfoService;
    private readonly AuthenticationStateService _authStateService;

    private CurrentUser _currentUser;
    private CurrentEmployeeDto _currentEmployee;
    private string _userRole = string.Empty;
    private bool _isAuthenticated = false;

    public const bool ConfirmedExternalExit = true;

    public NavigationLockService(
        IJSRuntime js,
        NavigationManager navigationManager,
        CurrentUserInfoService currentUserInfoService,
        AuthenticationStateService authStateService
    )
    {
        _js = js;
        _navigationManager = navigationManager;
        _currentUserInfoService = currentUserInfoService;
        _authStateService = authStateService;
        _currentUser = new();
        _currentEmployee = new();
    }

    public async Task ConfirmInternalExit(LocationChangingContext context)
    {
        var confirmed = await _js
            .InvokeAsync<bool>("window.confirm", "Changes that you made may not be saved. Continue?");

        if (!confirmed)
        {
            context.PreventNavigation();
        }
    }

    public async Task<CurrentUser> GetCurrentUserAsync()
    {
        return await _authStateService.GetCurrentUserAsync();
    }

    public async Task<bool> IsUserAuthenticated()
        => (await GetCurrentUserAsync()).IsAuthenticated;

    public string GetCurrentPage()
    {
        return _navigationManager.Uri.Replace(_navigationManager.BaseUri, "/");
    }

    public string BuildNavigationUrl(string role, string employeeId)
    {
        if (!string.IsNullOrEmpty(role) && Enum.TryParse(role, out Roles parsedRole))
        {
            var url = parsedRole
                .ToString()
                .ToLower()
                .Replace('_', '-');

            if (parsedRole == Roles.SENIOR_DEVELOPER || parsedRole == Roles.JUNIOR_DEVELOPER)
            {
                url += $"/{employeeId}";
            }

            return url;
        }

        return string.Empty;
    }

    public async Task HandleAuthenticationAndNavigationAsync()
    {
        var authState = await _authStateService.GetAuthenticationStateAsync();
        _currentUser = await _authStateService.GetCurrentUserAsync();
        _isAuthenticated = authState.User.Identity.IsAuthenticated;

        if (_isAuthenticated)
        {
            _userRole = await _currentUserInfoService.GetCurrentUserRoleAsync();

            if (_currentUser.UserName != ConstantValues.ADMIN_EMAIL)
            {
                _currentEmployee = await _currentUserInfoService.GetCurrentEmployeeIdAsync(_currentUser.UserName);
            }

            _navigationManager.NavigateTo(BuildNavigationUrl(_userRole, _currentEmployee.Id));
        }
    }
}
