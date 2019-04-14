using Dapper.GraphQL;
using Example.Models;
using GraphQL.Language.AST;

namespace Example.QueryBuilders
{
    public class CityQueryBuilder : IQueryBuilder<City>
    {
        public SqlQueryContext Build(SqlQueryContext query, IHaveSelectionSet context, string alias)
        {
            query.Select($"{alias}.Id");
            query.SplitOn<City>("Id");

            foreach (var field in context.GetSelectedFields())
            {
                switch (field.Key.ToLower())
                {
                    case "name": 
                        query.Select($"{alias}.Name"); 
                        break;
                    case "zipcode": 
                        query.Select($"{alias}.ZipCode"); 
                        break;
                    case "country":
                        var countryAlias = $"{alias}Country";
                        query.InnerJoin($"Countries {countryAlias} on {alias}.CountryId = {countryAlias}.Id");
                        query = new CountryQueryBuilder().Build(query, field.Value, countryAlias);
                        break;
                }
            }

            return query;
        }
    }
}