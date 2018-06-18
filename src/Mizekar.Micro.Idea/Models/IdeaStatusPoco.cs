namespace Mizekar.Micro.Idea.Models
{
    /// <summary>
    /// وضعیت ایده
    /// </summary>
    public class IdeaStatusPoco
    {
        public int Order { get; set; }
        public string Title { get; set; }
        public string Value { get; set; }
        public string HexColor { get; set; }
    }
}
