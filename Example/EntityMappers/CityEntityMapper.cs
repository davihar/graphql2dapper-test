namespace Example.EntityMappers
{
    using Dapper.GraphQL;
    using Models;

    public class CityEntityMapper : DeduplicatingEntityMapper<City>
    {
        private CountryEntityMapper countryEntityMapper;

        public CityEntityMapper()
        {
            // Deduplicate entities using MergedToPersonId instead of Id.
            PrimaryKey = p => p.Id;
        }

        public override City Map(EntityMapContext context)
        {
            // Avoid creating the mappers until they're used
            // NOTE: this avoids an infinite loop (had these been created in the ctor)
            if (countryEntityMapper == null)
            {
                countryEntityMapper = new CountryEntityMapper();
            }

            // NOTE: Order is very important here.  We must map the objects in
            // the same order they were queried in the QueryBuilder.

            // Start with the person, and deduplicate
            var city = Deduplicate(context.Start<City>());
            var country = context.Next("country", countryEntityMapper);

            if (city != null)
            {
                city.Country = city.Country ?? country;
            }

            return city;
        }
    }
}
