namespace QFLibrary.Code.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public string Publisher { get; set; }
        public int YearPublished { get; set; }
        public double AverageRating { get; set; }
    }
}