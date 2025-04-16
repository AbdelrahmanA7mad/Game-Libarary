namespace GameApi.Dtos
{
    public class Gamedto
    {
        public int ? id { get; set; }
        public string ? Name { get; set; }
        public string ? Description { get; set; }
        public string ? ImageUrl { get; set; }
        public string ? VideoUrl { get; set; }
        public int ? Categoryid { get; set; }
        public string ? CategoryName  { get; set; }
        public List<int> ? GameDeviceIds { get; set; } 
        public List<string> ? GameDevice { get; set; } 

    }
}
