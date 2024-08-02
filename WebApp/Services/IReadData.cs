using System.Threading.Tasks;

namespace WebApp.Services;


public interface IReadData<T>
{
    public Task<T> ReadData(string path);
}
