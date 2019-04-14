using Example.Models;
using GraphQL.Types;

namespace Example.Graphql
{
    public class PersonType : ObjectGraphType<Person>
    {
        public PersonType()
        {
            Field(x => x.Id);
            Field(x => x.FirstName);
            Field(x => x.LastName);
            Field(x => x.Address, type: typeof(AddressType));
        }
    }
}
