namespace GameApi.Dtos
{
    public class AddGameDto
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public string? VideoUrl { get; set; }
        public int? Categoryid { get; set; }
        public List<int>? GameDeviceIds { get; set; }
    }
}
