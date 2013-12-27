using System;
using System.Collections.Generic;
using Csla.Rules;
using Csla.Core;

namespace Whc.UnitTesting.Rules
{
    public class EnumRequired<TEnum> : BusinessRule where TEnum : IComparable
    {
        readonly TEnum _emptyValue;

        public EnumRequired(IPropertyInfo primaryProperty, TEnum emptyValue)
            : base(primaryProperty)
        {
            InputProperties = new List<IPropertyInfo> {primaryProperty};
            _emptyValue = emptyValue;
        }

        protected override void Execute(RuleContext context)
        {
            object value = context.InputPropertyValues[PrimaryProperty];
            if ((Enum.IsDefined(typeof(TEnum), value) == false) ||
                (value.Equals(_emptyValue)))
            {
                context.AddErrorResult(String.Format("{0} required.", PrimaryProperty.FriendlyName));
            }
        }
    }
}
