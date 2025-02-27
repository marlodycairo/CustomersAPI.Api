using Microsoft.EntityFrameworkCore.Query;
using System.Collections;
using System.Linq.Expressions;


namespace CustomersAPI.Tests.Extensions
{
    public static class AsyncEnumerableExtensions
    {
        public static IAsyncEnumerable<T> ToAsyncEnumerable<T>(this IEnumerable<T> source)
        {
            return new TestAsyncEnumerable<T>(source);
        }
    }

    public class TestAsyncEnumerable<T> : IAsyncEnumerable<T>, IQueryable<T>
    {
        private readonly IEnumerable<T> _enumerable;

        public TestAsyncEnumerable(IEnumerable<T> enumerable)
        {
            _enumerable = enumerable;
        }

        public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        {
            return new TestAsyncEnumerator<T>(_enumerable.GetEnumerator());
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _enumerable.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public Type ElementType => typeof(T);
        public Expression Expression => _enumerable.AsQueryable().Expression;
        public IQueryProvider Provider => new TestAsyncQueryProvider<T>(_enumerable.AsQueryable().Provider);
    }

    public class TestAsyncEnumerator<T> : IAsyncEnumerator<T>
    {
        private readonly IEnumerator<T> _enumerator;

        public TestAsyncEnumerator(IEnumerator<T> enumerator)
        {
            _enumerator = enumerator;
        }

        public ValueTask DisposeAsync()
        {
            _enumerator.Dispose();
            return ValueTask.CompletedTask;
        }

        public ValueTask<bool> MoveNextAsync()
        {
            return new ValueTask<bool>(_enumerator.MoveNext());
        }

        public T Current => _enumerator.Current;
    }

    public class TestAsyncQueryProvider<T> : IAsyncQueryProvider
    {
        private readonly IQueryProvider _inner;

        public TestAsyncQueryProvider(IQueryProvider inner)
        {
            _inner = inner;
        }

        public IQueryable CreateQuery(Expression expression)
        {
            return new TestAsyncEnumerable<T>(_inner.CreateQuery<T>(expression));
        }

        public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
        {
            return new TestAsyncEnumerable<TElement>(_inner.CreateQuery<TElement>(expression));
        }

        public object Execute(Expression expression)
        {
            return _inner.Execute(expression);
        }

        public TResult Execute<TResult>(Expression expression)
        {
            return _inner.Execute<TResult>(expression);
        }

        public IAsyncEnumerable<TResult> ExecuteAsync<TResult>(Expression expression)
        {
            return new TestAsyncEnumerable<TResult>(_inner.CreateQuery<TResult>(expression));
        }

        public Task<TResult> ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken)
        {
            return Task.FromResult(_inner.Execute<TResult>(expression));
        }

        TResult IAsyncQueryProvider.ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken)
        {
            return ExecuteAsync<TResult>(expression, cancellationToken).GetAwaiter().GetResult();
        }

    }
}

