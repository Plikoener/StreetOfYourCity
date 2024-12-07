namespace StreetOfYourCity.Services.ImagesServices.Dto;

public class ImageResult
{
    public string Id { get; } = Guid.NewGuid().ToString();

    public ImageSource? Image1 { get; set; }
    public DateTime Created1 { get; set; }
    public string? Creator1 { get; set; }

    public ImageSource? Image2 { get; set; }
    public DateTime Created2 { get; set; }
    public string? Creator2 { get; set; }

    public ImageSource? Image3 { get; set; }
    public DateTime Created3 { get; set; }
    public string? Creator3 { get; set; }

}