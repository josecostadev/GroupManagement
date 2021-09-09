using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace CodingMilitia.PlayBall.GroupManagement.Business.Impl.Mapping
{
    public static class MappingExtensions
    {
        public static IReadOnlyCollection<TOut> Map<TIn, TOut>(this IReadOnlyCollection<TIn> input, Func<TIn, TOut> mapper)
        {
            if (input == null)
                throw new ArgumentException(nameof(input));

            if (mapper == null)
                throw new ArgumentException(nameof(mapper));

            if (input?.Count == 0)
            {
                return Array.Empty<TOut>();
            }

            var result = new TOut[input.Count];
            var i = 0;
            foreach (var inModel in input)
            {
                result[i] = mapper(inModel);
                ++i;
            }

            return new ReadOnlyCollection<TOut>(result);
        }
    }
}
