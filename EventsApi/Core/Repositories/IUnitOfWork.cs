using EventsApi.Data.Repositories;
using System.Threading.Tasks;

namespace EventsApi.Core.Repositories
{
    public interface IUnitOfWork
    {
        EventRepo EventRepo { get; }
        LectureRepo LecturesRepo { get; }

        Task<bool> CompleteAsync();
    }
}