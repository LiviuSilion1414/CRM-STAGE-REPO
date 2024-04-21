using PlannerCRM.Shared.DTOs.EmployeeProfilePictureDto;

namespace PlannerCRM.Shared.Models;

public class CurrentUser
{
    public string Id { get; set; }
    public string UserName { get; set; }
    public string FullName { get; set; }
    public string Role { get; set; }
    public bool IsAuthenticated { get; set; }
    public ProfilePictureDto ProfilePicture { get; set; }
    public Dictionary<string, string> Claims { get; set; }
}