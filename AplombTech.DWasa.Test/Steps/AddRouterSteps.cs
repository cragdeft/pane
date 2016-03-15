using System;
using AplombTech.DWasa.Model.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace AplombTech.DWasa.Test.Steps
{
    [Binding]
    public class AddRouterSteps
    {
        public Router Router { get; set; }
        [Given(@"I have entered Router with following property")]
        public void GivenIHaveEnteredRouterWithFollowingProperty(Table table)
        {
            ScenarioContext.Current.Set<Router>(table.CreateInstance<Router>());
        }
        
        [When(@"I add Router")]
        public void WhenIAddRouter()
        {
            Router = ScenarioContext.Current.Get<Router>();
        }
        
        [Then(@"I will check the Router name")]
        public void ThenIWillCheckTheRouterName()
        {
            Assert.AreEqual(Router.Name, "Router");
        }
    }
}
