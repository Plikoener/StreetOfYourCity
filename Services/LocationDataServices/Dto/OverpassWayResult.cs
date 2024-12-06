using System.Runtime.Serialization;

namespace StreetOfYourCity.Services.LocationDataServices.Dto;

[DataContract]
public class OverpassWayResult : OverpassBaseResult
{
    [DataMember( Name = "elements" )]
    public List<WayElement>? Elements { get; set; }
}