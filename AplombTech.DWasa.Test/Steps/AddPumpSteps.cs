using System;
using AplombTech.DWasa.Model.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace AplombTech.DWasa.Test.Steps
{
    [Binding]
    public class AddPumpSteps
    {
        public Pump Pump { get; set; }
        [Given(@"I have entered Pump Device with following property")]
        public void GivenIHaveEnteredPumpDeviceWithFollowingProperty(Table table)
        {
            ScenarioContext.Current.Set<Pump>(table.CreateInstance<Pump>());
        }
        
        [When(@"I add Pump")]
        public void WhenIAddPump()
        {
            Pump = ScenarioContext.Current.Get<Pump>();
        }
        
        [Then(@"I will check the Pump name")]
        public void ThenIWillCheckThePumpName()
        {
            Assert.AreEqual(Pump.Name, "Pump");
        }
    }
}
