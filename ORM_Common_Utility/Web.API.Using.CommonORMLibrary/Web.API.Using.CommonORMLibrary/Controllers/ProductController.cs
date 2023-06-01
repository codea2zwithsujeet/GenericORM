using EFCore.Generic.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;
using Web.API.Using.CommonORMLibrary.Data;
using Web.API.Using.CommonORMLibrary.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Web.API.Using.CommonORMLibrary.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IDapperRepository<DapperContext> _daprepository;
        public ProductController(IDapperRepository<DapperContext> daprepository)
        {
            _daprepository = daprepository;
        }

        [HttpGet("GetProducts")]
        public async Task<IActionResult> GetProducts()
        {
            var query = "SELECT * FROM Products";
            var resultr = await _daprepository.QueryAsync<Product>(query);
         
            if (resultr == null)
            {
                return NotFound();
            }

            return Ok(resultr);
        }
        [HttpPost("AddProduct")]
        public async Task<int> AddAsync(Product entity)
        {
            entity.AddedOn = DateTime.Now;
            var sql = "Insert into Products (Name,Description,Barcode,Rate,AddedOn) VALUES (@Name,@Description,@Barcode,@Rate,@AddedOn)";
            var resultr = await _daprepository.AddAsync<Product>(entity, sql);
            return resultr;
        }
    }
}
