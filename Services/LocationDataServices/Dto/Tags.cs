using System.Runtime.Serialization;

namespace StreetOfYourCity.Services.LocationDataServices.Dto;

[DataContract]
public class Tags
{
    [DataMember( Name = "highway" )]
    public string? Highway { get; set; }

    [DataMember( Name = "name" )]
    public string? Name { get; set; }

    [DataMember( Name = "surface" )]
    public string? Surface { get; set; }
}