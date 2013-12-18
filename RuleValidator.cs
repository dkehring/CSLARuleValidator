using System.Globalization;
using Csla;
using Csla.Reflection;
using NUnit.Framework;
using System;
using System.Linq.Expressions;
using System.Text;

namespace Whc.UnitTesting
{
    public class RuleValidator
    {
        public static bool IsRuleBroken(Csla.Core.BusinessBase obj, string propertyName)
        {
            var rule = obj.BrokenRulesCollection.GetFirstBrokenRule(propertyName);
            return (rule != null);
        }

        public static void CheckSmartDateRequired(Csla.Core.BusinessBase obj, string getterPropertyName, string setterPropertyName, bool isRequired, SmartDate.EmptyValue emptyValue)
        {
            // First, make sure the object is valid.
            Assert.IsTrue(obj.IsValid, "[SmartDateRequired] : The object must be valid before the rule can be checked.");

            // Get the SmartDate value
            var sd = new SmartDate(Utilities.CallByName(obj, getterPropertyName, CallType.Get).ToString());

            // Check what happens when the string value is empty.
            Utilities.CallByName(obj, setterPropertyName, CallType.Set, sd.EmptyIsMin ? DateTime.MinValue.Date.ToString(CultureInfo.InvariantCulture) : DateTime.MaxValue.Date.ToString(CultureInfo.InvariantCulture));
            if (isRequired)
            {
                // The string is required so we should have a broken rule.
                Assert.IsTrue(IsRuleBroken(obj, getterPropertyName),
                    String.Format("[SmartDateRequired] : Object should not be valid. You are missing a ValidationRule to require a value for property [{0}].", getterPropertyName));
            }
            else
            {
                // The string is NOT required so we should NOT have a broken rule.
                Assert.IsFalse(IsRuleBroken(obj, getterPropertyName),
                    String.Format("[SmartDateRequired] : Object should be valid. You have a ValidationRule to require a value for property [{0}].", getterPropertyName));
            }

            // Check what happens when the string value is NOT empty.
            Utilities.CallByName(obj, setterPropertyName, CallType.Set, DateTime.Now.Date.ToString(CultureInfo.InvariantCulture));
            // The string is required so we should have a broken rule.
            Assert.IsFalse(IsRuleBroken(obj, getterPropertyName),
                String.Format("[SmartDateRequired] : Object should be valid. A broken rule is not being cleared for property [{0}].", getterPropertyName));
        }

        public static void CheckEmailRequired<T>(Csla.Core.BusinessBase obj, Expression<Func<T, object>> propertyLambdaExpression, bool isRequired)
        {
            var reflectedPropertyInfo = Reflect<T>.GetProperty(propertyLambdaExpression);
            CheckStringRequired(obj, 
                reflectedPropertyInfo.Name, 
                "x@x.com", 
                isRequired);
        }

        public static void CheckEmailRequired(Csla.Core.BusinessBase obj, string propertyName, bool isRequired)
        {
            CheckStringRequired(obj, propertyName, "x@x.com", isRequired);
        }

        public static void CheckStringRequired<T>(Csla.Core.BusinessBase obj, Expression<Func<T, object>> propertyLambdaExpression, bool isRequired)
        {
            var reflectedPropertyInfo = Reflect<T>.GetProperty(propertyLambdaExpression);
            CheckStringRequired(obj, reflectedPropertyInfo.Name, "X", isRequired);
        }

        public static void CheckStringRequired(Csla.Core.BusinessBase obj, string propertyName, bool isRequired)
        {
            CheckStringRequired(obj, propertyName, "X", isRequired);
        }

        internal static void CheckStringRequired(Csla.Core.BusinessBase obj, string propertyName, string validValue, bool isRequired)
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

        public static void CheckIntRequired<T>(Csla.Core.BusinessBase obj, Expression<Func<T, object>> propertyLambdaExpression, bool isRequired)
        {
            var reflectedPropertyInfo = Reflect<T>.GetProperty(propertyLambdaExpression);
            CheckIntRequired(obj, reflectedPropertyInfo.Name, isRequired);
        }

        public static void CheckIntRequired(Csla.Core.BusinessBase obj, string propertyName, bool isRequired)
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

        public static void CheckDecimalRequired<T>(Csla.Core.BusinessBase obj, string propertyName, bool isRequired)
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

