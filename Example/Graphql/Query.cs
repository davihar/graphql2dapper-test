using System.Linq;
using Dapper.GraphQL;
using Example.EntityMappers;
using Example.QueryBuilders;
using GraphQL.Types;
using Microsoft.Data.Sqlite;

namespace Example.Graphql
{
    public class Query : ObjectGraphType
    {
        // TODO have constructor accept a connection string rather than hard coding it below
        public Query()
        {
            Field<ListGraphType<PersonType>>(
                "persons", 
                resolve: context => {
                    const string alias = "person";
                    var sqlQuery = SqlBuilder.From($"Persons {alias}");
                    
                    sqlQuery = new PersonQueryBuilder().Build(sqlQuery, context.FieldAst, alias);
                    using (var db = new SqliteConnection("Data Source=example.db")) {
                        return sqlQuery.Execute(db, context.FieldAst, new PersonEntityMapper());
                    }
                });

            Field<PersonType>(
                "person", 
                arguments: new QueryArguments(new QueryArgument<IntGraphType> { Name = "id" }),
                resolve: context => {
                    var id = context.GetArgument<long>("id");

                    // TODO convert this to an example using the database
                    return ExampleSeed.Persons.SingleOrDefault(p => p.Id == id);
                 });
        }
    }
}
