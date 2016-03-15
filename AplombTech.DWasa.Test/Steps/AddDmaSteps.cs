using AplombTech.DWasa.Model.Models;
using AplombTech.DWasa.Service.Interfaces;
using Moq;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace AplombTech.DWasa.Test.Steps
{
    [Binding]
    public class AddDmaSteps
    {
        public Mock<IConfigurationService> ConfigurationService { get; set; }
        public Zone Zone { get; set; }
        public DMA DMA { get; set; }
        [Given(@"I have entered DMA with following property")]
        public void GivenIHaveEnteredDMAWithFollowingProperty(Table table)
        {
            ConfigurationService = new Mock<IConfigurationService>();
            ScenarioContext.Current.Set<DMA>(table.CreateInstance<DMA>());
        }
        
        
        [When(@"I press add")]
        public void WhenIPressAdd()
        {
            DMA = ScenarioContext.Current.Get<DMA>();
        }

        [Then(@"I will check the DMA name")]
        public void ThenIWillCheckTheDMAName()
        {
            Assert.AreEqual(DMA.Name, "DMA 810");
        }

    }
}
