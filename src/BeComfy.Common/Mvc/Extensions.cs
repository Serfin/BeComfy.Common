using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace BeComfy.Common.Mvc
{
    public static class Extensions
    {
        public static T Bind<T>(this T model, Expression<Func<T, object>> expression, object value)
            => model.Bind<T, object>(expression, value);

        public static T BindId<T>(this T model, Expression<Func<T, Guid>> expression)
            => model.Bind<T, Guid>(expression, Guid.NewGuid());

        public static T BindUserIdentity<T>(this T model, Expression<Func<T, Guid>> expression, string userIdentity)
            => model.Bind<T, Guid>(expression, GetUserIdentity(userIdentity));


        private static TModel Bind<TModel, TProperty>(this TModel model, Expression<Func<TModel, TProperty>> expression,
            object value)
        {
            var memberExpression = expression.Body as MemberExpression;
            if (memberExpression == null)
            {
                memberExpression = ((UnaryExpression) expression.Body).Operand as MemberExpression;
            }

            var propertyName = memberExpression.Member.Name.ToLowerInvariant();
            var modelType = model.GetType();
            var field = modelType.GetFields(BindingFlags.Instance | BindingFlags.NonPublic)
                .SingleOrDefault(x => x.Name.ToLowerInvariant().StartsWith($"<{propertyName}>"));
            if (field == null)
            {
                return model;
            }

            field.SetValue(model, value);

            return model;
        }

        private static Guid GetUserIdentity(string value)
            => string.IsNullOrEmpty(value) ? Guid.Empty : Guid.Parse(value);
    }
}