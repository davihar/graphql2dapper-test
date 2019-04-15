namespace Example.EntityMappers
{
    using Dapper.GraphQL;
    using Models;

    public class PersonEntityMapper : DeduplicatingEntityMapper<Person>
    {
        private AddressEntityMapper addressEntityMapper;

        public PersonEntityMapper()
        {
            // Deduplicate entities using MergedToPersonId instead of Id.
            PrimaryKey = p => p.Id;
        }

        public override Person Map(EntityMapContext context)
        {
            // Avoid creating the mappers until they're used
            // NOTE: this avoids an infinite loop (had these been created in the ctor)
            if (addressEntityMapper == null)
            {
                addressEntityMapper = new AddressEntityMapper();
            }

            // NOTE: Order is very important here.  We must map the objects in
            // the same order they were queried in the QueryBuilder.

            // Start with the person, and deduplicate
            var person = Deduplicate(context.Start<Person>());
            var address = context.Next("address", addressEntityMapper);

            if (person != null)
            {
                person.Address = person.Address ?? address;
            }

            return person;
        }
    }
}
