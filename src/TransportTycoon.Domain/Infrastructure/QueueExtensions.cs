using System.Collections.Generic;

namespace TransportTycoon.Domain.Infrastructure
{
    public static class QueueExtensions
    {
        public static bool TryDequeue<T>(this Queue<T> source, out T item)
        {
            item = default;

            if (source.Count > 0)
            {
                item = source.Dequeue();
                return true;
            }

            return false;
        }
    }
}