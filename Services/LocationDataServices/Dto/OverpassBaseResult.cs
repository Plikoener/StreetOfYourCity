using System.Runtime.Serialization;

namespace StreetOfYourCity.Services.LocationDataServices.Dto;

[DataContract]
public class OverpassBaseResult
{
    [DataMember( Name = "version" )]
    public double Version { get; set; }

    
    [DataMember( Name = "generator" )]
    public string? Generator { get; set; }

    [DataMember( Name = "osm3s" )]
    public OsmInfo? Info { get; set; }
}