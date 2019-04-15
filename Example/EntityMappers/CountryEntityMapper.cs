namespace Example.EntityMappers
{
    using Dapper.GraphQL;
    using Models;

    public class CountryEntityMapper : DeduplicatingEntityMapper<Country>
    {
        public CountryEntityMapper()
        {
            // Deduplicate entities using MergedToPersonId instead of Id.
            PrimaryKey = p => p.Id;
        }

        public override Country Map(EntityMapContext context)
        {
            // NOTE: Order is very important here.  We must map the objects in
            // the same order they were queried in the QueryBuilder.

            // Start with the person, and deduplicate
            var country = Deduplicate(context.Start<Country>());

            return country;
        }
    }
}
