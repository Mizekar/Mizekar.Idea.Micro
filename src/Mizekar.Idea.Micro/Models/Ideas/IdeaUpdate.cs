using System.ComponentModel.DataAnnotations;

namespace Mizekar.Idea.Micro.Models.Ideas
{
    public class IdeaUpdate
    {
        [MaxLength(50)]
        public string FirstName { get; set; }
        [MaxLength(50)]
        public string LastName { get; set; }
        [MaxLength(100)]
        public string UserName { get; set; }

        [MaxLength(200)]
        public string Email { get; set; }
        public bool EmailVerified { get; set; }
        [MaxLength(20)]
        public string Mobile { get; set; }
        public bool MobileVerified { get; set; }

        public int ImageId { get; set; }

    }
}