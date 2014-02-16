using System;
using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using Csla;
using Csla.Core;
using Csla.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Whc.UnitTesting
{
    public class RuleValidator<T> where T : BusinessBase
    {
        public static bool IsRuleBroken(T obj, string propertyName)
        {
            var rule = obj.BrokenRulesCollection.GetFirstBrokenRule(propertyName);
            return (rule != null);
        }

        public static void CheckSmartDateRequired(T obj, Expression<Func<T, object>> propertyLambdaExpression, bool isRequired, SmartDate.EmptyValue emptyValue)
        {
            PropertyInfo reflectedPropertyInfo = Reflect<T>.GetProperty(propertyLambdaExpression);
            CheckSmartDateRequired(obj, reflectedPropertyInfo.Name, isRequired, emptyValue);
        }

        public static void CheckSmartDateRequired(T obj, string propertyName, bool isRequired, SmartDate.EmptyValue emptyValue)
        {
            // First, make sure the object is valid.
            Assert.IsTrue(obj.IsValid, "[SmartDateRequired] : The object must be valid before the rule can be checked.");

            // Get the SmartDate value
            var sd = new SmartDate(Utilities.CallByName(obj, propertyName, CallType.Get).ToString());

            // Check what happens when the string value is empty.
            Utilities.CallByName(obj, propertyName, CallType.Set, sd.EmptyIsMin ? DateTime.MinValue.Date.ToString(CultureInfo.InvariantCulture) : DateTime.MaxValue.Date.ToString(CultureInfo.InvariantCulture));
            if (isRequired)
            {
                // The string is required so we should have a broken rule.
                Assert.IsTrue(IsRuleBroken(obj, propertyName),
                    String.Format("[SmartDateRequired] : Object should not be valid. You are missing a ValidationRule to require a value for property [{0}].", propertyName));
            }
            else
            {
                // The string is NOT required so we should NOT have a broken rule.
                Assert.IsFalse(IsRuleBroken(obj, propertyName),
                    String.Format("[SmartDateRequired] : Object should be valid. You have a ValidationRule to require a value for property [{0}].", propertyName));
            }

            // Check what happens when the string value is NOT empty.
            Utilities.CallByName(obj, propertyName, CallType.Set, DateTime.Now.Date.ToString(CultureInfo.InvariantCulture));
            // The string is required so we should have a broken rule.
            Assert.IsFalse(IsRuleBroken(obj, propertyName),
                String.Format("[SmartDateRequired] : Object should be valid. A broken rule is not being cleared for property [{0}].", propertyName));
        }

        public static void CheckDateRequired(T obj, Expression<Func<T, object>> propertyLambdaExpression, bool isRequired)
        {
            PropertyInfo reflectedPropertyInfo = Reflect<T>.GetProperty(propertyLambdaExpression);
            CheckDateRequired(obj, reflectedPropertyInfo.Name, isRequired);
        }

        public static void CheckDateRequired(T obj, string propertyName, bool isRequired)
        {
            // First, make sure the object is valid.
            Assert.IsTrue(obj.IsValid, "[DateRequired] : The object must be valid before the rule can be checked.");

            Utilities.CallByName(obj, propertyName, CallType.Set, DateTime.MinValue);

            if (isRequired)
            {
                // The date is required so we should have a broken rule.
                Assert.IsTrue(IsRuleBroken(obj, propertyName),
                    String.Format(
                        "[DateRequired] : Object should not be valid. You are missing a ValidationRule to require a value for property [{0}].",
                        propertyName));
            }
            else
            {
                // The date is NOT required so we should NOT have a broken rule.
                Assert.IsFalse(IsRuleBroken(obj, propertyName),
                    String.Format(
                        "[DateRequired] : Object should be valid. You have a ValidationRule to require a value for property [{0}].",
                        propertyName));
            }

            // Check what happens when the date value is NOT empty.
            Utilities.CallByName(obj, propertyName, CallType.Set,
                DateTime.Now.Date);
            // The string is required so we should have a broken rule.
            Assert.IsFalse(IsRuleBroken(obj, propertyName),
                String.Format(
                    "[DateRequired] : Object should be valid. A broken rule is not being cleared for property [{0}].",
                    propertyName));
        }

        public static void CheckEmailRequired(T obj, Expression<Func<T, object>> propertyLambdaExpression, bool isRequired)
        {
            PropertyInfo reflectedPropertyInfo = Reflect<T>.GetProperty(propertyLambdaExpression);
            CheckStringRequired(obj,
                reflectedPropertyInfo.Name,
                "x@x.com",
                isRequired);
        }

        public static void CheckEmailRequired(T obj, string propertyName, bool isRequired)
        {
            CheckStringRequired(obj, propertyName, "x@x.com", isRequired);
        }

        public static void CheckStringRequired(T obj, Expression<Func<T, object>> propertyLambdaExpression, bool isRequired)
        {
            PropertyInfo reflectedPropertyInfo = Reflect<T>.GetProperty(propertyLambdaExpression);
            CheckStringRequired(obj, reflectedPropertyInfo.Name, "X", isRequired);
        }

        public static void CheckStringRequired(T obj, string propertyName, bool isRequired)
        {
            CheckStringRequired(obj, propertyName, "X", isRequired);
        }

        internal static void CheckStringRequired(T obj, string propertyName, string validValue, bool isRequired)
        {
            // First, make sure the object is valid.
            Assert.IsTrue(obj.IsValid, "[StringRequired] : The object must be valid before the rule can be checked.");

            // Check what happens when the string value is empty.
            Utilities.CallByName(obj, propertyName, CallType.Set, "");
            if (isRequired)
            {
                // The string is required so we should have a broken rule.
                Assert.IsTrue(IsRuleBroken(obj, propertyName),
                    String.Format("[StringRequired] : Object should not be valid. You are missing a ValidationRule to require a value for property [{0}].", propertyName));
            }
            else
            {
                // The string is NOT required so we should NOT have a broken rule.
                Assert.IsFalse(IsRuleBroken(obj, propertyName),
                    String.Format("[StringRequired] : Object should be valid. You have a ValidationRule to require a value for property [{0}].", propertyName));
            }

            // Check what happens when the string value is NOT empty.
            Utilities.CallByName(obj, propertyName, CallType.Set, validValue);
            // The string is required so we should have a broken rule.
            Assert.IsFalse(IsRuleBroken(obj, propertyName),
                String.Format("[StringRequired] : Object should be valid. A broken rule is not being cleared for property [{0}].", propertyName));
        }

        public static void CheckIntRequired(T obj, Expression<Func<T, object>> propertyLambdaExpression, bool isRequired)
        {
            PropertyInfo reflectedPropertyInfo = Reflect<T>.GetProperty(propertyLambdaExpression);
            CheckIntRequired(obj, reflectedPropertyInfo.Name, isRequired);
        }

        public static void CheckIntRequired(T obj, string propertyName, bool isRequired)
        {
            // First, make sure the object is valid.
            Assert.IsTrue(obj.IsValid, "[IntRequired] : The object must be valid before the rule can be checked.");

            // Check what happens when the value is zero.
            Utilities.CallByName(obj, propertyName, CallType.Set, 0);
            if (isRequired)
            {
                // The value is required so we should have a broken rule.
                Assert.IsTrue(IsRuleBroken(obj, propertyName),
                    String.Format("[IntRequired] : Object should not be valid. You are missing a ValidationRule to require a value for property [{0}].", propertyName));
            }
            else
            {
                // The string is NOT required so we should NOT have a broken rule.
                Assert.IsFalse(IsRuleBroken(obj, propertyName),
                    String.Format("[IntRequired] : Object should be valid. You have a ValidationRule to require a value for property [{0}].", propertyName));
            }

            // Check what happens when the string value is NOT empty.
            Utilities.CallByName(obj, propertyName, CallType.Set, 1);
            // The string is required so we should have a broken rule.
            Assert.IsFalse(IsRuleBroken(obj, propertyName),
                String.Format("[IntRequired] : Object should be valid. A broken rule is not being cleared for property [{0}].", propertyName));
        }

        public static void CheckDecimalRequired(T obj, string propertyName, bool isRequired)
        {
            // First, make sure the object is valid.
            Assert.IsTrue(obj.IsValid, "[DecimalRequired] : The object must be valid before the rule can be checked.");

            // Check what happens when the value is zero.
            Utilities.CallByName(obj, propertyName, CallType.Set, default(T));
            if (isRequired)
            {
                // The value is required so we should have a broken rule.
                Assert.IsTrue(IsRuleBroken(obj, propertyName),
                    String.Format("[DecimalRequired] : Object should not be valid. You are missing a ValidationRule to require a value for property [{0}].", propertyName));
            }
            else
            {
                // The string is NOT required so we should NOT have a broken rule.
                Assert.IsFalse(IsRuleBroken(obj, propertyName),
                    String.Format("[DecimalRequired] : Object should be valid. You have a ValidationRule to require a value for property [{0}].", propertyName));
            }

            // Check what happens when the string value is NOT empty.
            Utilities.CallByName(obj, propertyName, CallType.Set, 1.0m);
            // The string is required so we should have a broken rule.
            Assert.IsFalse(IsRuleBroken(obj, propertyName),
                String.Format("[DecimalRequired] : Object should be valid. A broken rule is not being cleared for property [{0}].", propertyName));
        }

        public static void CheckNumberRequired(T obj, Expression<Func<T, object>> propertyLambdaExpression, bool isRequired)
        {
            PropertyInfo reflectedPropertyInfo = Reflect<T>.GetProperty(propertyLambdaExpression);
            CheckDecimalRequired(obj, reflectedPropertyInfo.Name, isRequired);
        }

        public static void CheckNumberRequired(T obj, string propertyName, bool isRequired)
        {
            // First, make sure the object is valid.
            Assert.IsTrue(obj.IsValid, "[NumberRequired] : The object must be valid before the rule can be checked.");

            // Check what happens when the value is zero.
            Utilities.CallByName(obj, propertyName, CallType.Set, default(T));
            if (isRequired)
            {
                // The value is required so we should have a broken rule.
                Assert.IsTrue(IsRuleBroken(obj, propertyName),
                    String.Format("[NumberRequired] : Object should not be valid. You are missing a ValidationRule to require a value for property [{0}].", propertyName));
            }
            else
            {
                // The string is NOT required so we should NOT have a broken rule.
                Assert.IsFalse(IsRuleBroken(obj, propertyName),
                    String.Format("[NumberRequired] : Object should be valid. You have a ValidationRule to require a value for property [{0}].", propertyName));
            }

            // Check what happens when the string value is NOT empty.
            Utilities.CallByName(obj, propertyName, CallType.Set, 1);
            // The string is required so we should have a broken rule.
            Assert.IsFalse(IsRuleBroken(obj, propertyName),
                String.Format("[NumberRequired] : Object should be valid. A broken rule is not being cleared for property [{0}].", propertyName));
        }

        public static void CheckDecimalRequired(T obj, Expression<Func<T, object>> propertyLambdaExpression, bool isRequired)
        {
            PropertyInfo reflectedPropertyInfo = Reflect<T>.GetProperty(propertyLambdaExpression);
            CheckDecimalRequired(obj, reflectedPropertyInfo.Name, isRequired);
        }

        public static void CheckBitmapRequired(T obj, string propertyName, bool isRequired)
        {
            // First, make sure the object is valid.
            Assert.IsTrue(obj.IsValid, "[BitmapRequired] : The object must be valid before the rule can be checked.");

            // Check what happens when the image is null.
            Utilities.CallByName(obj, propertyName, CallType.Set, new object[] { null });
            if (isRequired)
            {
                // The string is required so we should have a broken rule.
                Assert.IsTrue(IsRuleBroken(obj, propertyName),
                    String.Format("[BitmapRequired] : Object should not be valid. You are missing a ValidationRule to require a value for property [{0}].", propertyName));
            }
            else
            {
                // The string is NOT required so we should NOT have a broken rule.
                Assert.IsFalse(IsRuleBroken(obj, propertyName),
                    String.Format("[BitmapRequired] : Object should be valid. You have a ValidationRule to require a value for property [{0}].", propertyName));
            }

            // Check what happens when the string value is NOT empty.
            Utilities.CallByName(obj, propertyName, CallType.Set, new System.Drawing.Bitmap(10, 10));
            // The string is required so we should have a broken rule.
            Assert.IsFalse(IsRuleBroken(obj, propertyName),
                String.Format("[BitmapRequired] : Object should be valid. A broken rule is not being cleared for property [{0}].", propertyName));
        }

        public static void CheckStringMaxLength(T obj, Expression<Func<T, object>> propertyLambdaExpression, int maxLength)
        {
            PropertyInfo reflectedPropertyInfo = Reflect<T>.GetProperty(propertyLambdaExpression);
            CheckStringMaxLength(obj, reflectedPropertyInfo.Name, maxLength, "X", new String('X', maxLength), new String('X', maxLength + 1));
        }

        public static void CheckStringMaxLength(T obj, string propertyName, int maxLength)
        {
            CheckStringMaxLength(obj, propertyName, maxLength, "X", new String('X', maxLength), new String('X', maxLength + 1));
        }

        internal static void CheckStringMaxLength(T obj, string propertyName, int maxLength, string shortValue, string maxLengthValue, string longValue)
        {
            // First, make sure the object is valid.
            Assert.IsTrue(obj.IsValid, "[CheckStringMaxLength] : The object must be valid before the rule can be checked.");

            // Make sure we have a valid string by setting at least one character
            Utilities.CallByName(obj, propertyName, CallType.Set, shortValue);
            Assert.IsFalse(IsRuleBroken(obj, propertyName), "[CheckStringMaxLength] : Property should not be broken. Make sure the object is valid first.");


            // Set the length to the max and make sure we're still valid.
            Utilities.CallByName(obj, propertyName, CallType.Set, maxLengthValue);
            Assert.IsFalse(IsRuleBroken(obj, propertyName),
                String.Format("[CheckStringMaxLength] : Property [{0}] should not be broken at the maximum length {1}. Check your ValidationRule.", propertyName, maxLength));

            // Now set the length to the max + 1 and make sure we're broken now.
            Utilities.CallByName(obj, propertyName, CallType.Set, longValue);
            Assert.IsTrue(IsRuleBroken(obj, propertyName),
                String.Format("[CheckStringMaxLength] : Property [{0}] should be broken at {1} characters. Check your ValidationRule.", propertyName, maxLength + 1));

            // Finally, set the length back to the max and make sure the broken rule is cleared.
            Utilities.CallByName(obj, propertyName, CallType.Set, maxLengthValue);
            Assert.IsFalse(IsRuleBroken(obj, propertyName),
                String.Format("[CheckStringMaxLength] : Property [{0}] should not be broken at the maximum length {1}. The validation rule is not being cleared.", propertyName, maxLength));
        }

        public static void CheckEmailMaxLength(T obj, Expression<Func<T, object>> propertyLambdaExpression, int maxLength)
        {
            PropertyInfo reflectedPropertyInfo = Reflect<T>.GetProperty(propertyLambdaExpression);
            CheckStringMaxLength(obj,
                reflectedPropertyInfo.Name,
                maxLength,
                "x@x.com",
                String.Format("{0}@x.com", new string('x', maxLength - 6)),
                String.Format("{0}@x.com", new string('x', maxLength - 5)));
        }

        public static void CheckEmailMaxLength(T obj, string propertyName, int maxLength)
        {
            CheckStringMaxLength(obj,
                propertyName,
                maxLength,
                "x@x.com",
                String.Format("{0}@x.x", new string('x', maxLength - 6)),
                String.Format("{0}@x.x", new string('x', maxLength - 5)));
        }

        public static void CheckEnumRules<TEnum>(T obj, Expression<Func<T, object>> propertyLambdaExpression, TEnum validValue, TEnum emptyValue, bool required)
        {
            PropertyInfo reflectedPropertyInfo = Reflect<T>.GetProperty(propertyLambdaExpression);
            CheckEnumRules(obj, reflectedPropertyInfo.Name, validValue, emptyValue, required);
        }

        public static void CheckEnumRules<TEnum>(T obj, string propertyName, TEnum validValue, TEnum emptyValue, bool required)
        {
            // First, make sure the object is valid.
            Assert.IsTrue(obj.IsValid, "[CheckEnumRules] : The object must be valid before the rule can be checked.");

            Utilities.CallByName(obj, propertyName, CallType.Set, emptyValue);
            Assert.IsFalse(obj.IsValid, String.Format("[{0}] Object should not be valid.", propertyName));
            Utilities.CallByName(obj, propertyName, CallType.Set, validValue);
            Assert.IsTrue(obj.IsValid, String.Format("[{0}] Object should be valid. {1}", propertyName, obj.BrokenRulesCollection));
            if (required)
            {
                Utilities.CallByName(obj, propertyName, CallType.Set, emptyValue);
                Assert.IsFalse(obj.IsValid, String.Format("[{0}, Required] Object should not be valid if the value is required.", propertyName));
                Utilities.CallByName(obj, propertyName, CallType.Set, validValue);
                Assert.IsTrue(obj.IsValid, String.Format("[{0}, Required] Object should be valid. {1}", propertyName, obj.BrokenRulesCollection));
            }
            else
            {
                Utilities.CallByName(obj, propertyName, CallType.Set, emptyValue);
                Assert.IsTrue(obj.IsValid, String.Format("[{0}, Not Required] Object should be valid if the value is NOT required. {1}", propertyName, obj.BrokenRulesCollection));
            }
            // Leave the object in a valid state.
            Assert.IsTrue(obj.IsValid, String.Format("[{0}] Object should be valid. {1}", propertyName, obj.BrokenRulesCollection));
        }

        public static void CheckGuidRules(T obj, Expression<Func<T, object>> propertyLambdaExpression, bool required)
        {
            PropertyInfo reflectedPropertyInfo = Reflect<T>.GetProperty(propertyLambdaExpression);
            CheckGuidRules(obj, reflectedPropertyInfo.Name, required);
        }

        public static void CheckGuidRules(T obj, string propertyName, bool required)
        {
            // First, make sure the object is valid.
            Assert.IsTrue(obj.IsValid, "[CheckGuidRules] : The object must be valid before the rule can be checked.");

            if (required)
            {
                Utilities.CallByName(obj, propertyName, CallType.Set, Guid.Empty);
                Assert.IsFalse(obj.IsValid, String.Format("[{0}, Required] Object should not be valid if the value is required.", propertyName));
                Utilities.CallByName(obj, propertyName, CallType.Set, Guid.NewGuid());
                Assert.IsTrue(obj.IsValid, String.Format("[{0}, Required] Object should be valid. {1}", propertyName, obj.BrokenRulesCollection));
            }
            else
            {
                Utilities.CallByName(obj, propertyName, CallType.Set, Guid.Empty);
                Assert.IsTrue(obj.IsValid, String.Format("[{0}, Not Required] Object should be valid if the value is NOT required.", propertyName));
            }
            // Leave the object in a valid state.
            Assert.IsTrue(obj.IsValid, String.Format("[{0}] Object should be valid. {1}", propertyName, obj.BrokenRulesCollection));
        }

        public static void CheckObjectRequiredRules(T obj, IPropertyInfo propertyInfo, T nonEmptyValue, bool required)
        {
            CheckObjectRequiredRules(obj, propertyInfo.Name, nonEmptyValue, required);
        }

        public static void CheckObjectRequiredRules(T obj, string propertyName, T nonEmptyValue, bool required)
        {
            // First, make sure the object is valid.
            Assert.IsTrue(obj.IsValid, "[CheckObjectRequiredRules] : The object must be valid before the rule can be checked.");

            if (required)
            {
                Utilities.CallByName(obj, propertyName, CallType.Set, new object[] { null });
                Assert.IsFalse(obj.IsValid, String.Format("[{0}, Required] Object should not be valid if the value is required.", propertyName));
                Utilities.CallByName(obj, propertyName, CallType.Set, nonEmptyValue);
                Assert.IsTrue(obj.IsValid, String.Format("[{0}, Required] Object should be valid. {1}", propertyName, obj.BrokenRulesCollection));
            }
            else
            {
                Utilities.CallByName(obj, propertyName, CallType.Set, new object[] { null });
                Assert.IsTrue(obj.IsValid, String.Format("[{0}, Not Required] Object should be valid if the value is NOT required.", propertyName));
            }
            // Leave the object in a valid state.
            Assert.IsTrue(obj.IsValid, String.Format("[{0}] Object should be valid. {1}", propertyName, obj.BrokenRulesCollection));
        }

        public static void CheckIntMinMaxRules(T obj, Expression<Func<T, object>> propertyLambdaExpression, int min, int max)
        {
            PropertyInfo reflectedPropertyInfo = Reflect<T>.GetProperty(propertyLambdaExpression);
            CheckIntMinMaxRules(obj, reflectedPropertyInfo.Name, min, max);
        }

        public static void CheckIntMinMaxRules(T obj, string propertyName, int min, int max)
        {
            // First, make sure the object is valid.
            Assert.IsTrue(obj.IsValid, "[CheckIntMinMaxRules] : The object must be valid before the rule can be checked.");

            if (min != int.MinValue)
            {
                Utilities.CallByName(obj, propertyName, CallType.Set, min - 1);
                Assert.IsFalse(obj.IsValid, String.Format("[{0}] Object should not be valid.", propertyName));
                Utilities.CallByName(obj, propertyName, CallType.Set, min);
                Assert.IsTrue(obj.IsValid, String.Format("[{0}] Object should be valid. {1}", propertyName, obj.BrokenRulesCollection));
            }
            if (max != int.MaxValue)
            {
                Utilities.CallByName(obj, propertyName, CallType.Set, max + 1);
                Assert.IsFalse(obj.IsValid, String.Format("[{0}] Object should not be valid.", propertyName));
                Utilities.CallByName(obj, propertyName, CallType.Set, max);
                Assert.IsTrue(obj.IsValid, String.Format("[{0}] Object should be valid. {1}", propertyName, obj.BrokenRulesCollection));
            }
            // Leave the object in a valid state.
            Assert.IsTrue(obj.IsValid, String.Format("[{0}] Object should be valid. {1}", propertyName, obj.BrokenRulesCollection));
        }

        public static void CheckIntMinExclusiveRules(T obj, string propertyName, int min)
        {
            // First, make sure the object is valid.
            Assert.IsTrue(obj.IsValid, "[CheckIntMinExclusiveRules] : The object must be valid before the rule can be checked.");

            if (min != int.MinValue)
            {
                Utilities.CallByName(obj, propertyName, CallType.Set, min - 1);
                Assert.IsFalse(obj.IsValid, String.Format("[{0}] Object should not be valid.", propertyName));
                Utilities.CallByName(obj, propertyName, CallType.Set, min + 1);
                Assert.IsTrue(obj.IsValid, String.Format("[{0}] Object should be valid. {1}", propertyName, obj.BrokenRulesCollection));
                Utilities.CallByName(obj, propertyName, CallType.Set, min);
                Assert.IsFalse(obj.IsValid, String.Format("[{0}] Object should not be valid.", propertyName));
                Utilities.CallByName(obj, propertyName, CallType.Set, min + 1);
                Assert.IsTrue(obj.IsValid, String.Format("[{0}] Object should be valid. {1}", propertyName, obj.BrokenRulesCollection));
            }
            // Leave the object in a valid state.
            Assert.IsTrue(obj.IsValid, String.Format("[{0}] Object should be valid. {1}", propertyName, obj.BrokenRulesCollection));
        }

        public static void CheckIntMaxExclusiveRules(T obj, string propertyName, int max)
        {
            // First, make sure the object is valid.
            Assert.IsTrue(obj.IsValid, "[CheckEnumRules] : The object must be valid before the rule can be checked.");

            if (max != int.MaxValue)
            {
                Utilities.CallByName(obj, propertyName, CallType.Set, max + 1);
                Assert.IsFalse(obj.IsValid, String.Format("[{0}] Object should not be valid.", propertyName));
                Utilities.CallByName(obj, propertyName, CallType.Set, max - 1);
                Assert.IsTrue(obj.IsValid, String.Format("[{0}] Object should be valid. {1}", propertyName, obj.BrokenRulesCollection));
                Utilities.CallByName(obj, propertyName, CallType.Set, max);
                Assert.IsFalse(obj.IsValid, String.Format("[{0}] Object should not be valid.", propertyName));
                Utilities.CallByName(obj, propertyName, CallType.Set, max - 1);
                Assert.IsTrue(obj.IsValid, String.Format("[{0}] Object should be valid. {1}", propertyName, obj.BrokenRulesCollection));
            }
            // Leave the object in a valid state.
            Assert.IsTrue(obj.IsValid, String.Format("[{0}] Object should be valid. {1}", propertyName, obj.BrokenRulesCollection));
        }

        public static void CheckDoubleMinMaxRules(T obj, Expression<Func<T, object>> propertyLambdaExpression, double min, double max)
        {
            PropertyInfo reflectedPropertyInfo = Reflect<T>.GetProperty(propertyLambdaExpression);
            CheckDoubleMinMaxRules(obj, reflectedPropertyInfo.Name, min, max);
        }

        public static void CheckDoubleMinMaxRules(T obj, string propertyName, double min, double max)
        {
            // First, make sure the object is valid.
            Assert.IsTrue(obj.IsValid, "[CheckDoubleMinMaxRules] : The object must be valid before the rule can be checked.");

            if (Math.Abs(min - double.MinValue) > double.Epsilon)
            {
                Utilities.CallByName(obj, propertyName, CallType.Set, min - 1);
                Assert.IsFalse(obj.IsValid, String.Format("[{0}] Object should not be valid.", propertyName));
                Utilities.CallByName(obj, propertyName, CallType.Set, min);
                Assert.IsTrue(obj.IsValid, String.Format("[{0}] Object should be valid. {1}", propertyName, obj.BrokenRulesCollection));
            }
            if (Math.Abs(max - double.MaxValue) > double.Epsilon)
            {
                Utilities.CallByName(obj, propertyName, CallType.Set, max + 1);
                Assert.IsFalse(obj.IsValid, String.Format("[{0}] Object should not be valid.", propertyName));
                Utilities.CallByName(obj, propertyName, CallType.Set, max);
                Assert.IsTrue(obj.IsValid, String.Format("[{0}] Object should be valid. {1}", propertyName, obj.BrokenRulesCollection));
            }
            // Leave the object in a valid state.
            Assert.IsTrue(obj.IsValid, String.Format("[{0}] Object should be valid. {1}", propertyName, obj.BrokenRulesCollection));
        }

        public static void CheckDecimalMinMaxRules(T obj, Expression<Func<T, object>> propertyLambdaExpression, decimal min, decimal max)
        {
            PropertyInfo reflectedPropertyInfo = Reflect<T>.GetProperty(propertyLambdaExpression);
            CheckDecimalMinMaxRules(obj, reflectedPropertyInfo.Name, min, max);
        }

        public static void CheckDecimalMinMaxRules(T obj, string propertyName, decimal min, decimal max)
        {
            // First, make sure the object is valid.
            Assert.IsTrue(obj.IsValid, "[CheckDecimalMinMaxRules] : The object must be valid before the rule can be checked.");

            if (min != decimal.MinValue)
            {
                Utilities.CallByName(obj, propertyName, CallType.Set, min - 1);
                Assert.IsFalse(obj.IsValid, String.Format("[{0}] Object should not be valid.", propertyName));
                Utilities.CallByName(obj, propertyName, CallType.Set, min);
                Assert.IsTrue(obj.IsValid, String.Format("[{0}] Object should be valid. {1}", propertyName, obj.BrokenRulesCollection));
            }
            if (max != decimal.MaxValue)
            {
                Utilities.CallByName(obj, propertyName, CallType.Set, max + 1);
                Assert.IsFalse(obj.IsValid, String.Format("[{0}] Object should not be valid.", propertyName));
                Utilities.CallByName(obj, propertyName, CallType.Set, max);
                Assert.IsTrue(obj.IsValid, String.Format("[{0}] Object should be valid. {1}", propertyName, obj.BrokenRulesCollection));
            }
            // Leave the object in a valid state.
            Assert.IsTrue(obj.IsValid, String.Format("[{0}] Object should be valid. {1}", propertyName, obj.BrokenRulesCollection));
        }

        public static void CheckByteRequired(T obj, Expression<Func<T, object>> propertyLambdaExpression, bool isRequired)
        {
            PropertyInfo reflectedPropertyInfo = Reflect<T>.GetProperty(propertyLambdaExpression);
            CheckByteRequired(obj, reflectedPropertyInfo.Name, isRequired);
        }

        public static void CheckByteRequired(T obj, string propertyName, bool isRequired)
        {
            // First, make sure the object is valid.
            Assert.IsTrue(obj.IsValid, "[ByteRequired] : The object must be valid before the rule can be checked.");

            // Check what happens when the value is zero.
            Utilities.CallByName(obj, propertyName, CallType.Set, (Byte)0);
            if (isRequired)
            {
                // The value is required so we should have a broken rule.
                Assert.IsTrue(IsRuleBroken(obj, propertyName),
                    String.Format("[ByteRequired] : Object should not be valid. You are missing a ValidationRule to require a value for property [{0}].", propertyName));
            }
            else
            {
                // The string is NOT required so we should NOT have a broken rule.
                Assert.IsFalse(IsRuleBroken(obj, propertyName),
                    String.Format("[ByteRequired] : Object should be valid. You have a ValidationRule to require a value for property [{0}].", propertyName));
            }

            // Check what happens when the string value is NOT empty.
            Utilities.CallByName(obj, propertyName, CallType.Set, (Byte)1);
            // The string is required so we should have a broken rule.
            Assert.IsFalse(IsRuleBroken(obj, propertyName),
                String.Format("[ByteRequired] : Object should be valid. A broken rule is not being cleared for property [{0}].", propertyName));
        }


        public static void CheckByteMinMaxRules(T obj, Expression<Func<T, object>> propertyLambdaExpression, byte min, byte max)
        {
            PropertyInfo reflectedPropertyInfo = Reflect<T>.GetProperty(propertyLambdaExpression);
            CheckByteMinMaxRules(obj, reflectedPropertyInfo.Name, min, max);
        }

        public static void CheckByteMinMaxRules(T obj, string propertyName, byte min, byte max)
        {
            // First, make sure the object is valid.
            Assert.IsTrue(obj.IsValid, "[CheckByteMinMaxRules] : The object must be valid before the rule can be checked.");

            byte val;
            if (min != byte.MinValue)
            {
                val = min;
                val--;
                Utilities.CallByName(obj, propertyName, CallType.Set, val);
                Assert.IsFalse(obj.IsValid, String.Format("[{0}] Object should not be valid.", propertyName));
                Utilities.CallByName(obj, propertyName, CallType.Set, min);
                Assert.IsTrue(obj.IsValid, String.Format("[{0}] Object should be valid. {1}", propertyName, obj.BrokenRulesCollection));
            }
            if (max != byte.MaxValue)
            {
                val = max;
                val++;
                Utilities.CallByName(obj, propertyName, CallType.Set, val);
                Assert.IsFalse(obj.IsValid, String.Format("[{0}] Object should not be valid.", propertyName));
                Utilities.CallByName(obj, propertyName, CallType.Set, max);
                Assert.IsTrue(obj.IsValid, String.Format("[{0}] Object should be valid. {1}", propertyName, obj.BrokenRulesCollection));
            }
            // Leave the object in a valid state.
            Assert.IsTrue(obj.IsValid, String.Format("[{0}] Object should be valid. {1}", propertyName, obj.BrokenRulesCollection));
        }

        public static string RandomString(int size)
        {
            var builder = new StringBuilder();
            var random = new Random();
            for (var i = 0; i < size; i++)
            {
                var ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            return builder.ToString();
        }

    }
}
