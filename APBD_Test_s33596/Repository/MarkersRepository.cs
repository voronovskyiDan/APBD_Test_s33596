using APBD_Test_s33596.DTOs.Request;
using APBD_Test_s33596.DTOs.Response;
using APBD_Test_s33596.Excpetions;
using Microsoft.Data.SqlClient;

namespace APBD_Test_s33596.Repository
{
    public class MarkersRepository : IMarkersRepository
    {
        private readonly string _connectionString;
        public MarkersRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Default")
                ?? throw new Exception("No connection string provided");
        }

        public async Task<int> AddMarker(AddMarketDto dto)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                int newMakerId;
                await connection.OpenAsync();

                await using var transaction = (SqlTransaction)await connection.BeginTransactionAsync();
                try
                {
                    var mainQuery = @"INSERT INTO Makers (Name)
                                    VALUES (@Name);
                                    SELECT SCOPE_IDENTITY();";
                    using (SqlCommand command = new SqlCommand(mainQuery, connection, transaction))
                    {
                        command.Parameters.AddWithValue("@Name", dto.Name);

                        var newId = await command.ExecuteScalarAsync();
                        newMakerId = Convert.ToInt32(newId);
                    }

                    foreach (var product in dto.Products)
                    {
                        await addProdcutToMaker(connection, transaction, newMakerId, product);
                    }

                    await transaction.CommitAsync();
                    return newMakerId;
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }

            }
        }
        private async Task addProdcutToMaker(SqlConnection connection, SqlTransaction transaction, int makerId, ProductRequestDto product)
        {
            var query = @"INSERT INTO Products (Name, Description, StickerPrice, MakerId, ProductTypeId)
                        VALUES (@Name, @Description, @StickerPrice, @MakerId, @ProductTypeId);
                        SELECT SCOPE_IDENTITY();";
            using (SqlCommand command = new SqlCommand(query, connection, transaction))
            {
                command.Parameters.AddWithValue("@Name", product.Name);
                command.Parameters.AddWithValue("@Description", product.Description);
                command.Parameters.AddWithValue("@StickerPrice", product.StickerPrice);
                command.Parameters.AddWithValue("@MakerId", makerId);
                command.Parameters.AddWithValue("@ProductTypeId", await getProuctTypeId(connection, transaction, product.Type));
                
                var newProductId = await command.ExecuteScalarAsync();
                
                int productId = Convert.ToInt32(newProductId);
            }
        }
        private async Task<int> getProuctTypeId(SqlConnection connection, SqlTransaction transaction, string typeName)
        {
            var query = @"SELECT Id FROM ProductTypes WHERE Name = @Name";
            using (SqlCommand command = new SqlCommand(query, connection, transaction))
            {
                command.Parameters.AddWithValue("@Name", typeName);
                var result = await command.ExecuteScalarAsync();
                if (result == null)
                    throw new NotFoundException($"Product type '{typeName}' not found.");
                return Convert.ToInt32(result);
            }
        }

        public async Task<MakersDetailsDto?> GetMarkersDetails(int id)
        {
            MakersDetailsDto markersDetailsDto = new();
            string query = @"SELECT name FROM Makers
                            WHERE Id = @Id";
            using SqlConnection connection = new SqlConnection(_connectionString);

            await connection.OpenAsync();
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Id", id);

                using SqlDataReader reader = await command.ExecuteReaderAsync();

                if (!await reader.ReadAsync())
                    return null;

                markersDetailsDto.Name = reader.GetString(0);
            }

            markersDetailsDto.Products = await getProducts(connection, id);

            return markersDetailsDto;
        }
        private async Task<List<ProductResponseDto>> getProducts(SqlConnection connection, int id)
        {
            List<ProductResponseDto> products = new();
            string query = @"SELECT p.Id, p.Name, p.Description, p.stickerPrice, pt.Id, pt.Name FROM Products p
                            JOIN ProductTypes pt ON p.ProductTypeId = pt.Id
                            WHERE p.MakerId = @Id";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Id", id);
                using SqlDataReader reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    ProductResponseDto product = new ProductResponseDto
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Description = reader.GetString(2),
                        StrickerPrice = reader.GetDecimal(3),
                        ProductType = new TypeResponseDto
                        {
                            Id = reader.GetInt32(4),
                            Name = reader.GetString(5)
                        }
                    };
                    products.Add(product);
                }
            }
            foreach (var product in products)
            {
                product.Vendors = await getVendorOfProduct(connection, product.Id);
            }
            return products;
        }
        private async Task<List<VendorResponseDto>> getVendorOfProduct(SqlConnection connection, int productId)
        {
            List<VendorResponseDto> res = [];
            string query = @"SELECT v.Code, v.Name, vp.Amount, vp.PricePerUnit FROM Vendors v
                            JOIN VendorProducts vp ON v.Code = vp.VendorCode
                            WHERE vp.ProductId = @Id";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Id", productId);

                using SqlDataReader reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    res.Add(new VendorResponseDto
                    {
                        Code = reader.GetString(0),
                        Name = reader.GetString(1),
                        Amount = reader.GetInt32(2),
                        PricePerUnit = reader.GetDecimal(3)
                    });
                }
            }

            return res;
        }

    }
}