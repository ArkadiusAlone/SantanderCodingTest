﻿using Microsoft.AspNetCore.Mvc;
using SantanderCodingTest.Helpers;
using SantanderCodingTest.Services;
using System.Xml.Linq;

namespace SantanderCodingTest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StoriesController : ControllerBase
    {
        private readonly ILogger<StoriesController> _logger;
        private readonly IStoryService _storyService;

        public StoriesController(IStoryService storyService, ILogger<StoriesController> logger)
        {
            _storyService = storyService;
            _logger = logger;
        }

        [HttpGet("GetBestStories")]
        [Throttle(Name = "GetBestStoriesThrottle", Time = 30)]
        public async Task<IActionResult> GetBestStories(int n)
        {
            try
            {
                var bestStories = await _storyService.GetTopNBestStories(n);

                return Ok(bestStories);
            }
            catch(Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
    }
}