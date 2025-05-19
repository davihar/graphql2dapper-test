namespace Example.SqlQuery
{
    using System;

    using Dapper.GraphQL;

    using Model;

    public class PersonMapper : IEntityMapper<Person>
    {
        public Func<Person, Person> ResolveEntity { get; set; }

        public Person Map(EntityMapContext context)
        {
            Person person = null;
            Address address = null;
            City city = null;
            Country country = null;

            foreach (var obj in context.Items)
            {
                switch (obj)
                {
                    case Person p:
                        person = p;
                        break;
                    case Address a:
                        address = a;
                        break;
                    case City c:
                        city = c;
                        break;
                    case Country ctr:
                        country = ctr;
                        break;
                }
            }

            if (person != null)
            {
                if (address != null)
                {
                    person.Address = address;
                    if (city != null)
                    {
                        person.Address.City = city;
                        if (country != null)
                        {
                            person.Address.City.Country = country;
                        }
                    }
                }
            }

            return person;
        }
    }
}
