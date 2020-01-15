using System;
using System.Linq;
using System.Linq.Expressions;

namespace NETStandardLibrary.Linq
{
	public sealed class ExpressionTrees
	{
		public ExpressionTrees()
		{
			// TODO: This whole class is just for testing, kill it with FIRE!
			// Replace the following using expression trees:
			// companies.Where(company => (company.ToLower() == "coho winery" || company.Length > 16)).OrderBy(company => company)

			string[] companies = { "Consolidated Messenger", "Alpine Ski House", "Southridge Video", "City Power & Light",
												"Coho Winery", "Wide World Importers", "Graphic Design Institute", "Adventure Works",
												"Humongous Insurance", "Woodgrove Bank", "Margie's Travel", "Northwind Traders",
												"Blue Yonder Airlines", "Trey Research", "The Phone Company",
												"Wingtip Toys", "Lucerne Publishing", "Fourth Coffee" };

			// The IQueryable data to query.
			IQueryable<string> queryableData = companies.AsQueryable<string>();

			// Compose the expression tree that represents the parameter to the predicate.
			// NOTE: The "company" here is meaningless since our type is a string.
			ParameterExpression pe = Expression.Parameter(typeof(string), "company");



			// ***** company => company.ToLower() == "coho winery" *****
			Expression left = Expression.Call(pe, typeof(string).GetMethod("ToLower", System.Type.EmptyTypes));
			Expression right = Expression.Constant("coho winery");

			// Create an expression tree that represents the expression 'company.ToLower() == "coho winery"'.
			Expression e1 = Expression.Equal(left, right);


			// ***** company => company.Length > 16 *****
			left = Expression.Property(pe, typeof(string).GetProperty("Length"));
			right = Expression.Constant(16, typeof(int));

			// Create an expression tree that represents the expression 'company.Length > 16'.
			Expression e2 = Expression.GreaterThan(left, right);



			// Combine the expression trees to create an expression tree that represents the
			// expression '(company.ToLower() == "coho winery" || company.Length > 16)'.
			Expression predicateBody = Expression.OrElse(e1, e2);

			// Create an expression tree that represents the expression
			// 'queryableData.Where(company => (company.ToLower() == "coho winery" || company.Length > 16))'
			MethodCallExpression whereCallExpression = Expression.Call(
					typeof(Queryable),
					"Where",
					new Type[] { queryableData.ElementType },
					queryableData.Expression,
					Expression.Lambda<Func<string, bool>>(predicateBody, new ParameterExpression[] { pe }));
			// ***** End Where *****



			// ***** OrderBy(company => company) *****
			// Create an expression tree that represents the expression
			// 'whereCallExpression.OrderBy(company => company)'
			MethodCallExpression orderByCallExpression = Expression.Call(
					typeof(Queryable),
					"OrderBy",
					new Type[] { queryableData.ElementType, queryableData.ElementType },
					whereCallExpression,
					Expression.Lambda<Func<string, string>>(pe, new ParameterExpression[] { pe }));
			// ***** End OrderBy *****



			// Create an executable query from the expression tree.
			IQueryable<string> results = queryableData.Provider.CreateQuery<string>(orderByCallExpression);

			// Enumerate the results.
			foreach (string company in results)
					Console.WriteLine(company);

			/*  This code produces the following output:

					Blue Yonder Airlines
					City Power & Light
					Coho Winery
					Consolidated Messenger
					Graphic Design Institute
					Humongous Insurance
					Lucerne Publishing
					Northwind Traders
					The Phone Company
					Wide World Importers
			*/
		}
	}
}