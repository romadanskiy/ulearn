using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Reflection.Randomness
{
    public class Generator<T>
    {
        private static readonly PropertyInfo[] properties;
        private static readonly FromDistribution[] distributions;

        static Generator()
        {
            properties = typeof(T)
                .GetProperties()
                .Where(p => p.GetCustomAttributes(typeof(FromDistribution), false).Length > 0)
                .ToArray();

            distributions = properties
                .Select(p => (FromDistribution) Attribute.GetCustomAttribute(p, typeof(FromDistribution)))
                .ToArray();
        }

        public T Generate(Random rnd)
        {
            var t = Activator.CreateInstance(typeof(T));

            for (var i = 0; i < properties.Length; i++)
            {
                var d = distributions[i];
                CheckDistributionType(d);
                var continuousDistribution = GetContinuousDistribution(d);
                properties[i].SetValue(t, continuousDistribution.Generate(rnd));
            }

            return (T)t;
        }

        private static void CheckDistributionType(FromDistribution d)
        {
            if (!d.DistributionType.GetInterfaces().Contains(typeof(IContinuousDistribution)))
                throw new ArgumentException($"{d.DistributionType.Name} is not a DistributionType");
        }

        private static IContinuousDistribution GetContinuousDistribution(FromDistribution d)
        {
            try
            {
               return (IContinuousDistribution) Activator.CreateInstance(d.DistributionType, d.Parameters);
            }
            catch
            {
                var type = d.DistributionType.Name;
                var n = d.Parameters.Length;
                throw new ArgumentException($"{type} doesn't have a constructor with {n} number of arguments");
            }
        }
    }
    
    [AttributeUsage(AttributeTargets.Property)]
    public class FromDistribution : Attribute
    {
        public readonly Type DistributionType;
        public readonly object[] Parameters;

        public FromDistribution(Type distributionType, params object[] parameters)
        {
            DistributionType = distributionType;
            Parameters = parameters;
        }
    }
}