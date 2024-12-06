using Newtonsoft.Json;
using StreetOfYourCity.Services.LocationDataServices.Dto;
using Xunit;
using Xunit.Sdk;

namespace StreetOfYourCity.UnitTests
{
    public class DecoderTests
    {
        [Fact]
        public void DecodeBase()
        {
            var testResultString = "{\"version\":0.6,\"generator\":\"Overpass API 0.7.62.4 2390de5a\",\"osm3s\":{\"timestamp_osm_base\":\"2024-12-06T06:48:32Z\",\"timestamp_areas_base\":\"2024-12-06T05:45:41Z\",\"copyright\":\"The data included in this document is from www.openstreetmap.org. The data is made available under ODbL.\"},\"elements\":[{\"type\":\"way\",\"id\":24736995,\"nodes\":[398458100,450660830,398458105,268846028,3754233330,268846025,398458100],\"tags\":{\"highway\":\"service\",\"lit\":\"yes\",\"service\":\"parking_aisle\",\"surface\":\"asphalt\"}},{\"type\":\"way\",\"id\":24976441,\"nodes\":[271395291,271395302,428738098,3754077561,3754077559,3589210383,3589210380,1146304077],\"tags\":{\"bicycle\":\"no\",\"highway\":\"pedestrian\",\"lit\":\"yes\",\"noname\":\"yes\",\"surface\":\"paving_stones\"}}]}";

            var messageBase = JsonConvert.DeserializeObject<OverpassBaseResult>(testResultString);

            Assert.NotNull(messageBase);
            Assert.Equal("Overpass API 0.7.62.4 2390de5a", messageBase.Generator);
        }
    }
}
