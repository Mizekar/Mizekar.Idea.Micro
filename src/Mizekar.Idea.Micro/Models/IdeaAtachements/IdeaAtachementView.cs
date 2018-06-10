using System;
using System.ComponentModel.DataAnnotations;
using Mizekar.Idea.Micro.Data.Entities;

namespace Mizekar.Idea.Micro.Models.IdeaAtachements
{
    /// <summary>
    /// Idea Atachements Info
    /// </summary>
    public class IdeaAtachementView : BusinessBaseModel
    {
        /// <summary>
        /// Id of Idea
        /// </summary>
        public Guid IdeaInfoId { get; set; }

        /// <summary>
        /// Title
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// Description
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Type of Media
        /// </summary>
        public AtachementType AtachementType { get; set; }
        /// <summary>
        /// FileId in Drive Service
        /// </summary>
        public Guid FileId { get; set; }
    }
}
