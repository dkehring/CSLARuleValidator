using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;

namespace WHC.UnitTesting.NUnit
{
    public class TransactionalTest : TestBase
    {
        protected TransactionScope _ts;

        protected override void OnInitialize()
        {
            base.OnInitialize();
            _ts = new TransactionScope(TransactionScopeOption.Required);
        }

        public override void OnCleanup()
        {
            _ts.Dispose();
            base.OnCleanup();
        }
    }
}
