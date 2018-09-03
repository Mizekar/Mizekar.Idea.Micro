using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mizekar.Micro.Idea.Resources
{
    /// <summary>
    /// 
    /// </summary>
    public static class PermissionConstant
    {
        public static KeyValuePair<string, string> ViewAllIdeaContactsInfo = new KeyValuePair<string, string>("view_all_idea_contacts_info", "دیدن اطلاعات ارتباطی همه سوژه ها");
        public static KeyValuePair<string, string> DeleteAllIdeas = new KeyValuePair<string, string>("delete_all_ideas", "امکان حذف تمام سوژه ها");
        public static KeyValuePair<string, string> EditAllIdeas = new KeyValuePair<string, string>("edit_all_ideas", "امکان ویرایش تمام سوژه ها");
        public static KeyValuePair<string, string> EditAllComments = new KeyValuePair<string, string>("edit_all_comments", "امکان ویرایش تمام نظرات");
    }
}
