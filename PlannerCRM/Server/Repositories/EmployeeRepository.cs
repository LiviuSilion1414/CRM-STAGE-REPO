using PlannerCRM.Shared.DTOs;
using PlannerCRM.Shared.DTOs.EmployeeProfilePictureDto;

namespace PlannerCRM.Server.Repositories;

public class EmployeeRepository
{
    private readonly AppDbContext _dbContext;
    private readonly DtoValidatorUtillity _validator;
    private readonly ILogger<DtoValidatorUtillity> _logger;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public EmployeeRepository(
        AppDbContext dbContext,
        DtoValidatorUtillity validator,
        UserManager<IdentityUser> userManager,
        RoleManager<IdentityRole> roleManager,
        Logger<DtoValidatorUtillity> logger)
    {
        _dbContext = dbContext;
        _validator = validator;
        _userManager = userManager;
        _roleManager = roleManager;
        _logger = logger;
    }

    public async Task AddAsync(EmployeeFormDto dto)
    {
        try
        {
            var isValid = await _validator.ValidateEmployeeAsync(dto, OperationType.ADD);

            if (isValid)
            {
                await _dbContext.Employees.AddAsync(
                    new Employee
                    {
                        Email = dto.Email,
                        FirstName = dto.FirstName,
                        LastName = dto.LastName,
                        FullName = $"{dto.FirstName} {dto.LastName}",
                        Username = dto.Email,
                        BirthDay = dto.BirthDay
                            ?? throw new NullReferenceException(ExceptionsMessages.NULL_ARG),
                        StartDate = dto.StartDate
                            ?? throw new NullReferenceException(ExceptionsMessages.NULL_ARG),
                        Password = dto.Password,
                        NumericCode = dto.NumericCode,
                        Role = dto.Role
                            ?? throw new NullReferenceException(ExceptionsMessages.NULL_ARG),
                        CurrentHourlyRate = dto.CurrentHourlyRate,
                        Salaries = dto.EmployeeSalaries
                            .Select(ems =>
                                new EmployeeSalary
                                {
                                    StartDate = ems.StartDate,
                                    FinishDate = ems.FinishDate,
                                    Salary = ems.Salary
                                }
                            )
                            .ToList()
                    }
                );


                //await _userManager.AddToRoleAsync(user, dto.Role.ToString() ?? throw new ArgumentNullException(nameof(dto.Role), ExceptionsMessages.NULL_ARG));

                if (await _dbContext.SaveChangesAsync() == 0)
                {
                    throw new DbUpdateException(ExceptionsMessages.IMPOSSIBLE_SAVE_CHANGES);
                }

                await AddUserAsync(dto);
                await SetRoleAsync(dto.Email, dto.Role ?? throw new ArgumentNullException(nameof(dto.Role), ExceptionsMessages.NULL_ARG));
                await SetProfilePictureAsync(dto);
                await SetFKProfilePictureIdAsync(dto.Email);
            }
            else
            {
                throw new DbUpdateException(ExceptionsMessages.IMPOSSIBLE_ADD);
            }
        }
        catch (Exception exc)
        {
            _logger.LogError("\nError: {0} \n\nMessage: {1}", exc.StackTrace, exc.Message);

            throw;
        }
    }

    private async Task AddUserAsync(EmployeeFormDto dto)
    {
        try
        {
            var user = new IdentityUser
            {
                Email = dto.Email,
                UserName = dto.Email,
                NormalizedEmail = dto.Email.ToUpper()
            };
            await _userManager.CreateAsync(user, dto.Password);
        }
        catch (Exception exc)
        {
            _logger.LogError("\nError: {0} \n\nMessage: {1}", exc.StackTrace, exc.Message);

            throw;
        }
    }

