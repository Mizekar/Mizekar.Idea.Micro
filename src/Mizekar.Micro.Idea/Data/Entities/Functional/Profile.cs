using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mizekar.Core.Data;

namespace Mizekar.Micro.Idea.Data.Entities.Functional
{
    public class Profile : BusinessBaseEntity
    {
        /// <summary>
        /// صاحب پروفایل
        /// </summary>
        public long OwnerId { get; set; }

        /// <summary>
        /// کارشناس سیستم
        /// </summary>
        public bool IsExpertUser { get; set; }
    }
}
