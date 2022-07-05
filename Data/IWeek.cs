using System.Threading.Tasks;

namespace VirtualDean.Data
{
    public interface IWeek
    {
        Task IncrementWeek();
        Task<int> GetLastWeek();
    }
}
