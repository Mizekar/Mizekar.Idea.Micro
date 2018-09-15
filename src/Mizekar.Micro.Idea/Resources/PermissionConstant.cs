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
        public static KeyValuePair<string, string> ViewAnyIdeaContactsInfo = new KeyValuePair<string, string>("view_any_idea_contacts_info", "دیدن اطلاعات ارتباطی همه سوژه ها");
        public static KeyValuePair<string, string> DeleteAnyIdeas = new KeyValuePair<string, string>("delete_any_ideas", "امکان حذف تمام سوژه ها");
        public static KeyValuePair<string, string> EditAnyIdeas = new KeyValuePair<string, string>("edit_any_ideas", "امکان ویرایش تمام سوژه ها");
        public static KeyValuePair<string, string> EditAnyComments = new KeyValuePair<string, string>("edit_any_comments", "امکان ویرایش تمام نظرات");
        public static KeyValuePair<string, string> DeleteAnyComments = new KeyValuePair<string, string>("delete_any_comments", "امکان حذف تمام نظرات");
    }
}
