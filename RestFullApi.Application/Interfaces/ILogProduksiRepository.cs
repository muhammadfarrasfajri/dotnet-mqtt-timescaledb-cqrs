using RestFullApi.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestFullApi.Application.Interfaces
{
    public interface ILogProduksiRepository
    {
        // Kontrak untuk mencatat barang (Command)
        Task<int> CatatProduksiAsync(LogProduksi log);

        // Kontrak untuk membaca rekap realtime (Query)
        Task<IEnumerable<LogProduksi>> AmbilSemuaLogAsync();

        Task<LogProduksi> AmbilByIdLogAsync(Guid id);
    }
}
