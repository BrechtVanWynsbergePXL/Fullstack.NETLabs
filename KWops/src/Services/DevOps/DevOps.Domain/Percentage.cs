using Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevOps.Domain
{
    public class Percentage : ValueObject<Percentage>
    {
        public readonly double _value;

        public Percentage()
        {

        }

        public Percentage(double value)
        {
            if(value < 0.0 || value > 1.0)
            {
                throw new ContractException();
            }
            _value = value;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return _value;
        }

        public override string ToString()
        {
            return Math.Round(_value * 100, 2).ToString().Replace('.', ',') + "%";
        }

        public static implicit operator string(Percentage value) => value.ToString();
        public static implicit operator double(Percentage value) => value._value;
        public static implicit operator Percentage(double value) => new Percentage(value);
    }
}
