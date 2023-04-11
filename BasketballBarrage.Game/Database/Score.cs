using MongoDB.Bson;
using Realms;

namespace BasketballBarrage.Game.Database;

public class Score : RealmObject
{
    [PrimaryKey]
    [MapTo("id")]
    public ObjectId Id { get; set; } = ObjectId.GenerateNewId();

    [MapTo("player_name")]
    [Required]
    public string PlayerName { get; set; } = null!;

    [MapTo("points")]
    public int Points { get; set; }

    [MapTo("mode")]
    [Required]
    public string Mode { get; set; } = null!;

    [MapTo("timestamp")]
    [Required]
    public string Timestamp { get; set; } = null!;
}
