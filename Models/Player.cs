namespace FootballManager.Models
{
    public class Player
    {
        public int PlayerId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime BirthDate { get; set; }
        public int Number {  get; set; }

        public int ClubId { get; set; }
        public Club Club { get; set; }

    }
}
