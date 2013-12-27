using System;
using System.Collections.Generic;
using Csla.Rules.CommonRules;
using Csla.Core;

namespace Whc.UnitTesting.Rules
{
    public class GuidRequired : CommonBusinessRule
    {
        public GuidRequired(IPropertyInfo primaryProperty) : base(primaryProperty)
        {
            InputProperties = new List<IPropertyInfo> {primaryProperty};
        }

        protected override void Execute(Csla.Rules.RuleContext context)
        {
            var id = (Guid)context.InputPropertyValues[PrimaryProperty];
            if ((id == null) || (id == Guid.Empty))
            {
                var message = String.Format("{0} is required.", PrimaryProperty.FriendlyName);
                context.AddErrorResult(message);
            }
        }
    }
}
