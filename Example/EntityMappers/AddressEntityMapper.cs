namespace Example.EntityMappers
{
    using Dapper.GraphQL;
    using Models;

    public class AddressEntityMapper : DeduplicatingEntityMapper<Address>
    {
        private CityEntityMapper cityEntityMapper;

        public AddressEntityMapper()
        {
            // Deduplicate entities using MergedToPersonId instead of Id.
            PrimaryKey = p => p.Id;
        }

        public override Address Map(EntityMapContext context)
        {
            // Avoid creating the mappers until they're used
            // NOTE: this avoids an infinite loop (had these been created in the ctor)
            if (cityEntityMapper == null)
            {
                cityEntityMapper = new CityEntityMapper();
            }

            // NOTE: Order is very important here.  We must map the objects in
            // the same order they were queried in the QueryBuilder.

            // Start with the person, and deduplicate
            var address = Deduplicate(context.Start<Address>());
            var city = context.Next("city", cityEntityMapper);

            if (address != null)
            {
                address.City = address.City ?? city;
            }

            return address;
        }
    }
}
