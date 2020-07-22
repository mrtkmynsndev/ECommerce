using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using ECommerce.Core.Entities;

namespace ECommerce.Core.Specifications
{
    public class Specification<T> : ISpecification<T> where T : BaseEntity<int>
    {
        public Specification()
        {

        }

        public Specification(Expression<Func<T, bool>> criteria)
        {
            Criteria = criteria;
        }

        public Expression<Func<T, bool>> Criteria { get; }

        public List<Expression<Func<T, object>>> Includes { get; } = new List<Expression<Func<T, object>>>();

        public Expression<Func<T, object>> OrderBy { get; private set; }

        public Expression<Func<T, object>> OrderByDescending { get; private set; }

        public int Take { get; private set; }

        public int Skip { get; private set; }

        public bool IsPagingEnabled { get; private set; }

        protected void AddInclude(Expression<Func<T, object>> includeExpression)
        {
            Includes.Add(includeExpression);
        }

        protected void AddOrderBy(Expression<Func<T, object>> orderBy)
        {
            OrderBy = orderBy;
        }

        protected void AddOrderByDescending(Expression<Func<T, object>> orderByDescending)
        {
            OrderByDescending = orderByDescending;
        }

        protected void ApplyPaging(int skip, int take){
            Skip = skip;
            Take = take;
            IsPagingEnabled = true;
        }
    }
}