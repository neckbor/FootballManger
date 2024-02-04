namespace FootballManager.Models
{
    public class FootballAssociation
    {
        public int FootballAssociationId { get; set; }
        public string Name { get; set; }

        public ICollection<Country> Countries { get; set; }
    }
}
