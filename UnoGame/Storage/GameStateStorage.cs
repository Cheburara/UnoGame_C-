using System;
using System.IO;
using System.Text.Json;
using UnoGame.GameObject;
using UnoGame.DAL;

    namespace UnoGame.Storage
    {
        public class GameStateStorage
        {
            public string GameName { get; set; } 
            public void SaveToJSON(string fileName, GameState gameState)
            {
                try
                {
                    // Ensure the file name has the correct extension
                    if (!fileName.EndsWith(".json", StringComparison.OrdinalIgnoreCase))
                    {
                        fileName += ".json";
                    }

                    JsonSerializerOptions options = GetJsonSerializerOptions();
                    string jsonString = JsonSerializer.Serialize(gameState, options);

                    string directoryPath = @"C:\Users\arina\RiderProjects\UNO\UnoGame\JSON";
                    string fullPath = Path.Combine(directoryPath, EnsureJsonExtension(fileName));

                    if (!Directory.Exists(directoryPath))
                    {
                        Directory.CreateDirectory(directoryPath);
                    }

                    // Write the JSON to the file
                    File.WriteAllText(fullPath, jsonString + Environment.NewLine);

                    Console.WriteLine("Game state saved to " + fullPath);
                }
                catch (UnauthorizedAccessException ex)
                {
                    Console.WriteLine("UnauthorizedAccessException: Insufficient permissions to write to the specified file or directory.");
                    Console.WriteLine("Error Details: " + ex.Message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error saving the game state to JSON: " + ex.Message);
                }
            }

            
            public GameState LoadFromJSON(string fileName)
            {
                try
                {
                    string jsonString = File.ReadAllText(EnsureJsonExtension(fileName));
                    GameState gameState = JsonSerializer.Deserialize<GameState>(jsonString, GetJsonSerializerOptions());

                    // If PlayersHands is null, initialize it as an empty dictionary
                    gameState.PlayersHands ??= new Dictionary<string, List<Card>>();

                    return gameState;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error loading the game state from JSON: " + ex.Message);
                    return null;
                }
            }
            
            private JsonSerializerOptions GetJsonSerializerOptions()
            {
                return new JsonSerializerOptions
                {
                    WriteIndented = true,
                    // Add more settings as needed
                };
            }
            private string EnsureJsonExtension(string fileName)
            {
                return fileName.EndsWith(".json", StringComparison.OrdinalIgnoreCase) ? fileName : $"{fileName}.json";
            }
            public void SaveToDatabase(GameState gameState,  AppDbContext dbContext)
            {
                try
                {  
                    var gameRepository = new GameRepositoryEF(dbContext);
                    gameRepository.Save(gameState);

                    Console.WriteLine("Game state saved to the database.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error saving the game state to the database: " + ex.Message);
    
                    if (ex.InnerException != null)
                    {
                        Console.WriteLine("Inner exception: " + ex.InnerException.Message);
                    }
                }

            }

            
        }
    }
