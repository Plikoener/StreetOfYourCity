using System.Runtime.Serialization;

namespace StreetOfYourCity.Services.LocationDataServices.Dto;

[DataContract]
public class OsmInfo
{
    [DataMember( Name = "timestamp_osm_base" )]
    public DateTime TimestampOsm { get; set; }

    [DataMember( Name = "timestamp_areas_base" )]
    public DateTime TimestampAreas { get; set; }

    [DataMember( Name = "copyright" )]
    public string? Copyright { get; set; }
}