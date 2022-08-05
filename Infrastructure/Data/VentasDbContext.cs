using System.Data;
using System.Data.SqlClient;

using Domain;

namespace Infrastructure;
public class VentasDbContext
{
    private readonly string _connectionString;
    public VentasDbContext(string connectionString)
    {
        _connectionString = connectionString;
    }

    public List<Venta> List()
    {
        var data = new List<Venta>();

        var con = new SqlConnection(_connectionString);
        var cmd = new SqlCommand("SELECT [Id],[ClienteId],[ProductoId],[Concepto] FROM [Ventas]", con);
        try
        {
            con.Open();
            var dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                data.Add(new Venta
                {
                    Id = (Guid)dr["Id"],
                    ClienteId = (Guid)dr["ClienteId"],
                    ProductoId = (Guid)dr["ProductoId"],
                    Concepto = (string)dr["Concepto"]
                });
            }
            return data;
        }
        catch (Exception)
        {
            throw;
        }
        finally
        {
            con.Close();
        }
    }

       public Venta Details(Guid id)
    {
        var data = new Venta();
        var con = new SqlConnection(_connectionString);
        var cmd = new SqlCommand(@"SELECT 
[V].[Id],
[V].[ClienteId],
    [C.][Nombre],
    [C.][Direccion],
    [C.][Telefono],
    [C.][Correo],
[V].[ProductoId],
    [P.][Descripcion],
    [P.][Precio],
    [P.][Cantidad],
    FROM [Ventas] AS [V]
    JOIN[Cliente] AS [C] ON [V].[ClienteId] = [C].[Id]
    JOIN[Producto] AS [P] ON [V].[ProductoId] = [P].[Id]
    WHERE [V].[Id] = @Id", con);
        cmd.Parameters.Add("@Id", SqlDbType.UniqueIdentifier).Value = id;
        try
        {
            con.Open();
            var dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                return new Venta
                {
                    Id = (Guid)dr["Id"],
                    ClienteId = (Guid)dr["ClienteId"],
                    Cliente = new Cliente
                    {
                        Id = (Guid)dr["Id"],
                        Nombre = (string)dr["Nombre"],
                        Direccion = (string)dr["Direccion"],
                        Telefono = (string)dr["Telefono"],
                        Correo = (string)dr["Correo"]

                    },
                    ProductoId = (Guid)dr["ProductoId"],
                    Producto = new Producto
                    {
                        Id = (Guid)dr["Id"],
                        Descripcion = (string)dr["Descripcion"],
                        Precio = (decimal)dr["Precio"],
                        Cantidad = (int)dr["Cantidad"]
                    }
                };
            }
            return data;
        }
        catch (Exception)
        {
            throw;
        }
        finally
        {
            // ToDo
            con.Close();
        }
    }
    

    public void Create(Venta data)
    {
        var con = new SqlConnection(_connectionString);
        var cmd = new SqlCommand("INSERT INTO [Ventas] ([ClienteId],[ProductoId],[Concepto]) VALUES (@ClienteId, @ProductoId , @Concepto)", con);
        cmd.Parameters.Add("ClienteId",  SqlDbType.UniqueIdentifier).Value = data.ClienteId;
        cmd.Parameters.Add("ProductoId",  SqlDbType.UniqueIdentifier).Value = data.ProductoId;
        cmd.Parameters.Add("Concepto",  SqlDbType.NVarChar,128).Value = data.Concepto;

        try
        {
            con.Open();
            cmd.ExecuteNonQuery();
        }
        catch (Exception)
        {
            throw;
        }
        finally
        {
            con.Close();
        }
    }

   public void Edit(Venta data)
    {
        var con = new SqlConnection(_connectionString);
        var cmd = new SqlCommand("UPDATE [Ventas] SET [ClienteId] = @ClienteId, [ProductoId] = @ProductoId, [Concepto] = @Concepto WHERE [Id] = @Id", con);
        cmd.Parameters.Add("ClienteId", SqlDbType.UniqueIdentifier).Value = data.ClienteId;
        cmd.Parameters.Add("ProductoId", SqlDbType.UniqueIdentifier).Value = data.ProductoId;
        cmd.Parameters.Add("Concepto", SqlDbType.NVarChar, 128).Value = data.Concepto;
        try
        {
            con.Open();
            cmd.ExecuteNonQuery();
        }
        catch (Exception)
        {
            throw;
        }
        finally
        {
            con.Close();
        }
    }


      public void Delete(Guid id)
    {
        var con = new SqlConnection(_connectionString);
        var cmd = new SqlCommand("DELETE FROM [Ventas] WHERE [Id] = @Id", con);
        cmd.Parameters.Add("Id", SqlDbType.UniqueIdentifier).Value = id;
        try
        {
            con.Open();
            cmd.ExecuteNonQuery();
        }
        catch (Exception)
        {
            throw;
        }
        finally
        {
            con.Close();
        }
    }
}