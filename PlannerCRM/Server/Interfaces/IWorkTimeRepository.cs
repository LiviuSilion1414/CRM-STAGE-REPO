namespace PlannerCRM.Server.Interfaces;

public interface IWorkTimeRepository
{
    public Task<List<WorkTimeRecordViewDto>> GetPaginatedWorkTimeRecordsAsync(int limit, int offset);
    public Task<WorkTimeRecordViewDto> GetAllWorkTimeRecordsByEmployeeIdAsync(int employeeId);
    public Task<WorkTimeRecordViewDto> GetForViewByIdAsync(int workOrderId, int activityId, int employeeId);
}