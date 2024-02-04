namespace FootballManager.Models
{
    public class Club
    {
        public int ClubId { get; set; }
        public string Name { get; set; }
        public DateTime FoundationDate { get; set; }

        public int CityId { get; set; }
        public City City { get; set; }
        public ICollection<Player> Players { get; set; }
    }
}
