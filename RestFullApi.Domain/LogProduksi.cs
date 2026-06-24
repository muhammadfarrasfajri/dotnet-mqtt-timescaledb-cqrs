using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestFullApi.Domain
{
    public class LogProduksi
    {
        public Guid Id { get; set; }
        public required string IdMesin { get; set; }
        public int Jumlah { get; set; }
        public DateTime WaktuDeteksi { get; set; }
    }
}
