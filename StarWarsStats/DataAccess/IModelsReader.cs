using StarWarsStats.DTOs;
using StarWarsStats.ApiDataAccess;
using StarWarsStats.Model;

namespace StarWarsStats.DataAccess;

public interface IModelsReader
{
    Task<List<T>> Read<T, TRoot>(
        string requestHost,
        string requestPath,
        IApiDataReader reserveApiDataReader,
        bool imitateServerError = false) where T : class, IModel
                                         where TRoot : IResultModel;
}