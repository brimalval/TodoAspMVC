using TodoWeb.Dtos;

namespace TodoWeb.Data.Services
{
    public interface IStatusService : ICrudService<StatusViewDto, CreateStatusArgs, UpdateStatusArgs>
    {
    }
}
