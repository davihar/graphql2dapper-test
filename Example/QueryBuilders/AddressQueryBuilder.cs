using Dapper.GraphQL;
using Example.Models;
using GraphQL.Language.AST;

namespace Example.QueryBuilders
{
    public class AddressQueryBuilder : IQueryBuilder<Address>
    {
        public SqlQueryContext Build(SqlQueryContext query, IHaveSelectionSet context, string alias)
        {
            query.Select($"{alias}.Id");
            query.SplitOn<Address>("Id");

            foreach (var field in context.GetSelectedFields())
            {
                switch (field.Key.ToLower())
                {
                    case "street":
                        query.Select($"{alias}.Street");
                        break;
                    case "city":
                        var cityAlias = $"{alias}City";
                        query.InnerJoin($"Cities {cityAlias} on {alias}.CityId = {cityAlias}.Id");
                        query = new CityQueryBuilder().Build(query, field.Value, cityAlias);
                        break;
                }
            }

            return query;
        }
    }
}