namespace Mizekar.Micro.Idea.Models.Profiles
{
    public class ProfilePoco
    {
        /// <summary>
        /// صاحب پروفایل
        /// </summary>
        public long OwnerId { get; set; }

        /// <summary>
        /// کارشناس سیستم
        /// </summary>
        public bool IsExpertUser { get; set; }
        /// <summary>
        /// مدیر سیستم
        /// </summary>
        public bool IsSuperAdmin { get; set; }
    }
}
