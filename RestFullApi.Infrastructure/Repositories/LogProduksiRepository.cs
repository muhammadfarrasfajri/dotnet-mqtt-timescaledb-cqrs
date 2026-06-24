using Dapper;
using Npgsql;
using RestFullApi.Application.Interfaces;
using RestFullApi.Domain;
using Microsoft.Extensions.Configuration;

namespace RestFullApi.Infrastructure.Repositories
{
    // Kelas ini "mewarisi" dan wajib mematuhi kontrak dari ILogProduksiRepository
    public class LogProduksiRepository : ILogProduksiRepository
    {
        private readonly string _connectionString;

        // Constructor untuk menangkap string koneksi dari Dependency Injection
        public LogProduksiRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<int> CatatProduksiAsync(LogProduksi log)
        {
            // Menggunakan koneksi Npgsql ke PostgreSQL
            using var connection = new NpgsqlConnection(_connectionString);

            // Menulis raw SQL
            var sql = @"
                INSERT INTO log_produksi (id, id_mesin, jumlah, waktu_deteksi) 
                VALUES (@Id, @IdMesin, @Jumlah, @WaktuDeteksi)";

            // Dapper secara otomatis memetakan properti dari objek 'log' ke dalam parameter @ raw SQL
            return await connection.ExecuteAsync(sql, log);
        }

        public async Task<IEnumerable<LogProduksi>> AmbilSemuaLogAsync()
        {
            using var connection = new NpgsqlConnection(_connectionString);

            var sql = "SELECT id, id_mesin, jumlah, waktu_deteksi FROM log_produksi ORDER BY waktu_deteksi DESC LIMIT 100";

            // Dapper secara otomatis membungkus hasil kembalian SQL ke dalam List objek Domain
            return await connection.QueryAsync<LogProduksi>(sql);
        }
        // 1. Hapus IEnumerable, jadikan tipe kembaliannya objek tunggal
        public async Task<LogProduksi> AmbilByIdLogAsync(Guid id)
        {
            using var connection = new NpgsqlConnection(_connectionString);

            var sql = "SELECT id, id_mesin, jumlah, waktu_deteksi FROM log_produksi WHERE id = @id";

            // 2. Gunakan QueryFirstOrDefaultAsync yang dirancang untuk mengambil 1 baris (atau null)
            return await connection.QueryFirstOrDefaultAsync<LogProduksi>(sql, new { id = id });
        }
    }
}