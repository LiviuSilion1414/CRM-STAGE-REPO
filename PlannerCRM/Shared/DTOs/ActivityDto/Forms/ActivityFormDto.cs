namespace PlannerCRM.Shared.DTOs.ActivityDto.Forms;

public partial class ActivityFormDto
{
    public string Id { get; set; }

    [Required(ErrorMessage = """ Campo "Nome" richiesto. """)]
    public string Name { get; set; }

    [Required(ErrorMessage = """ Campo "Data d'inizio" richiesto. """)]
    public DateTime? StartDate { get; set; }

    [Required(ErrorMessage = """ Campo "Data di fine" richiesto. """)]
    public DateTime? FinishDate { get; set; }

    public int? WorkOrderId { get; set; }

    public HashSet<EmployeeActivityDto> EmployeeActivity { get; set; }

    [CannotBeEmpty(ErrorMessage = """ Campo "Dipendenti selezionati" richiesto. """)]
    public HashSet<EmployeeActivityDto> ViewEmployeeActivity { get; set; }

    public HashSet<EmployeeActivityDto> DeleteEmployeeActivity { get; set; }

    [Required(ErrorMessage = """ Campo "Commessa" richiesto. """)]
    public string SelectedWorkOrder { get; set; }

    public string SelectedEmployee { get; set; }

    public string ClientName { get; set; }
}