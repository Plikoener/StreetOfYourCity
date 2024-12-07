namespace StreetOfYourCity.Models;

public class ImageServiceModel
{
    public string Url { get; set; } = null!;
    public DateTime RecordTime { get; set; }
    public string? Creator { get; set; }

    public ImageServiceModel(string url, DateTime recordTime, string? creator)
    {
        Url = url;
        RecordTime = recordTime;
        Creator = creator;
    }
}
