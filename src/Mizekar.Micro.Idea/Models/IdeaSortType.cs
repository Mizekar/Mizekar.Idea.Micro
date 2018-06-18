using System.Runtime.Serialization;

namespace Mizekar.Micro.Idea.Models
{
    /// <summary>
    /// Sort Type
    /// </summary>
    public enum IdeaSortType
    {
        [EnumMember(Value = "+createdon")]
        CreatedOnAsc,
        [EnumMember(Value = "-createdon")]
        CreatedOnDes,

        [EnumMember(Value = "+modifiedon")]
        ModifiedOnAsc,
        [EnumMember(Value = "-modifiedon")]
        ModifiedOnDesc,

        [EnumMember(Value = "+like")]
        LikeAsc,
        [EnumMember(Value = "-like")]
        LikeDesc,

        [EnumMember(Value = "+comment")]
        CommentAsc,
        [EnumMember(Value = "-comment")]
        CommentDesc,

        [EnumMember(Value = "+score")]
        ScoreAsc,
        [EnumMember(Value = "-score")]
        ScoreDesc,
    }
}