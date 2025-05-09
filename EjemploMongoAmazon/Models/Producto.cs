using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace EjemploMongoAmazon.models
{
    class Producto
    {
 
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("nameProduct")]
        public string NameProduct { get; set; }

        [BsonElement("priceProduct")]
        public double PriceProduct { get; set; }

        [BsonElement("descriptionProduct")]
        public string DescriptionProduct { get; set; }

        [BsonElement("AmountProduct")]
        public int AmountProduct { get; set; }

        [BsonElement("imageProduct")]
        public byte[] ImagenProduct { get; set; }

        [BsonElement("dateRegistration")]
        public DateTime DateRegistration { get; set; } = DateTime.UtcNow;

        [BsonElement("active")]
        public bool ActiveProduct { get; set; }

        [BsonElement("category")]
        public string Category { get; set; }

        public Producto(string nameProduct, double priceProduct, string descriptionProduct, int amountProduct, byte[] imageProduct, DateTime dateRegistration, bool active, string category)
        {
            NameProduct = nameProduct;
            PriceProduct = priceProduct;
            DescriptionProduct = descriptionProduct;
            AmountProduct = amountProduct;
            ImagenProduct = imageProduct;
            DateRegistration = dateRegistration;
            ActiveProduct = active;
            Category = category;
        }
    }
}
