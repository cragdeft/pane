using System;
using AplombTech.DWasa.Model.Models;
using AplombTech.DWasa.Service.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace AplombTech.DWasa.Test.Steps
{
    [Binding]
    public class AddPumpStationSteps
    {
        public Mock<IConfigurationService> ConfigurationService { get; set; }
        public PumpStation PumpStation { get; set; }
        public DMA DMA { get; set; }
        [Given(@"I have entered Pump with following property")]
        public void GivenIHaveEnteredPumpWithFollowingProperty(Table table)
        {
            ConfigurationService = new Mock<IConfigurationService>();
            ScenarioContext.Current.Set<PumpStation>(table.CreateInstance<PumpStation>());
        }
        [When(@"I  add")]
        public void WhenIAdd()
        {
            PumpStation = ScenarioContext.Current.Get<PumpStation>();
        }

        [Then(@"I will check the Pump name and DMA")]
        public void ThenIWillCheckThePumpNameAndDMA()
        {
            Assert.AreEqual(PumpStation.Name, "Baridhara Pump");
        }
    }
}
