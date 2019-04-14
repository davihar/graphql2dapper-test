using Example.Models;
using GraphQL.Types;

namespace Example.Graphql
{
    public class AddressType : ObjectGraphType<Address>
    {
        public AddressType()
        {
            Field(x => x.Id);
            Field(x => x.Street);
            Field(x => x.City, type: typeof(CityType));
        }
    }
}