    private async Task SetRoleAsync(string email, Roles role)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user is not null)
            {
                await _userManager.AddToRoleAsync(user, role.ToString());
            }
        }
        catch (Exception exc)
        {
            _logger.LogError("\nError: {0} \n\nMessage: {1}", exc.StackTrace, exc.Message);

            throw;
        }
    }

    private async Task SetProfilePictureAsync(EmployeeFormDto dto)
    {
        try
        {
            var employee = await _dbContext.Employees
                .SingleAsync(em => em.Email == dto.Email);

            await _dbContext.ProfilePictures.AddAsync(
                new EmployeeProfilePicture
                {
                    ImageType = dto.ProfilePicture.ImageType,
                    Thumbnail = dto.ProfilePicture.Thumbnail,
                    EmployeeId = employee.Id,
                    EmployeeInfo = employee
                }
            );

            await _dbContext.SaveChangesAsync();
        }
        catch (Exception exc)
        {
            _logger.LogError("\nError: {0} \n\nMessage: {1}", exc.StackTrace, exc.Message);

            throw;
        }
    }

    private async Task SetFKProfilePictureIdAsync(string employeeEmail)
    {
        try
        {
            var employee = await _dbContext.Employees
                .SingleAsync(em => em.Email == employeeEmail);

            var profilePic = await _dbContext.ProfilePictures
                .SingleAsync(pic => pic.EmployeeInfo.Email == employeeEmail);

            employee.ProfilePictureId = profilePic.Id;

            _dbContext.Update(employee);

            await _dbContext.SaveChangesAsync();
        }
        catch (Exception exc)
        {
            _logger.LogError("\nError: {0} \n\nMessage: {1}", exc.StackTrace, exc.Message);

            throw;
        }
    }

    public async Task ArchiveAsync(string employeeId)
    {
        try
        {
            var isValid = await _validator.ValidateDeleteEmployeeAsync(employeeId);

            if (isValid)
            {
                var employee = await _dbContext.Employees
                    .SingleAsync(em => em.Id == employeeId);

                employee.IsArchived = true;

                _dbContext.Update(employee);

                if (await _dbContext.SaveChangesAsync() == 0)
                {
                    throw new DbUpdateException(ExceptionsMessages.IMPOSSIBLE_SAVE_CHANGES);
                }
            }
            else
            {
                throw new DbUpdateException(ExceptionsMessages.IMPOSSIBLE_GOING_FORWARD);
            }
        }
        catch (Exception exc)
        {
            _logger.LogError("\nError: {0} \n\nMessage: {1}", exc.StackTrace, exc.Message);

            throw;
        }
    }

    public async Task RestoreAsync(string employeeId)
    {
        try
        {
            var isValid = await _validator.ValidateDeleteEmployeeAsync(employeeId);

            if (isValid)
            {
                var employee = await _dbContext.Employees
                    .SingleAsync(em => em.Id == employeeId);

                employee.IsArchived = false;

                _dbContext.Update(employee);

                if (await _dbContext.SaveChangesAsync() == 0)
                {
                    throw new DbUpdateException(ExceptionsMessages.IMPOSSIBLE_SAVE_CHANGES);
                }
            }
            else
            {
                throw new DbUpdateException(ExceptionsMessages.IMPOSSIBLE_GOING_FORWARD);
            }
        }
        catch (Exception exc)
        {
            _logger.LogError("\nError: {0} \n\nMessage: {1}", exc.StackTrace, exc.Message);

            throw;
        }
    }

    public async Task DeleteAsync(string employeeId)
    {
        try
        {
            var isValid = await _validator.ValidateDeleteEmployeeAsync(employeeId);

            if (isValid)
            {
                var employee = await _dbContext.Employees
                    .SingleAsync(em => em.Id == employeeId);
                _dbContext.Employees.Remove(employee);

                var user = await _userManager.FindByIdAsync(employeeId);
                await _userManager.DeleteAsync(user);
            }
            else
            {
                throw new DbUpdateException(ExceptionsMessages.IMPOSSIBLE_DELETE);
            }
        }
        catch (Exception exc)
        {
            _logger.LogError("\nError: {0} \n\nMessage: {1}", exc.StackTrace, exc.Message);

            throw;
        }
    }

    public async Task EditAsync(EmployeeFormDto dto)
    {
        try
        {
            var isValid = await _validator.ValidateEmployeeAsync(dto, OperationType.EDIT);

            if (isValid)
            {
                var user = await _userManager.FindByEmailAsync(dto.OldEmail);
                var model = await _dbContext.Employees.SingleAsync(em => em.Id == dto.Id);

                model.FirstName = dto.FirstName;
                model.LastName = dto.LastName;
                model.FullName = $"{dto.FirstName + dto.LastName + string.Empty}";
                model.Username = dto.Email;
                model.BirthDay = dto.BirthDay
                    ?? throw new NullReferenceException(ExceptionsMessages.NULL_ARG);
                model.StartDate = dto.StartDate
                    ?? throw new NullReferenceException(ExceptionsMessages.NULL_ARG);
                model.Email = dto.Email;
                model.Role = dto.Role
                    ?? throw new NullReferenceException(ExceptionsMessages.NULL_ARG);
                model.NumericCode = dto.NumericCode;
                model.CurrentHourlyRate = dto.CurrentHourlyRate;

                var isContainedModifiedHourlyRate = await _dbContext.Employees
                    .AnyAsync(em => em.Salaries
                        .Any(s => s.Salary != dto.CurrentHourlyRate));

                if (!isContainedModifiedHourlyRate)
                {
                    model.Salaries = dto.EmployeeSalaries
                        .Where(ems => _dbContext.Employees
                            .Any(em => em.Id == ems.EmployeeId))
                        .Select(ems =>
                            new EmployeeSalary
                            {
                                EmployeeId = dto.Id,
                                StartDate = ems.StartDate,
                                FinishDate = ems.FinishDate,
                                Salary = ems.Salary
                            }
                        ).ToList();
                }

                _dbContext.Employees.Update(model);

                user.Email = dto.Email;
                user.NormalizedEmail = dto.Email.ToUpper();
                user.UserName = dto.Email;

                var passChangeResult = await _userManager.RemovePasswordAsync(user);
                var updateResult = await _userManager.AddPasswordAsync(user, dto.Password);

                if (!passChangeResult.Succeeded || !updateResult.Succeeded)
                {
                    throw new DbUpdateException(ExceptionsMessages.IMPOSSIBLE_SAVE_CHANGES);
                }

                var rolesList = await _userManager.GetRolesAsync(user);
                var userRole = rolesList
                    .Single()
                        ?? throw new NullReferenceException(ExceptionsMessages.NULL_PARAM);

                var isInRole = await _userManager.IsInRoleAsync(user, userRole);

                if (isInRole)
                {
                    var deleteRoleResult = await _userManager.RemoveFromRoleAsync(user, userRole);
                    var reassignmentRoleResult = await _userManager.AddToRoleAsync(user, dto.Role.ToString());

                    if (!deleteRoleResult.Succeeded || !reassignmentRoleResult.Succeeded)
                    {
                        throw new DbUpdateException(ExceptionsMessages.IMPOSSIBLE_SAVE_CHANGES);
                    }
                }

                if (await _dbContext.SaveChangesAsync() == 0)
                {
                    throw new DbUpdateException(ExceptionsMessages.IMPOSSIBLE_SAVE_CHANGES);
                }

                await SetProfilePictureAsync(dto);
                await SetFKProfilePictureIdAsync(dto.Email);
            }
            else
            {
                throw new DbUpdateException(ExceptionsMessages.IMPOSSIBLE_EDIT);
            }
        }
        catch (Exception exc)
        {
            _logger.LogError("\nError: {0} \n\nMessage: {1}", exc.StackTrace, exc.Message);

            throw;
        }
    }

    public async Task<EmployeeViewDto> GetForViewByIdAsync(string employeeId)
    {
        return await _dbContext.Employees
            .Select(em => new EmployeeViewDto
            {
                Id = em.Id,
                FirstName = em.FirstName,
                LastName = em.LastName,
                FullName = $"{em.FirstName} {em.LastName}",
                BirthDay = em.BirthDay,
                StartDate = em.StartDate,
                NumericCode = em.NumericCode,
                Password = em.Password,
                StartDateHourlyRate = em.Salaries.Single().StartDate,
                FinishDateHourlyRate = em.Salaries.Single().FinishDate,
                Email = em.Email,
                IsDeleted = em.IsDeleted,
                IsArchived = em.IsArchived,
                Role = em.Role,
                CurrentHourlyRate = em.CurrentHourlyRate,
                ProfilePicture = new ProfilePictureDto
                {
                    Id = em.ProfilePictureId,
                    EmployeeId = em.Id,
                    EmployeeInfo = new EmployeeSelectDto
                    {
                        Email = em.Email,
                        FirstName = em.FirstName,
                        LastName = em.LastName,
                        FullName = $"{em.FirstName} {em.LastName}",
                        CurrentHourlyRate = em.CurrentHourlyRate,
                        Role = em.Role
                    },
                    Thumbnail = _dbContext.ProfilePictures
                        .Single(pp => pp.EmployeeId == em.Id)
                        .Thumbnail,
                    ImageType = _dbContext.ProfilePictures
                        .Single(pp => pp.EmployeeId == em.Id)
                        .ImageType
                },
                EmployeeSalaries = em.Salaries
                    .Select(ems => new EmployeeSalaryDto
                    {
                        Id = ems.Id,
                        EmployeeId = ems.Id,
                        StartDate = ems.StartDate,
                        FinishDate = ems.StartDate,
                        Salary = ems.Salary
                    })
                    .ToList(),
                EmployeeActivities = em.EmployeeActivity
                    .Select(ea =>
                        new EmployeeActivityDto
                        {
                            EmployeeId = ea.EmployeeId,
                            Employee = new EmployeeSelectDto
                            {
                                Id = ea.Employee.Id,
                                Email = ea.Employee.Email,
                                FirstName = ea.Employee.FirstName,
                                LastName = ea.Employee.LastName,
                                Role = ea.Employee.Role,
                                CurrentHourlyRate = ea.Employee.CurrentHourlyRate,
                            },
                            ActivityId = ea.ActivityId,
                            Activity = new ActivitySelectDto
                            {
                                Id = ea.Activity.Id,
                                Name = ea.Activity.Name,
                                StartDate = ea.Activity.StartDate,
                                FinishDate = ea.Activity.FinishDate,
                                WorkOrderId = ea.Activity.WorkOrderId
                            }
                        })
                    .ToList()
            })
            .SingleAsync(em => em.Id == employeeId);
    }

    public async Task<EmployeeSelectDto> GetForRestoreAsync(string employeeId)
    {
        return await _dbContext.Employees
            .Select(em => new EmployeeSelectDto
            {
                Id = em.Id,
                Email = em.Email,
                FirstName = em.FirstName,
                LastName = em.LastName,
                FullName = em.FullName,
                Role = em.Role
            })
            .SingleAsync(em => em.Id == employeeId);
    }

    public async Task<EmployeeFormDto> GetForEditByIdAsync(string employeeId)
    {
        return await _dbContext.Employees
            .Where(em => !em.IsDeleted || !em.IsArchived)
            .Select(em => new EmployeeFormDto
            {
                Id = em.Id,
                FirstName = em.FirstName,
                LastName = em.LastName,
                Email = em.Email,
                OldEmail = em.Email,
                BirthDay = em.BirthDay,
                StartDate = em.StartDate,
                Role = em.Role,
                NumericCode = em.NumericCode,
                Password = em.Password,
                CurrentHourlyRate = em.CurrentHourlyRate,
                IsDeleted = em.IsDeleted,
                StartDateHourlyRate = em.Salaries.Single().StartDate,
                FinishDateHourlyRate = em.Salaries.Single().FinishDate,
                EmployeeSalaries = em.Salaries
                    .Where(ems => _dbContext.Employees
                        .Any(em => em.Id == ems.EmployeeId))
                    .Select(ems => new EmployeeSalaryDto
                    {
                        Id = ems.Id,
                        EmployeeId = em.Id,
                        StartDate = ems.StartDate,
                        FinishDate = ems.StartDate,
                        Salary = ems.Salary
                    })
                    .ToList()
            })
            .SingleAsync(em => em.Id == employeeId);
    }

    public async Task<EmployeeDeleteDto> GetForDeleteByIdAsync(string employeeId)
    {
        return await _dbContext.Employees
            .Where(em => (!em.IsDeleted || !em.IsArchived) && em.Id == employeeId)
            .Select(em => new EmployeeDeleteDto
            {
                Id = em.Id,
                FullName = $"{em.FirstName} {em.LastName}",
                Email = em.Email,
                Role = em.Role
                    .ToString()
                    .Replace('_', ' '),
                EmployeeActivities = _dbContext.EmployeeActivity
                    .Where(ea => ea.EmployeeId == employeeId)
                    .Select(ea => new EmployeeActivityDto
                    {
                        Id = ea.Id,
                        EmployeeId = ea.EmployeeId,
                        Employee = _dbContext.Employees
                            .Where(e => e.Id == ea.EmployeeId)
                            .Select(_ => new EmployeeSelectDto
                            {
                                Id = ea.Employee.Id,
                                Email = ea.Employee.Email,
                                FullName = ea.Employee.FullName,
                            })
                            .Single(),
                        ActivityId = ea.ActivityId,
                        Activity = _dbContext.Activities
                            .Where(ac => ac.Id == ea.ActivityId)
                            .Select(_ => new ActivitySelectDto
                            {
                                Id = ea.Activity.Id,
                                Name = ea.Activity.Name,
                                WorkOrderId = ea.Activity.WorkOrderId,
                            })
                            .Single()
                    })
                    .ToList()

            })
            .SingleAsync(em => em.Id == employeeId);
    }

    public async Task<List<EmployeeSelectDto>> SearchEmployeeAsync(string email)
    {
        return await _dbContext.Employees
            .Where(em => !em.IsDeleted && !em.IsArchived)
            .Select(em => new EmployeeSelectDto
            {
                Id = em.Id,
                Email = em.Email,
                FirstName = em.FirstName,
                LastName = em.LastName,
                FullName = em.FullName,
                Role = em.Role
            })
            .Where(em => EF.Functions.ILike(em.FullName, $"%{email}%") ||
                EF.Functions.ILike(em.Email, $"%{email}%"))
            .ToListAsync();
    }

    public async Task<List<EmployeeViewDto>> GetPaginatedEmployeesAsync(int limit, int offset)
    {
        return await _dbContext.Employees
            .OrderBy(em => em.Id)
            .Skip(limit)
            .Take(offset)
            .Select(em => new EmployeeViewDto
            {
                Id = em.Id,
                FirstName = em.FirstName,
                LastName = em.LastName,
                FullName = $"{em.FirstName} {em.LastName}",
                BirthDay = em.BirthDay,
                StartDate = em.StartDate,
                Email = em.Email,
                Role = em.Role,
                CurrentHourlyRate = em.CurrentHourlyRate,
                IsDeleted = em.IsDeleted,
                IsArchived = em.IsArchived,
                EmployeeActivities = em.EmployeeActivity
                    .Select(ea => new EmployeeActivityDto
                    {
                        Id = ea.ActivityId,
                        EmployeeId = em.Id,
                        Employee = _dbContext.Employees
                            .Select(em => new EmployeeSelectDto
                            {
                                Id = em.Id,
                                Email = em.Email,
                                FirstName = em.FirstName,
                                LastName = em.LastName,
                                Role = em.Role
                            })
                            .Single(em => em.Id == ea.EmployeeId),
                        ActivityId = ea.ActivityId,
                        Activity = _dbContext.Activities
                            .Select(ac => new ActivitySelectDto
                            {
                                Id = ac.Id,
                                Name = ac.Name,
                                StartDate = ac.StartDate,
                                FinishDate = ac.FinishDate,
                                WorkOrderId = ac.WorkOrderId
                            })
                        .Single(ac => ac.Id == ea.ActivityId),
                    }).ToList(),
                EmployeeSalaries = em.Salaries
                    .Select(ems => new EmployeeSalaryDto
                    {
                        Id = ems.Id,
                        EmployeeId = ems.Id,
                        StartDate = ems.StartDate,
                        FinishDate = ems.StartDate,
                        Salary = ems.Salary
                    })
                    .ToList(),
            })
            .ToListAsync();
    }

    public async Task<CurrentEmployeeDto> GetEmployeeIdAsync(string email)
    {
        return await _dbContext.Employees
            .Where(em => !em.IsDeleted && !em.IsArchived)
            .Select(em => new CurrentEmployeeDto
            {
                Id = em.Id,
                Email = em.Email
            })
            .FirstAsync(em => em.Email == email);
    }

    public async Task<int> GetEmployeesSizeAsync() =>
        await _dbContext.Employees
            .CountAsync();
}