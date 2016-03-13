# CSLA Business Rule Validator for Unit Testing #

Provides utility classes for unit testing CSLA business rules as well as base classes for integration testing and additional business rules not found in the common rules provided by CSLA.

## Base Classes ##

**TestBase** - a template class for all test classes. Sets up virtual OnInitialize and OnCleaup methods.

**TransationalTest** - wraps a test in a transaction that will automatically rollback any database changes. Use this for integration tests that interact with the database.

## RuleValidator ##
The **RuleValidator** class is the main class used to validate business rules. 
### Basic usage ###

Given an object that inherits from CSLA.Core.BusinessBase<T>:

    using Whc.UnitTesting;
    
    [TestClass]
    public class OrderTests : TestBase
    {
        internal static Order NewObject()
        {
            var obj = Order.NewOrder();
			// Set up the properties of the object so that it is valid.
            obj.SiteNumber = 23;
            obj.DeliveryExceptionId = 1;
            obj.DeliveryRouteNumber = 1;
            obj.OrderDate = DateTime.Now;
            obj.ShipDate = DateTime.Now;
            obj.DeliveryDateTime = DateTime.Now;
            obj.OriginalSiteNumber = 2;
            return obj;
        }

        [TestMethod]
        public void SiteNumber_MinMaxValue()
        {
            var obj = NewObject();
            RuleValidator<Order>.CheckIntMinMaxRules(obj, x => x.SiteNumber, 1, int.MaxValue);
        }
	}

The method CheckIntMinMaxRules will check the bounds of the rules for the given property (in this case SiteNumber) and throw Asserts if the rule fails.

###Additional Rule Validation Methods###


- CheckDateRequired
- CheckBitmapRequired
- CheckByteMinMaxRules
- CheckByteRequired
- CheckDecimalMinMaxRules
- CheckDecimalRequired
- CheckDoubleMinMaxRules
- CheckDoubleRequired
- CheckDecimalMinMaxRules
- CheckDecimalMaxExclusive - :new:
- CheckDecimalMinExclusive - :new:
- CheckDoubleMaxExclusive - :new:
- CheckDoubleMinExclusive - :new:
- CheckEmailMaxLength
- CheckEmailRequired
- CheckEnumRules\<TEnum>
- CheckGuidRules
- CheckIntMaxExclusive
- CheckIntMinExclusive
- CheckIntMinMaxRules
- CheckIntRequired
- CheckShortMaxExclusive
- CheckShortMinExclusive
- CheckShortMinMaxRules
- CheckLongMaxExclusive
- CheckLongMinExclusive
- CheckLongMinMaxRules
- CheckNumberRequired
- CheckObjectRequired
- CheckSmartDateRequired
- CheckStringMaxLength
- CheckStringRequired

#Integration Tests#
The way I like to do integration testing against a database is to start with an empty database (with the exception of a User record for authentication and authorization). Each integration test is wrapped in a transaction that is rolled back at the end of the test, regardless of whether the test passes or fails. 

    using System.Transactions;
    
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
   

The transaction ensures that I leave the database unchanged after each test but requires that I set up the data I need for each test.

To support integration tests, simply inherit your test class from the base class TransactionalTest:
        
    using Whc.Csla;
    
    [TestClass]
    public class OrderTests : TransactionalTest
    {
	}
    