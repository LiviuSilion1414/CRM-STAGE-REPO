namespace PlannerCRM.Shared.Constants;

public static class ExceptionsMessages
{
    public const string EMPTY_string = "Oggetto string a vuoto.";

    public const string NULL_OBJECT = "Oggetto dto null.";
    public const string NULL_PARAM = "Parametri dto null.";
    public const string NULL_PROP = "Proprietà di tipo null";
    public const string NULL_ARG = "Parametro null.";

    public const string DUPLICATE_OBJECT = "Oggetto duplicato.";
    public const string OBJECT_ALREADY_PRESENT = "Oggetto già presente.";
    public const string OBJECT_NOT_FOUND = "Oggetto non trovato.";
    public const string WORKORDER_NOT_FOUND = "Nessuna commessa trovata.";
    public const string EMPLOYEE_NOT_FOUND = "Dipendenti non trovati.";

    public const string NOT_ASSEGNABLE_ROLE = "Impossibile assegnare questo ruolo.";

    public const string IMPOSSIBLE_ADD = "Impossibile aggiungere l'oggetto.";
    public const string IMPOSSIBLE_EDIT = "Impossibile modificare l'oggetto.";
    public const string IMPOSSIBLE_DELETE = "Impossibile eliminare l'oggetto.";

    public const string IMPOSSIBLE_SAVE_CHANGES = "Impossibile salvare le modifiche sull'oggetto.";
    public const string IMPOSSIBLE_GOING_FORWARD = "Impossibile proseguire.";

    public const string TYPE_MISMATCH = "Type not supported.";
    public const string EMPTY_FIELDS = "Tutti i campi sono obbligatori, si prega di ricontrollare.";

    public const string IMPOSSIBLE_APPLY_FILTER = "Impossibile applicare il filtro di contenuti.";
    public const string SOMETHING_WENT_WRONG = "Qualcosa è andato storto.";
}