using System;
using System.Linq;
using AplombTech.DWasa.Json;
using AplombTech.DWasa.Service.Interfaces;
using Moq;
using TechTalk.SpecFlow;

namespace AplombTech.DWasa.Test.Steps
{
    [Binding]
    public class ParseCommandForFeedbackSteps
    {
        public Mock<IJsonParserManagerService> JsonParserManagerService { get; set; }

        public LogJsonManager LogJsonManager { get; set; }

        [Given(@"I have entered following property")]
        public void GivenIHaveEnteredFollowingProperty(Table table)
        {
            foreach (string jsonString in table.Rows.Select(row => row["value"]))
            {
                JsonParserManagerService = new Mock<IJsonParserManagerService>();
                ScenarioContext.Current.Set<string>(jsonString);
            }
        }

        [Given(@"I created a parser with that string")]
        public void GivenICreatedAJsonWithThatString()
        {
            var jsonString = ScenarioContext.Current.Get<string>();
            LogJsonManager = new LogJsonManager(jsonString, JsonParserManagerService.Object);
        }

        [When(@"I parse It will save the data")]
        public void WhenIParseItWillSaveTheData()
        {
            LogJsonManager.Parse();
        }

    }
}
