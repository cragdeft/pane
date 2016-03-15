using System;
using AplombTech.DWasa.Model.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace AplombTech.DWasa.Test.Steps
{
    [Binding]
    public class AddCameraSteps
    {
        public Camera Camera { get; set; }
        [Given(@"I have entered Camera Sensor with following property")]
        public void GivenIHaveEnteredCameraSensorWithFollowingProperty(Table table)
        {
            ScenarioContext.Current.Set<Camera>(table.CreateInstance<Camera>());
        }
        
        [When(@"I add Camera")]
        public void WhenIAddCamera()
        {
            Camera = ScenarioContext.Current.Get<Camera>();
        }
        
        [Then(@"I will check the Camera name")]
        public void ThenIWillCheckTheCameraName()
        {
            Assert.AreEqual(Camera.Name, "Camera");
        }
    }
}
