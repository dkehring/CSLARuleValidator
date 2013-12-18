using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WHC.UnitTesting.MSTest
{
    public class TestBase
    {
        [TestInitialize]
        public void InitializeTest()
        {
            OnInitialize();
        }

        protected virtual void OnInitialize()
        {
        }

        [TestCleanup]
        public void Cleanup()
        {
            OnCleanup();
        }

        public virtual void OnCleanup()
        {
        }

        public string GetBrokenRules(Csla.Core.BusinessBase obj)
        {
            return String.Format("Object is not valid.\n\n{0}", obj.BrokenRulesCollection);
        }
    }
}
