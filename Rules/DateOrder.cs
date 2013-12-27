using System;
using System.Collections.Generic;
using Csla.Rules;
using Csla.Core;
using Csla.Rules.CommonRules;

namespace Whc.UnitTesting.Rules
{
    public class DateOrder : CommonBusinessRule
    {
        public IPropertyInfo LowerDateProperty { get; set; }
        public IPropertyInfo UpperDateProperty { get; set; }

        public DateOrder(IPropertyInfo lowerDateProperty, IPropertyInfo upperDateProperty)
        {
            LowerDateProperty = lowerDateProperty;
            UpperDateProperty = upperDateProperty;
            InputProperties = new List<IPropertyInfo> { lowerDateProperty, upperDateProperty };
        }

        protected override void Execute(RuleContext context)
        {
            var lowerDateNullable = (DateTime?)context.InputPropertyValues[LowerDateProperty];
            var upperDateNullable = (DateTime?)context.InputPropertyValues[UpperDateProperty];

            var lowerDate = lowerDateNullable ?? DateTime.MinValue;
            var upperDate = upperDateNullable ?? DateTime.MinValue;

            if (lowerDate.CompareTo(upperDate) > 0)
            {
                var message = String.Format("{0} cannot be later than {1}.", LowerDateProperty.FriendlyName, UpperDateProperty.FriendlyName);
                context.Results.Add(new RuleResult(RuleName, PrimaryProperty, message) { Severity = Severity });
            }
        }
    }
}
