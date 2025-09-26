namespace Winhanced_Shell.Models
{
    public class GameMetadata
    {
        public string Name { get; set; }
        public DateTime SessionStartTime { get; set; }
        public ulong Playtime { get; set; }
        public DateTime? LastPlayed { get; set; }
        public int AchievementsUnlocked { get; set; }
        public int TotalAchievements { get; set; }
        public string DisplayBoxArtPath { get; set; }
        public string DisplayHeroArtPath { get; set; }
    }
}
