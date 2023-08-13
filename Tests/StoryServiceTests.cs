using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SantanderCodingTest.Services;
using System.IO;
using System;
using System.Net;
using System.Threading.Tasks;
using System.Linq;
using RichardSzalay.MockHttp;

namespace Tests
{
    [TestFixture]
    public class Tests
    {
        private MockHttpMessageHandler handlerMock;
        private Mock<ILogger<StoryService>> _loggerMock;
        private IStoryService _storyService;

        private string bestStoryIds;
        private string storyDetailsList;

        [SetUp]
        public void Setup()
        {
            _loggerMock = new Mock<ILogger<StoryService>>();
            handlerMock = new MockHttpMessageHandler();

            bestStoryIds = File.ReadAllText(Path.Combine(Environment.CurrentDirectory, "TestJsons/bestSeries.json"));
            storyDetailsList = File.ReadAllText(Path.Combine(Environment.CurrentDirectory, "TestJsons/storyDetails.json"));

            handlerMock.When($"https://hacker-news.firebaseio.com/v0/beststories.json")
                       .Respond(HttpStatusCode.OK, "application/json", bestStoryIds);

            handlerMock.When($"https://hacker-news.firebaseio.com/v0/item/*")
                       .Respond(HttpStatusCode.OK, "application/json", storyDetailsList);

            var client = handlerMock.ToHttpClient();
            client.BaseAddress = new Uri($"https://hacker-news.firebaseio.com");
            var storyService = new StoryService(client, _loggerMock.Object);

            _storyService = new StoryService(client, _loggerMock.Object);
        }

        [Test]
        public async Task GetAllBestSeriesIds()
        {
            var result = await _storyService.GetAllStoriesId();

            Assert.IsNotNull(result);
            Assert.AreEqual(10, result.Count());
            Assert.AreEqual(37062422, result.ToArray()[1]);
            Assert.AreEqual(37054241, result.ToList().Last());
        }

        [Test]
        public async Task GetTopNBestStories_ReturnsTopNStories()
        {
            int numberOfSeries = 5;

            var result = await _storyService.GetTopNBestStories(numberOfSeries);

            Assert.IsNotNull(result);
            Assert.AreEqual(numberOfSeries, result.Count());
            Assert.AreEqual(1320, result.First().Score);
        }
    }
}