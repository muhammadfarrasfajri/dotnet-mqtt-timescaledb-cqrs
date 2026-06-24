using Microsoft.AspNetCore.Mvc;
using RestFullApi.Application.Interfaces;
using RestFullApi.Domain;

namespace RestFullApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // URL otomatis menjadi: http://localhost:port/api/produksi
    public class ProduksiController : ControllerBase
    {
        private readonly ILogProduksiRepository _repository;

        // Dependency Injection: Controller meminta kontrak ILogProduksiRepository
        public ProduksiController(ILogProduksiRepository repository)
        {
            _repository = repository;
        }

        // Endpoint POST: api/produksi/catat
        [HttpPost("catat")]
        public async Task<IActionResult> CatatProduksi([FromBody] LogProduksi payload)
        {
            // Set ID dan Waktu otomatis dari sisi server jika sensor tidak mengirimkannya
            payload.Id = Guid.NewGuid();

            // Menggunakan UTC adalah praktik terbaik untuk time-series database
            payload.WaktuDeteksi = DateTime.UtcNow;

            try
            {
                var result = await _repository.CatatProduksiAsync(payload);
                return Ok(new
                {
                    Status = "Sukses",
                    Pesan = $"Berhasil mencatat {payload.Jumlah} barang dari mesin {payload.IdMesin}."
                });
            }
            catch (Exception ex)
            {
                // Sangat berguna untuk melihat error jika tabel database belum ada
                return StatusCode(500, new { Pesan = "Terjadi kesalahan server", Error = ex.Message });
            }
        }

        // Endpoint GET: api/produksi/rekap
        [HttpGet("rekap")]
        public async Task<IActionResult> AmbilRekap()
        {
            var data = await _repository.AmbilSemuaLogAsync();
            return Ok(data);
        }

        [HttpGet("rekap/{id}")]
        public async Task<IActionResult> AmbilRekapByid(Guid id)
        {
            // Sekarang variabel 'id' sudah ditangkap dari URL dan bisa diteruskan ke database
            var data = await _repository.AmbilByIdLogAsync(id);

            // Opsional: Cek jika data tidak ditemukan
            if (data == null)
            {
                return NotFound(new { Pesan = "Data log produksi tidak ditemukan." });
            }

            return Ok(data);
        }
    }
}