using System;
using Microsoft.EntityFrameworkCore;
using UnoGame.Storage;
using UnoGame.DAL.Entity;
using UnoGame.GameObject;

namespace UnoGame.DAL
{
    public class GameRepositoryEF
    {
        private readonly AppDbContext _ctx;

        public GameRepositoryEF(AppDbContext ctx)
        {
            _ctx = ctx;
        }

        public void Save(GameState gameState)
        {
            var gameEntity = new GameEntity
            {
                GameName = gameState.GameName,
                NumberOfPlayers = gameState.NumberOfPlayers,
                Id = gameState.Id,
                PlayerHandSummaries = new List<PlayerHandSummaryEntity>(),
                UpdatedAt = gameState.UpdatedAt
            };

            _ctx.Games.Add(gameEntity);
            _ctx.SaveChanges();

            Console.WriteLine("Game state saved to the database.");

            foreach (var playerState in gameState.Players)
            {
                var playerEntity = new PlayerEntity
                {
                    Name = playerState.Name,
                    Type = playerState.Type,
                    GameId = gameEntity.Id,
                };

                // Save player entity
                _ctx.Players.Add(playerEntity);
                _ctx.SaveChanges();

                Console.WriteLine($"Player '{playerState.Name}' saved to the database.");

            }
        }
    }
}