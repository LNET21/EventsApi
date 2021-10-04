using EventClient.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace EventClient.HttpClients
{
    public interface IEventClient2
    {
        Task<CodeEventDto> Get(CancellationToken token, string name);
        Task<IEnumerable<CodeEventDto>> GetAll(CancellationToken token);
        Task<IEnumerable<LectureDto>> GetLectures(CancellationToken token, string name);
    }
}