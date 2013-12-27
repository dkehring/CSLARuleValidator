using System;
using System.Collections.Generic;
using Csla.Rules;
using Csla.Core;
using Csla.Rules.CommonRules;

namespace Whc.UnitTesting.Rules
{
    public class DateRange : CommonBusinessRule
    {
        public IPropertyInfo LowerDateProperty { get; set; }
        public IPropertyInfo UpperDateProperty { get; set; }
        public IPropertyInfo TargetDateProperty { get; set; }

        public DateRange(IPropertyInfo lowerDateProperty, IPropertyInfo upperDateProperty, IPropertyInfo targetDateProperty)
        {
            LowerDateProperty = lowerDateProperty;
            UpperDateProperty = upperDateProperty;
            TargetDateProperty = targetDateProperty;
            InputProperties = new List<IPropertyInfo> { lowerDateProperty, upperDateProperty, targetDateProperty };
        }

        protected override void Execute(RuleContext context)
        {
            var targetDateNullable = (DateTime?)context.InputPropertyValues[TargetDateProperty];
            if (targetDateNullable.HasValue == false) return;

            var lowerDateNullable = (DateTime?)context.InputPropertyValues[LowerDateProperty];
            var upperDateNullable = (DateTime?)context.InputPropertyValues[UpperDateProperty];

            var lowerDate = lowerDateNullable ?? DateTime.MinValue;
            var upperDate = upperDateNullable ?? DateTime.MinValue;
            var targetDate = targetDateNullable ?? DateTime.MinValue;

            if ((targetDate < lowerDate) || (targetDate > upperDate))
            {
                var message = String.Format("{0} must be between {1} and {2}.", TargetDateProperty.FriendlyName, LowerDateProperty.FriendlyName, UpperDateProperty.FriendlyName);
                context.Results.Add(new RuleResult(RuleName, PrimaryProperty, message) { Severity = Severity });
            }
        }
    }
}
