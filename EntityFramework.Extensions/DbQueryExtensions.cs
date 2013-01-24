using System;
using System.Linq.Expressions;

namespace EntityFramework.Extensions
{
    public static class DbQueryExtensions
    {
        private static string GetPropertyName<T>(Expression<Func<T, object>> expression)
        {
            var memberExpr = expression.Body as MemberExpression;
            if (memberExpr == null)
                throw new ArgumentException("Expression body must be a member expression");
            return memberExpr.Member.Name;
        }
    }
}