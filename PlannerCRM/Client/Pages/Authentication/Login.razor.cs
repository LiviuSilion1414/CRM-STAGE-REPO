using PlannerCRM.Client.Utilities.Navigation;

namespace PlannerCRM.Client.Pages.Authentication;

public partial class Login : ComponentBase
{
    [Inject] public CurrentUserInfoService CurrentUserInfoService { get; set; }
    [Inject] public LoginService LoginService { get; set; }
    [Inject] public NavigationManager NavManager { get; set; }
    [Inject] public NavigationLockService NavigationUtil { get; set; }
    [Inject] public CustomDataAnnotationsValidator CustomValidator { get; set; }
    [Inject] public Logger<LoginService> Logger { get; set; }

    private EmployeeLoginDto _model;
    private CurrentEmployeeDto _currentEmployee;
    private EditContext _editContext;

    private Dictionary<string, List<string>> _errors;

    private string _message;
    private bool _isError;
    private string _input;

    protected override void OnInitialized()
    {
        _model = new();
        _editContext = new(_model);
        _currentEmployee = new();
    }

    private async Task LoginOnValidInput()
    {
        try
        {
            var isValid = ValidatorService.Validate(_model, out _errors);

            if (isValid)
            {
                var response = await LoginService.LoginAsync(_model);

                if (response.IsSuccessStatusCode && _model.Email != ConstantValues.ADMIN_EMAIL)
                {
                    _currentEmployee = await CurrentUserInfoService.GetCurrentEmployeeIdAsync(_model.Email);
                }

                if (response.IsSuccessStatusCode)
                {

                    var role = await CurrentUserInfoService.GetCurrentUserRoleAsync();
                    var navigationUrl = NavigationUtil.BuildNavigationUrl(role, _currentEmployee.Id);

                    NavManager.NavigateTo(navigationUrl, true);
                }
                else
                {
                    _isError = true;
                    _message = await response.Content.ReadAsStringAsync();
                }
            }
            else
            {
                CustomValidator.DisplayErrors(_errors);
            }
        }
        catch (Exception exc)
        {
            _isError = true;
            _message = exc.Message;
            Logger.LogError("Error: { } Message: { }", exc.Message, exc.StackTrace);
        }
    }

    private void OnClickHideBanner(bool hidden) => _isError = hidden;

    private void SwitchPassword(string type) => _input = type;
}