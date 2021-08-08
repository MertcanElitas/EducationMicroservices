using Services.Order.Domain.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Order.Domain.OrderAggregate
{
    public class Adress : ValueObject
    {
        public string Province { get; private set; }
        public string ZipCode { get; private set; }
        public string District { get; private set; }
        public string Line { get; private set; }
        public string Street { get; private set; }

        public Adress(string province, string zipCode, string district, string line, string street)
        {
            Province = province;
            ZipCode = zipCode;
            District = district;
            Line = line;
            Street = street;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Province;
            yield return ZipCode;
            yield return District;
            yield return Line;
            yield return Street;
        }
    }
}
