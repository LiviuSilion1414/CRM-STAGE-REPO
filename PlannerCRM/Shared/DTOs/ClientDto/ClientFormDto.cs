namespace PlannerCRM.Shared.DTOs.ClientDto;

public class ClientFormDto
{
    public string Id { get; set; }

    [Required(ErrorMessage = """ Campo "Nome" richiesto """)]
    public string Name { get; set; }

    [Required(ErrorMessage = """ Campo "Partita IVA" richiesto """)]
    public string VatNumber { get; set; }

    public string WorkOrderId { get; set; }
}