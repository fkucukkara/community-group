using CG.Core.Domain;
using Xunit;

namespace CG.Test.Units
{
    public class UnitTest1
    {
        [Fact]
        public void Person_Model_Required_Properties()
        {
            var person = new Person()
            {
                Id = 1,
                FirstName = "fatih",
                LastName = "küçükkara",
                Email = "f.kucukkara@otelz.com",
                Occupation = "istanbul"
            };

            Assert.True(person.FirstName != null && person.LastName != null && person.Email != null && person.Occupation != null);

        }
    }
}
