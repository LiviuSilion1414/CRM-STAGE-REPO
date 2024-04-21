namespace PlannerCRM.Shared.DTOs.EmployeeDto.Forms;

public class EmployeeFormDto
{
    public string Id { get; set; }

    [Required(ErrorMessage = """Campo "Immagine profile" richiesto""")]
    public ProfilePictureDto ProfilePicture { get; set; }

    [Required(ErrorMessage = """Campo "Nome" richiesto""")]
    public string FirstName { get; set; }

    [Required(ErrorMessage = """Campo "Cognome" richiesto""")]
    public string LastName { get; set; }

    [StrongPasswordValidator(PASS_MIN_LENGTH, PASS_MAX_LENGTH,
        ErrorMessage = """ Campo "Password" non valido. Riprovare. """)]
    [Required(ErrorMessage = """Campo "Password" richiesto""")]
    public string Password { get; set; }

    [EmailAddress(ErrorMessage = """ Indirizzo "Email" non valido. """)]
    [Required(ErrorMessage = """Campo "Email" richiesto""")]
    public string Email { get; set; }

    public string OldEmail { get; set; }

    public bool IsDeleted { get; set; }

    [StartDateRange(ErrorMessage = """La data d'inizio non può essere oltre la data di oggi.""")]
    [Required(ErrorMessage = """ Campo "Data d'inizio" richiesto. """)]
    public DateTime? StartDate { get; set; }

    [BirthDayRange(MAJOR_AGE, MAX_AGE,
        ErrorMessage = """Il dipendente deve avere tra i 18 e i 55 anni.""")]
    [Required(ErrorMessage = """ Campo "Data di nascita" richiesto. """)]
    public DateTime? BirthDay { get; set; }

    [Required(ErrorMessage = """Campo "Codice fiscale" richiesto""")]
    public string NumericCode { get; set; }

    [EnumDataType(typeof(Roles))]
    [IsNotAdminRole(Roles.ACCOUNT_MANAGER, ErrorMessage = "Ruolo non assegnabile.")]
    [Required(ErrorMessage = """Campo "Ruolo" richiesto """)]
    public Roles? Role { get; set; }

    [MinimumHourlyRate(MINIMUM_HOURLY_RATE, ErrorMessage = $"""La tariffa oraria non può essere minore di 8€. """)]
    [Required(ErrorMessage = """ Campo "Tariffa oraria" richiesto.""")]
    public decimal CurrentHourlyRate { get; set; }

    [StartDateRangeHourlyRate(
        ErrorMessage = """La data d'inizio non può essere oltre la data del giorno stesso.""")]
    [Required(ErrorMessage = """ Campo "Data d'inizio tariffa oraria" richiesto.""")]
    public DateTime? StartDateHourlyRate { get; set; }

    [FinishDateRangeHourlyRate(ErrorMessage = """La data di fine non può essere prima della data d'inizio.""")]
    [Required(ErrorMessage = """ Campo "Data di fine tariffa oraria" richiesto.""")]
    public DateTime? FinishDateHourlyRate { get; set; }

    public List<EmployeeSalaryDto> EmployeeSalaries { get; set; }
}