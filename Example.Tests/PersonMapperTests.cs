using System.Collections.Generic;
using Example.Model;
using Example.SqlQuery;
using Dapper.GraphQL;
using Xunit;
using System.Linq;

namespace Example.Tests
{
    public class PersonMapperTests
    {
        [Fact]
        public void Map_Returns_FullyPopulated_Person_When_Item_Order_Varies()
        {
            var person = new Person { Id = 1, FirstName = "Foo", LastName = "Bar" };
            var address = new Address { Street = "123 Street" };
            var city = new City { Name = "Metropolis" };
            var country = new Country { Name = "Country", IsoCode = "CT" };

            var objects = new object[] { person, address, city, country };
            var mapper = new PersonMapper();

            foreach (var perm in GetPermutations(objects))
            {
                var context = new EntityMapContext(perm);
                var result = mapper.Map(context);

                Assert.Same(person, result);
                Assert.Same(address, result.Address);
                Assert.Same(city, result.Address.City);
                Assert.Same(country, result.Address.City.Country);
            }
        }

        private static IEnumerable<object[]> GetPermutations(object[] items)
        {
            return Permute(items, 0);
        }

        private static IEnumerable<object[]> Permute(object[] items, int index)
        {
            if (index == items.Length)
            {
                yield return (object[])items.Clone();
            }
            else
            {
                for (int i = index; i < items.Length; i++)
                {
                    Swap(items, index, i);
                    foreach (var result in Permute(items, index + 1))
                        yield return result;
                    Swap(items, index, i);
                }
            }
        }

        private static void Swap(object[] items, int i, int j)
        {
            var temp = items[i];
            items[i] = items[j];
            items[j] = temp;
        }
    }
}
