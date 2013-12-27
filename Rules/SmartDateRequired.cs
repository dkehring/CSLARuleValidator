using Csla;
using Csla.Core;
using Csla.Rules;
using Csla.Rules.CommonRules;
using System.Collections.Generic;

namespace Whc.UnitTesting.Rules
{
    public class SmartDateRequired : CommonBusinessRule
    {
        public SmartDateRequired(IPropertyInfo primaryProperty)
            : base(primaryProperty)
        {
            InputProperties = new List<IPropertyInfo> { primaryProperty };
        }

        protected override void Execute(RuleContext context)
        {
            var date = (SmartDate)context.InputPropertyValues[PrimaryProperty];
            if ((date == null) || ((date != null) && (date.IsEmpty)))
            {
                var message = string.Format("{0} required.", PrimaryProperty.FriendlyName);
                context.Results.Add(new RuleResult(RuleName, PrimaryProperty, message) { Severity = Severity });
            }
        }
    }
}
