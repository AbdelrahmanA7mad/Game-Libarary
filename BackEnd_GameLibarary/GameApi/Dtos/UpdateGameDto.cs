namespace GameApi.Dtos
{
    public class UpdateGameDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public string VideoUrl { get; set; }
        public int CategoryId { get; set; }
        public List<int> GameDeviceIds { get; set; } // IDs of GameDevices to associate
    }

}
