namespace Example
{
    using GraphQL;
    using GraphQL.Http;

    using Graphql;

    public static class Execute
    {
        public static string Query(string query)
        {
            var result = new DocumentExecuter().ExecuteAsync(opts => {
                opts.Query = query;
                opts.Schema = new GraphQL.Types.Schema
                {
                    Query = new Query()
                    //, Mutation = 
                    
                };
            }).Result;

            return new DocumentWriter(true).Write(result);
        }
    }
}
