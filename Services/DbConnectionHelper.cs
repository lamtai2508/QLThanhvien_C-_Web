namespace QLThanhvien_Web.Services;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

public class DbConnectionHelper
{
    private readonly IConfiguration _configuration;

    public DbConnectionHelper(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public MySqlConnection GetConnection()
    {
        var connStr = _configuration.GetConnectionString("DefaultConnection");
        return new MySqlConnection(connStr);
    }
}
