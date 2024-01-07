
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UnoGame.GameObject;

namespace UnoGame.DAL.Entity
{
    public class GameEntity
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string GameName { get; set; }
        public int NumberOfPlayers { get; set; }

        // Collection of players associated with this game
        public ICollection<PlayerEntity> Players { get; set; }

        // Collection of player hand summaries associated with this game
        public ICollection<PlayerHandSummaryEntity> PlayerHandSummaries { get; set; }

        public DateTime UpdatedAt { get; set; }
    }

    public class PlayerEntity
    {
        [Key]
        public int PlayerId { get; set; }
        public string Name { get; set; }
        public PlayerType Type { get; set; }

        // Foreign key to associate with a game
        public Guid GameId { get; set; }
        public GameEntity Game { get; set; }

        // Navigation property: association with the player's hand
        public PlayerHandEntity Hand { get; set; }
    }

    public class PlayerHandEntity
    {
        [Key]
        public int PlayerHandId { get; set; }

        // Foreign key to associate with a player
        public int PlayerId { get; set; }

        // Navigation property: association with the player
        public PlayerEntity Player { get; set; }

        // Collection of cards in the player's hand
        public List<Card> Cards { get; set; }
    }

    public class CardEntity
    {
        [Key]
        public int CardId { get; set; }
        public string Value { get; set; }
        public string Color { get; set; }
    }

    public class PlayerHandSummaryEntity
    {
        [Key]
        public int PlayerHandSummaryId { get; set; }

        // Foreign key to associate with a game
        public Guid GameId { get; set; }
        [ForeignKey("GameId")]
        public GameEntity Game { get; set; }

        // Foreign key to associate with a player
        public int PlayerId { get; set; }
        [ForeignKey("PlayerId")]
        public PlayerEntity Player { get; set; }

        // The PlayerName property is redundant if you can get it from the associated Player entity.
        public int TotalCards { get; set; }
    }
}
