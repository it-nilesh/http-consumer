using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Http.Consumer
{
    public class HttpAggregateResult : IDisposable
    {
        public List<object> Response { get; }
        public HttpAggregateResult() : this(new List<object>()) { }

        public HttpAggregateResult(List<object> response)
        {
            Response = response;
        }

        internal void AddResponse(object @obj)
        {
            Response.Add(obj);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private bool _disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                Response?.Clear();
            }
            
            _disposed = true;
        }
    }

    public class HttpAggregateResult<TResult> : HttpAggregateResult
    {
        public HttpAggregateResult() : base()
        {
        }

        public HttpAggregateResult(List<object> response) : base(response)
        {
        }

        public HttpAggregateResult<TInput, TResult> Get<TInput>(int index, TResult @obj)
        {
            return new HttpAggregateResult<TInput, TResult>(Response, (TInput)Response[index], obj);
        }
    }

    public class HttpAggregateResult<TInput, TOutput> : HttpAggregateResult<TOutput>
    {
        private readonly TInput  _input;
        private readonly TOutput _output;

        public HttpAggregateResult(List<object> response, TInput input, TOutput output) : base(response)
        {
            _input = input;
            _output = output;
        }

        public HttpAggregateResult<TInput, TOutput> Bind<TProperty>(Expression<Func<TInput, TProperty>> expressionT,
                                                    Expression<Func<TOutput, TProperty>> expressionR)
        {
            if (_input == null || _output == null)
                return this;

            if (expressionR.Body is MemberExpression memberExpression)
            {
                var property = memberExpression.Member as PropertyInfo;
                if (property != null)
                {
                    TProperty inputValue  = expressionT.Compile()(_input);
                    property.SetValue(_output, inputValue, null);
                }
            }

            return this;
        }
    }
}
