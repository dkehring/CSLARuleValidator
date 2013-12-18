using System;
using NUnit.Framework;

namespace WHC.UnitTesting.NUnit
{
    public class TestBase
    {
        [SetUp]
        public void InitializeTest()
        {
            OnInitialize();
        }

        protected virtual void OnInitialize()
        {
        }

        [TearDown]
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
