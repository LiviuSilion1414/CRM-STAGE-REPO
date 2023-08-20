namespace PlannerCRM.Server.Utilities;

public class DtoValidatorUtillity
{
    private readonly AppDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;

    public DtoValidatorUtillity(AppDbContext context, UserManager<IdentityUser> userManager) {
        _context = context;
        _userManager = userManager;
    }

    public async Task<bool> ValidateEmployeeAsync(EmployeeFormDto dto, OperationType operation) {
        var isValid = CheckDtoHealth(dto);

        if (isValid) {
            if (dto.Role == ConstantValues.ADMIN_ROLE || dto.Email == ConstantValues.ADMIN_EMAIL) {
                throw new DuplicateElementException(message: ExceptionsMessages.NOT_ASSEGNABLE_ROLE);
            }

            var employeeIsAlreadyPresent = await _context.Employees
                .AnyAsync(em => 
                    EF.Functions.ILike(em.Email, $"%{dto.Email}%") && em.Id == dto.Id);        
            
            var userIsAlreadyPresent = await _userManager.Users
                .AnyAsync(user => user.Email == dto.Email);

            if (operation == OperationType.ADD) {
                if (employeeIsAlreadyPresent && userIsAlreadyPresent) {
                    throw new DuplicateElementException(message: ExceptionsMessages.OBJECT_ALREADY_PRESENT);
                }
            } 
            
            if (operation == OperationType.EDIT) {
                if (!employeeIsAlreadyPresent && !userIsAlreadyPresent) {
                    throw new KeyNotFoundException(message: ExceptionsMessages.OBJECT_NOT_FOUND);
                }
            }

            return true;
        }
        
        return false;
    }

    public async Task<bool> ValidateWorkOrderAsync(WorkOrderFormDto dto, OperationType operation) {
        var isValid = CheckDtoHealth(dto);
        
        if (isValid) {
            var isAlreadyPresent = await _context.WorkOrders
                .AnyAsync(wo=> ((!wo.IsCompleted) || (!wo.IsDeleted)) && wo.Id == dto.Id);

            if (operation == OperationType.ADD) {
                if (isAlreadyPresent) {
                    throw new DuplicateElementException(message: ExceptionsMessages.OBJECT_ALREADY_PRESENT);
                }
            } 
            if (operation == OperationType.EDIT) {
                if (!isAlreadyPresent) {
                    throw new KeyNotFoundException(message: ExceptionsMessages.OBJECT_NOT_FOUND);
                }
            }

            return true;
        }

        return false;
    }

    public async Task<bool> ValidateActivityAsync(ActivityFormDto dto, OperationType operation) {
        var isValid = CheckDtoHealth(dto);

        if (isValid) {
            var isAlreadyPresent = await _context.Activities
                .AnyAsync(ac => ac.Id == dto.Id);

            if (operation == OperationType.ADD) {
                if (isAlreadyPresent) {
                    throw new DuplicateElementException(message: ExceptionsMessages.OBJECT_ALREADY_PRESENT);
                }
            } 
            if (operation == OperationType.EDIT) {
                if (!isAlreadyPresent) {
                    throw new KeyNotFoundException(message: ExceptionsMessages.OBJECT_NOT_FOUND);
                }
            }

            if (dto.ViewEmployeeActivity is null) {
                throw new NullReferenceException(ExceptionsMessages.NULL_PROP);
            }

            return true;
        }

        return false;
    }

    public bool ValidateWorkTime(WorkTimeRecordFormDto dto) => 
        CheckDtoHealth(dto);

    public async Task<Employee> ValidateDeleteEmployeeAsync(int id) {
        return await _context.Employees
            .SingleOrDefaultAsync(em => em.Id == id) ??
                throw new KeyNotFoundException(ExceptionsMessages.IMPOSSIBLE_DELETE);
    }

    public async Task<IdentityUser> ValidateDeleteUserAsync(string email) {
        if (string.IsNullOrEmpty(email)) {
            throw new ArgumentNullException(email, ExceptionsMessages.NULL_OBJECT);
        }

        return await _userManager.FindByEmailAsync(email) 
            ?? throw new KeyNotFoundException(ExceptionsMessages.OBJECT_NOT_FOUND);
    }

    public async Task<WorkOrder> ValidateDeleteWorkOrderAsync(int id) {
        var hasRelationships = await _context.EmployeeActivity
			.AnyAsync(ea  => ea.Activity.WorkOrderId == id);

        return await _context.WorkOrders
			.SingleOrDefaultAsync(w => w.Id == id) 
                ?? throw new KeyNotFoundException(ExceptionsMessages.IMPOSSIBLE_DELETE);
                
    }

    public async Task<Activity> ValidateDeleteActivityAsync(int id) {
        return await _context.Activities
            .SingleOrDefaultAsync(ac => ac.Id == id)
                ?? throw new InvalidOperationException(ExceptionsMessages.IMPOSSIBLE_DELETE);
    }

    private static bool CheckDtoHealth(object dto) {
        if (dto is null) {
            return false;   
        }

        var hasPropertiesNull = dto
            .GetType()
            .GetProperties()
            .Any(prop => prop.GetValue(dto) is null);

        if (hasPropertiesNull) {
            return false;
        }

        return true;
    }
}