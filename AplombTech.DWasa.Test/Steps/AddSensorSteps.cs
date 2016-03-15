using System;
using AplombTech.DWasa.Model.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace AplombTech.DWasa.Test.Steps
{
    [Binding]
    public class AddSensorSteps
    {
        public WaterLevelSensor WaterLevelSensor { get; set; }
        [Given(@"I have entered WaterLevel Sensor with following property")]
        public void GivenIHaveEnteredWaterLevelSensorWithFollowingProperty(Table table)
        {
            ScenarioContext.Current.Set<WaterLevelSensor>(table.CreateInstance<WaterLevelSensor>());
        }
        
        [When(@"I add Sensor")]
        public void WhenIAddSensor()
        {
            WaterLevelSensor = ScenarioContext.Current.Get<WaterLevelSensor>();
        }
        
        [Then(@"I will check the Sensor name")]
        public void ThenIWillCheckTheSensorName()
        {
            Assert.AreEqual(WaterLevelSensor.Name, "WT");
        }
    }
}
