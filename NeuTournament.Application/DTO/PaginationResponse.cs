using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuTournament.Application.DTO
{
    public class PaginationResponse<T>
    {
        private PaginationResponse()
        {
            Items = Enumerable.Empty<T>();
        }

        public PaginationResponse(IEnumerable<T> items, long totalItemCount)
        {
            Items = items;
            TotalItemCount = totalItemCount;
        }

        public IEnumerable<T> Items { get; set; } = Enumerable.Empty<T>();
        public long TotalItemCount { get; set; }
    }
}
