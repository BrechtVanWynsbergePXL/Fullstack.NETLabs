using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Test
{
    public abstract class BuilderBase<T> where T : class
    {
        protected static Random Random = new Random();
        private Type _itemType;
        protected T Item;

        protected void ConstructItem(params object[] constructorParameters)
        {
            _itemType = typeof(T);
            Item = Activator.CreateInstance(_itemType, BindingFlags.Instance | 
                BindingFlags.NonPublic | BindingFlags.Public, null, constructorParameters, null) as T;
        }

        protected void SetProperty<TProperty>(Expression<Func<T, TProperty>> propertyFunc, TProperty value)
        {
            var member = propertyFunc.Body as MemberExpression;
            var propertyInfo = member?.Member as PropertyInfo;
            if (propertyInfo == null)
            {
                throw new InvalidOperationException($"Expression does not match a property: { propertyFunc }");
            }
            object target = Item;
            if (!(member.Expression is ParameterExpression))
            {
                var parentFunc = Expression.Lambda(member.Expression, propertyFunc.Parameters);
                target = parentFunc.Compile().DynamicInvoke(target);
            }
            propertyInfo.SetValue(target, value);
        }
        public virtual T Build()
        {
            return Item;
        }
    }
}
