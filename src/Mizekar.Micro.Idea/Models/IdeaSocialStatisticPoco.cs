namespace Mizekar.Micro.Idea.Models
{
    /// <summary>
    /// اطلاعات آماری
    /// </summary>
    public class IdeaSocialStatisticPoco
    {
        /// <summary>
        /// تعداد لایک
        /// </summary>
        public long LikeCount { get; set; }
        /// <summary>
        /// مجموع امتیاز
        /// </summary>
        public long ScoreSum { get; set; }
        /// <summary>
        /// تعداد نظر
        /// </summary>
        public long CommentCount { get; set; }
        /// <summary>
        /// تعداد نمایش
        /// </summary>
        public long ViewCount { get; set; }
    }
}