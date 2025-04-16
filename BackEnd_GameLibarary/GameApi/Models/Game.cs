namespace GameApi.Models
{
    public class Game
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public string VideoUrl { get; set; }
        public int Categoryid { get; set; }
        public Category category { get; set; }
        public ICollection<GameDevice> GameDevices { get; set; }



    }
}
