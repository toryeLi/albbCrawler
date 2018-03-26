using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Command
{
   public class TInTOutClass
    {
        private static Dictionary<string, object> _Dic = new Dictionary<string, object>();
        private static TOut TransExp<TIn, TOut>(TIn tIn)
        {
            string key = $"funckey_{typeof(TIn).FullName}_{typeof(TOut).FullName}";
            ParameterExpression parameterExpression = Expression.Parameter(typeof(TIn), "p");
            List<MemberBinding> memberBindingList = new List<MemberBinding>();
            foreach (var item in typeof(TOut).GetProperties())
            {
                MemberExpression property = Expression.Property(parameterExpression, typeof(TIn).GetProperty(item.Name));
                memberBindingList.Add(Expression.Bind(item, property));
            }
            foreach (var item in typeof(TOut).GetFields())
            {
                MemberExpression property = Expression.Field(parameterExpression, typeof(TIn).GetField(item.Name));
                memberBindingList.Add(Expression.Bind(item, property));
            }
            Expression<Func<TIn, TOut>> expression = Expression.Lambda<Func<TIn, TOut>>(Expression.MemberInit(Expression.New(typeof(TOut)), memberBindingList), new ParameterExpression[]
            {
parameterExpression
            });
            Func<TIn, TOut> func = expression.Compile();
            _Dic.Add(key, func);
            return func(tIn);
        }
        public static TOut TransExp2<TIn, TOut>(TIn tIn)
        {
            if (_Dic.Keys.Contains($"funckey_{typeof(TIn).FullName}_{typeof(TOut).FullName}"))
            {
                Func<TIn, TOut> func = (Func<TIn, TOut>)_Dic[$"funckey_{typeof(TIn).FullName}_{typeof(TOut).FullName}"];
                return func(tIn);
            }
            else
            {
                return TransExp<TIn, TOut>(tIn);
            }

        }
    }
}
