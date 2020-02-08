using System;
using System.Collections.Generic;
using System.Reflection;
using LinqKit;

namespace Wingman.Linq
{
  public class WhereClause : List<SearchField>
	{
		public WhereClause() : base() { }
		public WhereClause(IEnumerable<SearchField> searchFields, WhereJoinOperator? joinOperator = null)
			: base(searchFields)
		{
			JoinOperator = joinOperator ?? JoinOperator;
		}

		/// <summary>
		/// Calculated property to identify if any where clauses are present.
		/// </summary>
		public bool HasClauses => Count > 0 || Subclauses?.Count > 0;

		/// <summary>
		/// The operator to join the where clauses and subclauses together with.
		/// </summary>
		public WhereJoinOperator JoinOperator { get; set; } = WhereJoinOperator.And;

		/// <summary>
		/// Operator to use between the clause and subclauses.
		/// </summary>
		public WhereJoinOperator JoinToSubclauseOperator { get; set; } = WhereJoinOperator.And;

		/// <summary>
		/// Subclauses to be joined with the main clause.
		/// </summary>
		public List<WhereClause> Subclauses { get; set; }

		/// <summary>
		/// The operator to join the subclauses together with.
		/// </summary>
		public WhereJoinOperator? SubclauseJoinOperator { get; set; } = WhereJoinOperator.And;

		/// <summary>
		/// Converts an object into a list of <c>SearchField</c> by scanning each public property of the object.
		/// </summary>
		/// <param name="model">The object to analyze.</param>
		/// <param name="ignoreNulls">Ignore properties with a value of <c>null</c>.</param>
		/// <param name="joinOperator">The where clause operator.</param>
		/// <returns>A <c>SearchFieldList</c>.</returns>
		public static WhereClause FromObject(object model, WhereJoinOperator joinOperator =  WhereJoinOperator.And, bool ignoreNulls = false)
		{
			if (model == null)
				throw new ArgumentNullException("Model is required");

			var properties = model.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
			var searchFields = new List<SearchField>();

			foreach (var property in properties)
			{
				var value = property.GetValue(model);
				if (ignoreNulls && value == null)
					continue;

				var attribute = (SearchFieldAttribute)property.GetCustomAttribute(typeof(SearchFieldAttribute));
				var valueType = attribute?.ValueType ?? property.PropertyType;
				var searchField = new SearchField
        {
					Name = attribute?.Name ?? property.Name,
					WhereOperator = attribute?.WhereClauseType ?? Linq.WhereOperator.Equal,
					Value = value,
					ValueType = valueType,
				};

				searchFields.Add(searchField);
			}

			var result = new WhereClause(searchFields, joinOperator);
			return result;
		}

		public ExpressionStarter<T> ToWhereExpression<T>()
		{
			var predicate = PredicateBuilder.New<T>(true);

			foreach (var searchField in this)
			{
				var whereExpression = ExpressionMethods.ToWhereExpression<T>(
					searchField.Name,
					searchField.WhereOperator,
					searchField.ValueType,
					searchField.Value,
					searchField.MaxValue
				);

				if (JoinOperator == WhereJoinOperator.And)
					predicate = predicate.And(whereExpression);
				else
					predicate = predicate.Or(whereExpression);
			}

			if (Subclauses != null && Subclauses.Count > 0)
			{
				var subclausePredicate = PredicateBuilder.New<T>(true);
				foreach (var subclause in Subclauses)
				{
					var subWhereExpression = subclause.ToWhereExpression<T>();
					if (SubclauseJoinOperator == WhereJoinOperator.And)
						subclausePredicate = subclausePredicate.And(subWhereExpression);
					else
						subclausePredicate = subclausePredicate.Or(subWhereExpression);
				}

				if (JoinToSubclauseOperator == WhereJoinOperator.And)
					predicate = predicate.And(subclausePredicate);
				else
					predicate = predicate.Or(subclausePredicate);
			}

			return predicate;
		}
	}
}
