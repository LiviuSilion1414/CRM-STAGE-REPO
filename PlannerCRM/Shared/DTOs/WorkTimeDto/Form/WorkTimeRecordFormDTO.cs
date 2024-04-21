namespace PlannerCRM.Shared.DTOs.WorkTimeDto.Form;

public class WorkTimeRecordFormDto

{
    public string Id { get; set; }

    [Required(ErrorMessage = """ Campo "Data" richiesto. """)]
    public DateTime Date { get; set; }

    [Required(ErrorMessage = """ Campo "Tariffa oraria" richiesto. """)]
    public int? Hours { get; set; }

    [Required(ErrorMessage = """ Campo "Prezzo totale" richiesto.""")]
    public decimal TotalPrice { get; set; }

    [Required(ErrorMessage = """ Campo "Attività" richiesto. """)]
    public string ActivityId { get; set; }

    [Required(ErrorMessage = """ Campo "Commessa" richiesto. """)]
    public string WorkOrderId { get; set; }

    [Required(ErrorMessage = """ Campo "Impiegato" richiesto. """)]
    public string EmployeeId { get; set; }

    [Required(ErrorMessage = """ Campo "Impiegato" richiesto. """)]
    public EmployeeViewDto Employee { get; set; }
}