using API.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _environment;

        public EmployeesController(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _environment = env;
        }


        [HttpGet]
        public JsonResult Get()
        {
            string query = @"
                select EmployeeId, EmployeeName, Department, convert(varchar(10),DateOfJoining,120) as DateOfJoining, PhotoFileName from dbo.Employee";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeContext");

            SqlDataReader dataReader;
            using (SqlConnection connection = new SqlConnection(sqlDataSource))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    dataReader = command.ExecuteReader();
                    table.Load(dataReader); ;
                    dataReader.Close();
                    connection.Close();
                }
            }
            return new JsonResult(table);
        }


        [HttpPost]
        public JsonResult Post(Employee employee)
        {
            string query = @"
                insert into dbo.Employee(EmployeeName,Department,DateOfJoining,PhotoFileName) values 
                (
                '" + employee.EmployeeName + @"',
                '" + employee.Department + @"',
                '" + employee.DateOfJoining + @"',
                '" + employee.PhotoFileName + @"'
                )";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeContext");

            SqlDataReader dataReader;
            using (SqlConnection connection = new SqlConnection(sqlDataSource))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    dataReader = command.ExecuteReader();
                    table.Load(dataReader); ;
                    dataReader.Close();
                    connection.Close();
                }
            }
            return new JsonResult("Department is Added successfully");
        }


        [HttpPut]
        public JsonResult Put(Employee employee)
        {
            string query = @"
                update dbo.Employee set
                EmployeeName = '" + employee.EmployeeName + @"',
                Department = '" + employee.Department + @"',
                DateOfJoining = '" + employee.DateOfJoining + @"'
                where EmployeeId = " + employee.EmployeeID + @"";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeContext");

            SqlDataReader dataReader;
            using (SqlConnection connection = new SqlConnection(sqlDataSource))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    dataReader = command.ExecuteReader();
                    table.Load(dataReader); ;
                    dataReader.Close();
                    connection.Close();
                }
            }
            return new JsonResult("Department is Updated successfully");
        }

        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            string query = @"
                delete from dbo.Employee where EmployeeId = " + id + @"";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeContext");

            SqlDataReader dataReader;
            using (SqlConnection connection = new SqlConnection(sqlDataSource))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    dataReader = command.ExecuteReader();
                    table.Load(dataReader); ;
                    dataReader.Close();
                    connection.Close();
                }
            }
            return new JsonResult("Department is Deleted successfully");
        }

        [Route("SaveFile")]
        [HttpPost]

        public JsonResult SaveFile()
        {
            try
            {
                var httpRequest = Request.Form;
                var postedFile = httpRequest.Files[0];
                string fileName = postedFile.FileName;
                var physicalPath = _environment.ContentRootPath + "/Photos" + fileName;

                using (var stream = new FileStream(physicalPath, FileMode.Create))
                {
                    postedFile.CopyTo(stream);
                }
                return new JsonResult(fileName);
            }
            catch(Exception)
            {
                return new JsonResult("photo.png");
            }

        }

        [Route ("GetAllDepartmentNames")]
        public JsonResult GetAllDepartmentNames()
        {
            string query = @"
                select  DepartmentName from dbo.Department";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeContext");

            SqlDataReader dataReader;
            using (SqlConnection connection = new SqlConnection(sqlDataSource))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    dataReader = command.ExecuteReader();
                    table.Load(dataReader); ;
                    dataReader.Close();
                    connection.Close();
                }
            }
            return new JsonResult(table);
        }

    }
}
