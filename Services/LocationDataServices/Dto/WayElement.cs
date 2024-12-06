using System.Runtime.Serialization;

namespace StreetOfYourCity.Services.LocationDataServices.Dto;

[DataContract]
public class WayElement
{
    [DataMember( Name = "type" )]
    public string? Type { get; set; }

    [DataMember( Name = "id" )]
    public long Id { get; set; }

    [DataMember( Name = "nodes" )]
    public List<long>? Nodes { get; set; }

    [DataMember( Name = "tags" )]
    public Tags? Tags { get; set; }
}