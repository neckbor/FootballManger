namespace FootballManager.Models
{
    public class Country
    {
        public int CountryId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }

        public int FootballAssociationId { get; set; }
        public FootballAssociation FootballAssociation { get; set; }
    }
}
