namespace CentalMvc.Models.ViewModels;

public class AboutViewModel
{
    public AboutInfo Info { get; set; } = new();
    public List<TeamMember> Team { get; set; } = new();
}
