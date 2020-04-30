using BookStore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Contracts
{
    public interface IBookRepository : IRepositoryBase<Book>
    {
    }
}
