using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Http.Consumer
{
    public class HttpAggregateResult
    {
        private readonly List<object> _response;

        public HttpAggregateResult() : this(new List<object>()) { }

        public HttpAggregateResult(List<object> response)
        {
            _response = response;
        }

        internal void AddResponse(object @obj)
        {
            _response.Add(obj);
        }

        public HttpAggregateResult<T, R> Get<T, R>(int index, R @obj)
        {
            return new HttpAggregateResult<T, R>(_response, (T)_response[index], obj);
        }
    }

    public class HttpAggregateResult<T, R> : HttpAggregateResult
    {
        private readonly T _valueT;
        private readonly R _valueR;

        public HttpAggregateResult(List<object> response, T valueT, R valueR) : base(response)
        {
            _valueT = valueT;
            _valueR = valueR;
        }

        public HttpAggregateResult<T, R> Bind<TProperty>(Expression<Func<T, TProperty>> expressionT,
                                                    Expression<Func<R, TProperty>> expressionR)
        {
            if (_valueT == null || _valueR == null)
                return this;

            if (expressionR.Body is MemberExpression memberExpression)
            {
                var property = memberExpression.Member as PropertyInfo;
                if (property != null)
                {
                    TProperty valueT = expressionT.Compile()(_valueT);
                    property.SetValue(_valueR, valueT, null);
                }
            }

            return this;
        }
    }
}
