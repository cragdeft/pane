using System;
using System.Linq;
using AplombTech.DWasa.Model.Models;
using AplombTech.DWasa.Service;
using AplombTech.DWasa.Service.Interfaces;
using Moq;
using NUnit.Core;
using NUnit.Framework;
using Repository.Pattern.UnitOfWork;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace AplombTech.DWasa.Test.Steps
{
    [Binding]
    public class AddZoneSteps
    {
        public Mock<IConfigurationService> ConfigurationService { get; set; }
        public Zone Zone { get; set; }

        [Given(@"I have entered following property")]
        public void GivenIHaveEnteredFollowingProperty(Table table)
        {
            ConfigurationService = new Mock<IConfigurationService>();
            foreach (string zoneName in table.Rows.Select(row => row["value"]))
            {
                ScenarioContext.Current.Set<Zone>(table.CreateInstance<Zone>());
            }
        }
        
        
        [When(@"I Add")]
        public void WhenIAdd()
        {
            Zone = ScenarioContext.Current.Get<Zone>();
        }
        

        [Then(@"I will check the Zone name")]
        public void ThenIWillCheckTheZoneName()
        {
            Assert.AreEqual(Zone.Name, "Zone 8");
        }

        [Then(@"I will check the Zone name and DMA name")]
        public void ThenIWillCheckTheZoneNameAndDMAName()
        {
            ScenarioContext.Current.Pending();
        }



    }
}
