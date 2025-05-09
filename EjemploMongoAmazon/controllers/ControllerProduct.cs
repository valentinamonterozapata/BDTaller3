//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using MongoDB.Driver;
//using EjemploMongoAmazon.models;
//using EjemploMongoAmazon.Models;

//namespace EjemploMongoAmazon.controllers
//{

//    public class ControllerProduct
//    {
//        private readonly MongoConexion _conexion;

//        public ControllerProduct()
//        {
//            _conexion = new MongoConexion();
//        }

//        public Task<bool> InsertProductAsync(Producto producto)
//        {
//            try
//            {
//                await _conexion.InsertProductoAsync(producto);
//                return true;
//            }
//            catch
//            {
//                return false;
//            }
//        }
//    }
//}