        public static void CheckNumberRequired<T>(Csla.Core.BusinessBase obj, Expression<Func<T, object>> propertyLambdaExpression, bool isRequired)
        {
            var reflectedPropertyInfo = Reflect<T>.GetProperty(propertyLambdaExpression);
            CheckDecimalRequired<T>(obj, reflectedPropertyInfo.Name, isRequired);
        }

        public static void CheckNumberRequired<T>(Csla.Core.BusinessBase obj, string propertyName, bool isRequired)
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

        public static void CheckDecimalRequired<T>(Csla.Core.BusinessBase obj, Expression<Func<T, object>> propertyLambdaExpression, bool isRequired)
        {
            var reflectedPropertyInfo = Reflect<T>.GetProperty(propertyLambdaExpression);
            CheckDecimalRequired<T>(obj, reflectedPropertyInfo.Name, isRequired);
        }

        public static void CheckBitmapRequired(Csla.Core.BusinessBase obj, string propertyName, bool isRequired)
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

        public static void CheckStringMaxLength<T>(Csla.Core.BusinessBase obj, Expression<Func<T, object>> propertyLambdaExpression, int maxLength)
        {
            var reflectedPropertyInfo = Reflect<T>.GetProperty(propertyLambdaExpression);
            CheckStringMaxLength(obj, reflectedPropertyInfo.Name, maxLength, "X", new String('X', maxLength), new String('X', maxLength + 1));
        }

        public static void CheckStringMaxLength(Csla.Core.BusinessBase obj, string propertyName, int maxLength)
        {
            CheckStringMaxLength(obj, propertyName, maxLength, "X", new String('X', maxLength), new String('X', maxLength + 1));
        }

        internal static void CheckStringMaxLength(Csla.Core.BusinessBase obj, string propertyName, int maxLength, string shortValue, string maxLengthValue, string longValue)
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

        public static void CheckEmailMaxLength<T>(Csla.Core.BusinessBase obj, Expression<Func<T, object>> propertyLambdaExpression, int maxLength)
        {
            var reflectedPropertyInfo = Reflect<T>.GetProperty(propertyLambdaExpression);
            CheckStringMaxLength(obj, 
                reflectedPropertyInfo.Name, 
                maxLength, 
                "x@x.com",
                String.Format("{0}@x.com", new string('x', maxLength - 6)),
                String.Format("{0}@x.com", new string('x', maxLength - 5)));
        }

        public static void CheckEmailMaxLength(Csla.Core.BusinessBase obj, string propertyName, int maxLength)
        {
            CheckStringMaxLength(obj,
                propertyName,
                maxLength,
                "x@x.com",
                String.Format("{0}@x.x", new string('x', maxLength - 6)),
                String.Format("{0}@x.x", new string('x', maxLength - 5)));
        }

        public static void CheckEnumRules<T>(Csla.Core.BusinessBase obj, Expression<Func<T, object>> propertyLambdaExpression, T validValue, T emptyValue, bool required)
        {
            var reflectedPropertyInfo = Reflect<T>.GetProperty(propertyLambdaExpression);
            CheckEnumRules(obj, reflectedPropertyInfo.Name, validValue, emptyValue, required);
        }

