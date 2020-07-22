using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using ECommerce.Core.Entities;

namespace ECommerce.Core.Specifications
{
    public interface ISpecification<T> where T : BaseEntity<int>
    {
        Expression<Func<T, bool>> Criteria { get; }
        Expression<Func<T, object>> OrderBy { get; }
        Expression<Func<T, object>> OrderByDescending { get; }
        List<Expression<Func<T, object>>> Includes { get; }

        int Take { get; }
        int Skip { get; }
        bool IsPagingEnabled { get; }
    }
}