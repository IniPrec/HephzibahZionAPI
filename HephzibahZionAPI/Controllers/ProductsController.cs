using HephzibahZionAPI.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace HephzibahZionAPI.Controllers
{
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public ProductsController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        [Route("GetAllProduct")]

        public List<Products> GetAllProducts()
        {
            List<Products> products = new List<Products>();
            SqlConnection connect = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            SqlCommand cmd = new SqlCommand("select * from Products;", connect);
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataTable data = new DataTable();
            adapter.Fill(data);

            for(int i = 0; i < data.Rows.Count; i++)
            {
                Products columns = new Products();
                columns.ProductId = int.Parse(data.Rows[i]["ProductId"].ToString());
                columns.ProductName = data.Rows[i]["ProductName"].ToString();
                columns.Category = data.Rows[i]["Category"].ToString();
                columns.Size = data.Rows[i]["Size"].ToString();
                columns.Price = float.Parse(data.Rows[i]["Price"].ToString());

                products.Add(columns);
            }

            return products;
        }

        [HttpPost]
        [Route("SaveProducts")]
        public string SaveProduct(Products products)
        {
            SqlConnection connect2 = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            SqlCommand cmd2 = new SqlCommand("INSERT INTO Products VALUES(@ProductName, @Category, @Size, @Price)", connect2);

            cmd2.Parameters.AddWithValue("@ProductName", products.ProductName);
            cmd2.Parameters.AddWithValue("@Category", products.Category);
            cmd2.Parameters.AddWithValue("@Size", products.Size);
            cmd2.Parameters.AddWithValue("@Price", products.Price);

            connect2.Open();
            cmd2.ExecuteNonQuery();
            connect2.Close();

            return "Product Saved Successfully!";
        }

        [HttpPut]
        [Route("UpdateProduct")]
        public string UpdateProduct(int productId, Products updatedProduct)
        {
            SqlConnection connect3 = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            SqlCommand cmd3 = new SqlCommand("UPDATE Products SET ProductName = @ProductName, Category = @Category, Size = @Size, Price = @Price WHERE ProductId = @ProductId", connect3);

            cmd3.Parameters.AddWithValue("@ProductId", productId);
            cmd3.Parameters.AddWithValue("@ProductName", updatedProduct.ProductName);
            cmd3.Parameters.AddWithValue("@Category", updatedProduct.Category);
            cmd3.Parameters.AddWithValue("@Size", updatedProduct.Size);
            cmd3.Parameters.AddWithValue("@Price", updatedProduct.Price);

            connect3.Open();
            cmd3.ExecuteNonQuery();
            connect3.Close();

            return "Product Updated Successfully!";
        }

        [HttpDelete]
        [Route("DeleteProduct")]
        public string DeleteProduct(int productId) 
        {
            SqlConnection connect4 = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            SqlCommand cmd4 = new SqlCommand("DELETE FROM Products WHERE ProductId = @ProductId", connect4);

            cmd4.Parameters.AddWithValue("@ProductId", productId);

            connect4.Open();
            cmd4.ExecuteNonQuery();
            connect4.Close();

            return "Product Deleted Successfully";
        }

    }
}
