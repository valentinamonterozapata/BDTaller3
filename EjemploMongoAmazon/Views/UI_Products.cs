using EjemploMongoAmazon.models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.IO;
using EjemploMongoAmazon.Models;
using MongoDB.Driver;


namespace EjemploMongoAmazon.Views
{
    public partial class UI_Products: Form
    {
        string nameImage;
        private MongoConexion _conexionMongo;
        private string category;

        public UI_Products()
        {
            InitializeComponent();
            _conexionMongo = new MongoConexion();
            LoadCategoriesComboBox();
        }

        private void LoadCategoriesComboBox()
        {
            // Aquí puedes cargar categorías desde la base de datos si las tienes en una colección separada
            comboBox1.Items.AddRange(new string[] { "Alimentos", "Bebidas", "Limpieza", "Electrónica" });
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog1 = new OpenFileDialog();
                openFileDialog1.Filter = "Image files | *.jpg";
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    nameImage = openFileDialog1.FileName;
                    Console.WriteLine("Name file" + openFileDialog1.FileName);
                    pictureBox1.Image = Image.FromFile(openFileDialog1.FileName);
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar la imagen: " + ex.Message);
            }
        }

        private async void button1_Click(object sender, EventArgs e) // Añade "async"
        {
            try
            {
                // Validación de campos
                if (string.IsNullOrEmpty(textBox1.Text) || string.IsNullOrEmpty(textBox2.Text) ||
                    string.IsNullOrEmpty(textBox3.Text) || string.IsNullOrEmpty(textBox4.Text) ||
                    string.IsNullOrEmpty(nameImage) || comboBox1.SelectedItem == null)
                {
                    MessageBox.Show("Complete todos los campos y seleccione una categoría");
                    return;
                }

                // Obtener la categoría seleccionada
                string category = comboBox1.SelectedItem.ToString(); // Corrige la asignación

                // Crear el objeto Producto
                Producto objP = new Producto(
                    nameProduct: textBox1.Text,
                    priceProduct: double.Parse(textBox2.Text),
                    descriptionProduct: textBox3.Text,
                    amountProduct: int.Parse(textBox4.Text),
                    imageProduct: File.ReadAllBytes(nameImage),
                    dateRegistration: DateTime.UtcNow,
                    active: true,
                    category: category // Usa la categoría seleccionada
                );

                // Insertar en MongoDB
                await _conexionMongo.InsertProductoAsync(objP); // Quita el comentario y usa await

                MessageBox.Show("Producto guardado exitosamente");
            }
            catch (FormatException)
            {
                MessageBox.Show("Formato incorrecto en precio o cantidad");
            }
            catch (MongoWriteException ex)
            {
                MessageBox.Show($"Error de MongoDB: {ex.Message}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error inesperado: {ex.Message}");
            }
        }

        private async void button3_Click(object sender, EventArgs e)
        {
            // Se utiliza la instancia existente de _conexionMongo en lugar de intentar acceder al método como si fuera estático
            await _conexionMongo.UpdateFirstProductActiveStatusAsync();
            MessageBox.Show("Primer producto actualizado con active=true");
        }

        private async void button4_Click(object sender, EventArgs e)
        {
            string productId = textBox5.Text;
            if (string.IsNullOrEmpty(productId))
            {
                MessageBox.Show("Por favor ingrese un ID de producto");
                return;
            }

            // Se utiliza la instancia existente de _conexionMongo en lugar de intentar acceder al método como si fuera estático
            var product = await _conexionMongo.GetProductByIdAsync(productId);
            if (product != null)
            {
                textBox1.Text = product.NameProduct;
                textBox2.Text = product.PriceProduct.ToString();
                textBox3.Text = product.DescriptionProduct;
                textBox4.Text = product.AmountProduct.ToString();

                if (product.ImagenProduct != null && product.ImagenProduct.Length > 0)
                {
                    using (var ms = new MemoryStream(product.ImagenProduct))
                    {
                        pictureBox1.Image = Image.FromStream(ms);
                    }
                }
            }
            else
            {
                MessageBox.Show("Producto no encontrado");
            }
        }

        private async void button5_Click(object sender, EventArgs e)
        {
            string category = comboBox1.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(category))
            {
                MessageBox.Show("Por favor seleccione una categoría");
                return;
            }

            // Se utiliza la instancia existente de _conexionMongo en lugar de intentar acceder al método como si fuera estático
            var products = await _conexionMongo.GetProductsByCategoryAsync(category);
            dataGridView1.DataSource = products.Select(p => new
            {
                p.Id,
                p.NameProduct,
                p.PriceProduct,
                p.DescriptionProduct,
                p.AmountProduct,
                p.Category,
                p.ActiveProduct
            }).ToList();
        }
    }
}

