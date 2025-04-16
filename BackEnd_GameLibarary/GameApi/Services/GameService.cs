using GameApi.Data;
using GameApi.Dtos;
using GameApi.Models;
using Microsoft.EntityFrameworkCore;

namespace GameApi.Services
{
    public class GameService : IGameService
    {
        private readonly AppDbContext _db;
        public GameService(AppDbContext db)
        {
           _db = db;
        }
        public async Task<IEnumerable<Game>> GetAllGamesAsync()
        {
            try
            {
                // Retrieve games along with their associated GameDevices and Category
                var games = await _db.Games
                    .Include(g => g.category)  // Include Category for category details
                    .Include(g => g.GameDevices)  // Include GameDevices for associated devices
                    .AsNoTracking()
                    .ToListAsync();

                return games;
            }
            catch (Exception ex)
            {
                // Handle the error (log it, rethrow, or return null)
                throw new Exception("Error retrieving games.", ex);
            }
        }
        public async Task<Gamedto> GetGameByIdAsync(int id)
        {
            var game = await _db.Games
                .Include(g => g.category)
                .Include(g => g.GameDevices)
                .FirstOrDefaultAsync(g => g.Id == id);

            if (game == null)
                return null;

            return new Gamedto
            {
                id = game.Id,
                Name = game.Name,
                Description = game.Description,
                ImageUrl = game.ImageUrl,
                VideoUrl = game.VideoUrl,
                CategoryName = game.category?.Name,
                GameDevice = game.GameDevices?.Select(d => d.Name).ToList()
            };
        }


        public async Task<Game> CreateGameAsync(AddGameDto dto)
        {
            var category = await _db.Categories.FindAsync(dto.Categoryid);
            if (category == null)
            {
                throw new Exception("Category not found.");
            }

            var game = new Game
            {
                Name = dto.Name,
                Description = dto.Description,
                ImageUrl = dto.ImageUrl,
                VideoUrl = dto.VideoUrl,
                category = category,
                GameDevices = new List<GameDevice>()
            };

            var gameDevices = await _db.GameDevices
                .Where(gd => dto.GameDeviceIds.Contains(gd.Id))
                .ToListAsync();

            if (gameDevices.Count != dto.GameDeviceIds.Count)
            {
                throw new Exception("Some GameDevices not found.");
            }

            foreach (var gameDevice in gameDevices)
            {
                game.GameDevices.Add(gameDevice);
            }

            _db.Games.Add(game);
            await _db.SaveChangesAsync();

            return game;
        }

        public async Task<bool> DeleteGameAsync(int id)
        {
            var game = await _db.Games.FirstOrDefaultAsync(g => g.Id == id);

            if (game == null)
            {
                return false;  
            }

            _db.Games.Remove(game); 
            await _db.SaveChangesAsync();  

            return true;  
        }


        public async Task<bool> UpdateGameAsync(int id, UpdateGameDto updateGameDto)
        {
            var game = await _db.Games
                .Include(g => g.GameDevices)
                .FirstOrDefaultAsync(g => g.Id == id);

            if (game == null)
                return false;

            // Update fields
            game.Name = updateGameDto.Name;
            game.Description = updateGameDto.Description;
            game.ImageUrl = updateGameDto.ImageUrl;
            game.VideoUrl = updateGameDto.VideoUrl;
            game.Categoryid = updateGameDto.CategoryId;

            // Update GameDevices
            var selectedDevices = await _db.GameDevices
                .Where(d => updateGameDto.GameDeviceIds.Contains(d.Id))
                .ToListAsync();

            game.GameDevices = selectedDevices;

            await _db.SaveChangesAsync();
            return true;
        }

      
    }
}
