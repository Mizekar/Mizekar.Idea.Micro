using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mizekar.Idea.Micro.Models
{
    public class RelationSimpleView
    {
        public RelationSimpleView()
        {

        }

        public RelationSimpleView(Guid id, string title)
        {
            Id = id;
            Title = title;
        }
        public Guid Id { get; set; }
        public string Title { get; set; }
    }
}
