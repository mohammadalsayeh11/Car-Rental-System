namespace CentalMvc.Models.ViewModels;

public class HomeViewModel
{
    public List<Car> FeaturedCars { get; set; } = new();
    public List<BlogPost> BlogPosts { get; set; } = new();
    public List<TeamMember> Team { get; set; } = new();

    // Average rating + review count per car id (for the vehicle cards)
    public Dictionary<int, (double Avg, int Count)> CarRatings { get; set; } = new();
}
