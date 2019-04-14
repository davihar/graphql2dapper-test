using Dapper.GraphQL;
using Example.Models;
using GraphQL.Language.AST;

namespace Example.QueryBuilders
{
    public class CountryQueryBuilder : IQueryBuilder<Country>
    {
        public SqlQueryContext Build(SqlQueryContext query, IHaveSelectionSet context, string alias)
        {
            query.Select($"{alias}.Id");
            query.SplitOn<Country>("Id");

            foreach (var field in context.GetSelectedFields()) {
                switch (field.Key.ToLower()) {
                    case "name":
                        query.Select($"{alias}.Name"); 
                        break;
                    case "isocode": 
                        query.Select($"{alias}.IsoCode"); 
                        break;
                }
            }

            return query;
        }
    }
}
