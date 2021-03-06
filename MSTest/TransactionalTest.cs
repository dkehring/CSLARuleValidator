﻿using System.Transactions;

namespace WHC.UnitTesting.MSTest
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
