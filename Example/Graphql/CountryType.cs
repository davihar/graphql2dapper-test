using Example.Models;
using GraphQL.Types;

namespace Example.Graphql
{
    public class CountryType : ObjectGraphType<Country>
    {
        public CountryType()
        {
            Field(x => x.Id);
            Field(x => x.Name);
            Field(x => x.IsoCode);
        }
    }
}
