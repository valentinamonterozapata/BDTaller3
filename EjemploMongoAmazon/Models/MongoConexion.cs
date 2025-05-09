using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EjemploMongoAmazon.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace EjemploMongoAmazon.models
{
    class MongoConexion
    {
        private readonly IMongoCollection<Producto> _productos;
        private readonly IMongoCollection<Categoria> _categorias;

        public MongoConexion()
        {
            var settings = MongoClientSettings.FromUrl(
                new MongoUrl("mongodb+srv://valentinamontero:q5Ac9hwxdQLr6Xdp@bduao.ppatzx9.mongodb.net/?retryWrites=true&w=majority&appName=BDuao") // Añade "/Amazonv1"
            );

            settings.SslSettings = new SslSettings
            {
                EnabledSslProtocols = System.Security.Authentication.SslProtocols.Tls12
            };

            var client = new MongoClient(settings);
            var database = client.GetDatabase("Amazonv1");
            _productos = database.GetCollection<Producto>("products");
        }

        public async Task InsertProductoAsync(Producto producto)
        {
            try
            {
                await _productos.InsertOneAsync(producto);
                Console.WriteLine($"Producto insertado con ID: {producto.Id}"); // Para depuración
            }
            //catch (MongoWriteException ex) when (ex.WriteError.Category == ServerErrorCategory.DuplicateKey)
            //{
            //    throw new Exception("Error: ID duplicado");
            //}
            catch (Exception ex)
            {
                throw new Exception($"Error al insertar: {ex.Message}");
            }
        }

        public async Task UpdateFirstProductActiveStatusAsync()
        {
            var filter = Builders<Producto>.Filter.Empty;
            var firstProduct = await _productos.Find(filter).FirstOrDefaultAsync();

            if (firstProduct != null)
            {
                var update = Builders<Producto>.Update.Set(p => p.ActiveProduct, true);
                await _productos.UpdateOneAsync(p => p.Id == firstProduct.Id, update);
            }
        }

        // 2. Obtener producto por ID y devolverlo
        public async Task<Producto> GetProductByIdAsync(string id)
        {
            var filter = Builders<Producto>.Filter.Eq(p => p.Id, id);
            return await _productos.Find(filter).FirstOrDefaultAsync();
        }

        // 4. Consultar productos por categoría
        public async Task<List<Producto>> GetProductsByCategoryAsync(string category)
        {
            var filter = Builders<Producto>.Filter.Eq(p => p.Category, category);
            return await _productos.Find(filter).ToListAsync();
        }
    }
}
