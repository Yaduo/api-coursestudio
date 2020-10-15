using System;
using System.Collections.Generic;
using System.Linq;
using JWT;
using CourseStudio.Doamin.Models.Trades;
using CourseStudio.Lib.Utilities.RuleEngine;
using CourseStudio.Lib.Utilities.Security;

namespace CourseStudio.Presentation.RuleEngineTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

			// Test JWT toke
			var privateKey = "liuyaduo";
			var payloads = new List<object>() { "123", "345", "789" };
			var payloads2 = new List<object>() { "123", "345" };
			var exp = DateTimeOffset.UtcNow.AddHours(-1);
			var token = JwtTokenHelper.GenerateToken(payloads, exp, privateKey);

            try
            {
				var good = JwtTokenHelper.IsValid(token, payloads, privateKey);
            }
            catch (TokenExpiredException)
            {
				Console.WriteLine("TokenExpiredException");
            }
            catch (SignatureVerificationException)
            {
				Console.WriteLine("SignatureVerificationException");
            }


            /////////////// check promotion rule
			// SFU Computer Science Bundle 50% off
			List<CouponRule> rule1 = new List<CouponRule>
			{
				new CouponRule()
				{
					Id = 1,
					CouponId = 1,
					MemberName = "CourseIds",
					Operator="Contains", 
					TargetValue="125"
				},
				new CouponRule()
                {
                    Id = 2,
                    CouponId = 2,
                    MemberName = "CourseIds",
                    Operator="Contains",
                    TargetValue="128"
                },
				new CouponRule()
                {
                    Id = 3,
                    CouponId = 3,
                    MemberName = "CourseIds",
                    Operator="Contains",
                    TargetValue="225"
                }
            };

			// buy 2 get 1
			List<CouponRule> rule2 = new List<CouponRule>
            {
                new CouponRule()
                {
                    Id = 1,
                    CouponId = 4,
					MemberName = "TotalQuantity",
					Operator="GreaterThan",
                    TargetValue="2"
                }
            };

			// Buy CMPT 125 get CMPT 225
            List<CouponRule> rule3 = new List<CouponRule>
            {
                new CouponRule()
                {
                    Id = 1,
                    CouponId = 5,
					MemberName = "CourseIds",
					Operator="Contains",
                    TargetValue="125"
                }
            };
            
			// 满300减50
            List<CouponRule> rule4 = new List<CouponRule>
            {
                new CouponRule()
                {
                    Id = 1,
                    CouponId = 6,
					MemberName = "TotalAmount",
					Operator="GreaterThan",
                    TargetValue="300"
                }
            };

			var lineItems = new List<LineItem>
            {
				new LineItem() 
				{
					CourseId = 125,
					Quantity = 1,
					UnitPrice = 30
				},
				new LineItem()
                {
                    CourseId = 128,
                    Quantity = 1,
                    UnitPrice = 60
                },
				new LineItem()
                {
                    CourseId = 225,
                    Quantity = 1,
                    UnitPrice = 35
                }
            };

			var preparedLineItems = new PreparedLineItemsForConpon
            (
                lineItems.Select(i => i.CourseId).ToList(),
                lineItems.Sum(i => i.Quantity),
                lineItems.Sum(i => i.Price)
            );

			var compiledRule1 = rule1.Select(RuleEngine.CompileRule<PreparedLineItemsForConpon>).ToList();
			var result1 = compiledRule1.All(r => r(preparedLineItems));
			Console.WriteLine("result1: {0}", result1);
            
			var compiledRule2 = rule2.Select(RuleEngine.CompileRule<PreparedLineItemsForConpon>).ToList();
            var result2 = compiledRule2.All(r => r(preparedLineItems));
            Console.WriteLine("result2: {0}", result2);

			var compiledRule3 = rule3.Select(RuleEngine.CompileRule<PreparedLineItemsForConpon>).ToList();
            var result3 = compiledRule3.All(r => r(preparedLineItems));
            Console.WriteLine("result3: {0}", result3);

			var compiledRule4 = rule4.Select(RuleEngine.CompileRule<PreparedLineItemsForConpon>).ToList();
            var result4 = compiledRule4.All(r => r(preparedLineItems));
            Console.WriteLine("result4: {0}", result4);

         
			Console.WriteLine("Hello World!");
        }
    }
}