        public static void CheckEnumRules<T>(Csla.Core.BusinessBase obj, string propertyName, T validValue, T emptyValue, bool required)
        {
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

        public static void CheckGuidRules<T>(Csla.Core.BusinessBase obj, Expression<Func<T, object>> propertyLambdaExpression, bool required)
        {
            var reflectedPropertyInfo = Reflect<T>.GetProperty(propertyLambdaExpression);
            CheckGuidRules(obj, reflectedPropertyInfo.Name, required);
        }

        public static void CheckGuidRules(Csla.Core.BusinessBase obj, string propertyName, bool required)
        {
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

        public static void CheckObjectRequiredRules<T>(Csla.Core.BusinessBase obj, Csla.Core.IPropertyInfo propertyInfo, T nonEmptyValue, bool required)
        {
            CheckObjectRequiredRules(obj, propertyInfo.Name, nonEmptyValue, required);
        }

        public static void CheckObjectRequiredRules<T>(Csla.Core.BusinessBase obj, string propertyName, T nonEmptyValue, bool required)
        {
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

        public static void CheckIntMinMaxRules<T>(Csla.Core.BusinessBase obj, Expression<Func<T, object>> propertyLambdaExpression, int min, int max)
        {
            var reflectedPropertyInfo = Reflect<T>.GetProperty(propertyLambdaExpression);
            CheckIntMinMaxRules(obj, reflectedPropertyInfo.Name, min, max);
        }

        public static void CheckIntMinMaxRules(Csla.Core.BusinessBase obj, string propertyName, int min, int max)
        {
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

        public static void CheckByteMinMaxRules(Csla.Core.BusinessBase obj, string propertyName, byte min, byte max)
        {
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

        //public static void CheckEmailAddress(Csla.Core.BusinessBase obj, string propertyName)
        //{
        //    // First, make sure the object is valid.
        //    Assert.IsTrue(obj.IsValid, "The object must be valid before the rule can be checked.");

        //    //Create a random email address
        //    string maxEmail, overMaxEmail;

        //    maxEmail = RandomString(64);
        //    maxEmail += "@";
        //    maxEmail += RandomString(251);
        //    maxEmail += ".com";

        //    overMaxEmail = RandomString(65);
        //    overMaxEmail += "@";
        //    overMaxEmail += RandomString(251);
        //    overMaxEmail += ".com";

        //    // Check on the Email Address Lenght 320 should be good 321 should fail
        //    CheckStringMaxLength(obj, propertyName, 320, "joe@yahoo.com", maxEmail, overMaxEmail);

        //    Validation.EmailAddressRequiredRuleArgs args = new Validation.EmailAddressRequiredRuleArgs(propertyName, true);

        //    string emailAddress = null;
        //    Utilities.CallByName(obj, propertyName, CallType.Set, emailAddress);
        //    // An email address can be null so this shouldn't be false...
        //    bool isRuleBroken = Validation.CommonBusinessRules.EmailRequired(obj, args);
        //    Assert.IsFalse(isRuleBroken, String.Format("Object should be valid. You DO NOT have a ValidationRule to require a value for property [{0}]. Case 1", propertyName));

        //    // Check Null String 
        //    emailAddress = "";
        //    Utilities.CallByName(obj, propertyName, CallType.Set, emailAddress);
        //    // An email address can be null so this shouldn't be false...
        //    isRuleBroken = Validation.CommonBusinessRules.EmailRequired(obj, args);
        //    Assert.IsFalse(isRuleBroken, String.Format("Object should be valid. You DO NOT have a ValidationRule to require a value for property [{0}]. Case 2", propertyName));

        //    args = new Validation.EmailAddressRequiredRuleArgs(propertyName, false);

        //    emailAddress = null;
        //    Utilities.CallByName(obj, propertyName, CallType.Set, emailAddress);
        //    // An email address can be null so this shouldn't be false...
        //    isRuleBroken = Validation.CommonBusinessRules.EmailRequired(obj, args);
        //    Assert.IsTrue(isRuleBroken, String.Format("Object should be valid. You DO NOT have a ValidationRule to require a value for property [{0}]. Case 3", propertyName));

        //    emailAddress = "";
        //    Utilities.CallByName(obj, propertyName, CallType.Set, emailAddress);
        //    // An email address can be null so this shouldn't be false...
        //    isRuleBroken = Validation.CommonBusinessRules.EmailRequired(obj, args);
        //    Assert.IsTrue(isRuleBroken, String.Format("Object should be valid. You DO NOT have a ValidationRule to require a value for property [{0}]. Case 4", propertyName));

        //    emailAddress = "brokenemail@someplace.com";
        //    Utilities.CallByName(obj, propertyName, CallType.Set, emailAddress);
        //    // An email address so should be true...
        //    isRuleBroken = Validation.CommonBusinessRules.EmailRequired(obj, args);
        //    Assert.IsTrue(isRuleBroken, String.Format("Object should be valid. You DO NOT have a ValidationRule to require a value for property [{0}]. Case 5", propertyName));

        //    emailAddress = "NotAnEmailAddress";
        //    Utilities.CallByName(obj, propertyName, CallType.Set, emailAddress);
        //    // Just a string shouldn't work
        //    isRuleBroken = Validation.CommonBusinessRules.EmailRequired(obj, args);
        //    Assert.IsFalse(isRuleBroken, String.Format("Object should be valid. You DO NOT have a ValidationRule to require a value for property [{0}]. Case 6", propertyName));


        //}

        // Created to generate a random email address
        public static string RandomString(int size)
        {
            var builder = new StringBuilder();
            var random = new Random();
            for (int i = 0; i < size; i++)
            {
                char ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            return builder.ToString();
        }

    }
}
