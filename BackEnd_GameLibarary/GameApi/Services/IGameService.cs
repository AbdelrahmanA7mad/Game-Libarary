using GameApi.Dtos;
using GameApi.Models;

namespace GameApi.Services
{
    public interface IGameService
    {
        Task<IEnumerable<Game>> GetAllGamesAsync();
        Task<Gamedto> GetGameByIdAsync(int id);
        Task<Game> CreateGameAsync(AddGameDto dto);
        Task<bool> DeleteGameAsync(int id);

        Task<bool> UpdateGameAsync(int id, UpdateGameDto updateGameDto);


    }
}
