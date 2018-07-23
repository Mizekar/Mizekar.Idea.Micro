using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mizekar.Core.Data;
using Mizekar.Core.Data.Services;
using Mizekar.Core.Model.Api;
using Mizekar.Core.Model.Api.Response;
using Mizekar.Micro.Idea.Data;
using Mizekar.Micro.Idea.Data.Entities;
using Mizekar.Micro.Idea.Models;
using Mizekar.Micro.Idea.Models.Announcements;
using Mizekar.Micro.Idea.Models.Dashboard;
using Mizekar.Micro.Idea.Models.Services;
using NSwag.Annotations;

namespace Mizekar.Micro.Idea.Controllers
{
    /// <summary>
    /// Dashboard - داشبورد
    /// </summary>
    [Route("api/v1/[controller]")]
    [ApiController]
    [SwaggerTag(name: "Dashboard", Name = "Dashboard", Description = "Dashboard - داشبورد")]
    public class DashboardController : ControllerBase
    {
        private readonly DbSet<IdeaInfo> _ideas;
        private readonly IdeaDbContext _context;
        private readonly IMapper _mapper;
        private readonly IUserResolverService _userResolverService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="mapper"></param>
        /// <param name="userResolverService"></param>
        public DashboardController(IdeaDbContext context, IMapper mapper, IUserResolverService userResolverService)
        {
            _context = context;
            _mapper = mapper;
            _userResolverService = userResolverService;
            _ideas = _context.IdeaInfos;
        }

        /// <summary>
        /// Get General Overview
        /// </summary>
        /// <returns></returns>
        [HttpGet("Overview")]
        [ProducesResponseType(typeof(GeneralOverview), 200)]
        public async Task<ActionResult<GeneralOverview>> GetOverview()
        {
            var ideasCount = _ideas.Count();
            var profilesCount = _context.Profiles.Count();
            var announcementsCount = _context.Announcements.Count();
            var servicesCount = _context.Services.Count();
            var overView = new GeneralOverview
            {
                Ideas = ideasCount,
                Anoncements = announcementsCount,
                Users = profilesCount,
                Services = servicesCount
            };
            return Ok(overView);
        }
    }
}