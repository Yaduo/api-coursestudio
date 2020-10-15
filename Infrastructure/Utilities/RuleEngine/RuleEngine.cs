using System;
using System.Linq.Expressions;
using System.Reflection;

namespace CourseStudio.Lib.Utilities.RuleEngine
{
	public static class RuleEngine
    {
		public static Func<T, bool> CompileRule<T>(IRule r)
        {
            var param = Expression.Parameter(typeof(T));
            Expression expr = BuildExpr<T>(r, param);
            // build a lambda function T->bool and compile it
            return Expression.Lambda<Func<T, bool>>(expr, param).Compile();
        }

		static Expression BuildExpr<T>(IRule r, ParameterExpression param)
        {
            var left = Expression.Property(param, r.MemberName);
            var tProp = typeof(T).GetProperty(r.MemberName).PropertyType;

            // a .NET known operator
            if (ExpressionType.TryParse(r.Operator, out ExpressionType tBinary))
            {
                var right = Expression.Constant(Convert.ChangeType(r.TargetValue, tProp));
                // use a binary operation, e.g. 'Equal' -> 'u.Age == 15'
                return Expression.MakeBinary(tBinary, left, right);
            }

            // operator like "Contains", etc.
            else
            {
                //var method = tProp.GetMethod(r.Operator);
                MethodInfo method = typeof(string).GetMethod(r.Operator, new[] { typeof(string) });
                var tParam = method.GetParameters()[0].ParameterType;
                var right = Expression.Constant(Convert.ChangeType(r.TargetValue, tParam));
                // use a method call, e.g. 'Contains' -> 'u.Tags.Contains(some_tag)'
                return Expression.Call(left, method, right);
            }
        }

    }
}
