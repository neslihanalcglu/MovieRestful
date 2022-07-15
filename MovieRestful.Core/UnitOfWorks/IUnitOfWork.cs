using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieRestful.Core.UnitOfWorks
{
    public interface IUnitOfWork
    {
        // DbContext'in SaveChange ve SaveChangeAsync methodlarını imlemente edecek.
        // Bu sayede veritabanına kaydetme işlemi tek bir yerden kontrollü olarak gerçekleşecek.
        Task CommitAsync();
        void Commit();

    }
}
