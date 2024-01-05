// UnoGame/Storage/SaveGame.cs
using System;
using System.IO;
using System.Text.Json;
using UnoGame.GameObject;
using UnoGame.GameLogic;

namespace UnoGame.Storage
{
    public class SaveGame
    {
        public void SaveGameState(string fileName, GameState gameState)
        {
            string jsonString = JsonSerializer.Serialize(gameState);
            File.WriteAllText(fileName, jsonString);
            Console.WriteLine("Game state saved to " + fileName);
        }

        public void Save(string gameName, CardDeckLogic cardDeckLogic, Player[] players)
        {
            // Create a GameState object and populate it with data
            GameState gameState = new GameState
            {
                Deck = cardDeckLogic.Deck, // Assuming Deck is a List<Card>
                Players = players,
            };

            // Save the game state with the provided gameName
            string fileName = $"{gameName}.json";
            string fullPath = Path.Combine("JSON", fileName);
            SaveGameState(fileName, gameState);
        }
    }
}