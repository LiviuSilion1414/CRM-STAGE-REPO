namespace PlannerCRM.Shared.DTOs.EmployeeProfilePictureDto;

public class ProfilePictureDto
{
    public string Id { get; set; }

    public string EmployeeId { get; set; }

    public string ImageType { get; set; }

    public string Thumbnail { get; set; }

    public virtual EmployeeSelectDto EmployeeInfo { get; set; }
}