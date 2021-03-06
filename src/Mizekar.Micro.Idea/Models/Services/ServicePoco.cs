﻿using System;

namespace Mizekar.Micro.Idea.Models.Services
{
    /// <summary>
    /// خدمات
    /// </summary>
    public class ServicePoco
    {
        public int Order { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Guid? ImageId { get; set; }
        public bool IsSpecial { get; set; }
        public bool IsActive { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset EndDate { get; set; }
    }
}
