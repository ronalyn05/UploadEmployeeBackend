using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using OfficeOpenXml;
using System.Data.SqlClient;
using System.Data;
using System;
using WebApplication1.Models;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using System.Collections.Generic; // Add this namespace for JSON handling

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class UploadEmpController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public UploadEmpController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        //Data retrieval
        [HttpGet]
        public JsonResult Get()
        {
            string query = @"
                            select EmpID, Name, FirstName, LastName, MiddleName, MaidenName, Birthdate, Age,
                                   BirthMonth, AgeBracket, Gender, MaritalStatus, SSS, PHIC, HDMF, 
                                   TIN, HRANID, ContactNumber, EmailAddress
                            from
                            dbo.Employee
                            ";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult(table);
        }
        // Data insertion endpoint
        [HttpPost("SaveData")]
        public IActionResult SaveData(List<Employee> employees)
        {
            try
            {
                // Perform data insertion logic here
                foreach (var employee in employees)
                {
                    InsertEmployee(employee); // Call a method to insert each employee into the database
                }

                return Ok("Data saved successfully!");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }
        private void InsertEmployee(Employee employee)
        {
            string query = @"
                INSERT INTO dbo.Employee (Name, FirstName, LastName, MiddleName, MaidenName,
                                           Birthdate, Age, BirthMonth, AgeBracket, Gender,
                                           MaritalStatus, SSS, PHIC, HDMF, TIN, HRANID,
                                           ContactNumber, EmailAddress)
                VALUES (@Name, @FirstName, @LastName, @MiddleName, @MaidenName,
                        @Birthdate, @Age, @BirthMonth, @AgeBracket, @Gender,
                        @MaritalStatus, @SSS, @PHIC, @HDMF, @TIN, @HRANID,
                        @ContactNumber, @EmailAddress)
            ";

            using (SqlConnection myCon = new SqlConnection(_configuration.GetConnectionString("EmployeeCon")))
            {
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@Name", employee.Name);
                    myCommand.Parameters.AddWithValue("@FirstName", employee.FirstName);
                    myCommand.Parameters.AddWithValue("@LastName", employee.LastName);
                    myCommand.Parameters.AddWithValue("@MiddleName", employee.MiddleName);
                    myCommand.Parameters.AddWithValue("@MaidenName", employee.MaidenName);
                    myCommand.Parameters.AddWithValue("@Birthdate", employee.Birthdate);
                    myCommand.Parameters.AddWithValue("@Age", employee.Age);
                    myCommand.Parameters.AddWithValue("@BirthMonth", employee.BirthMonth);
                    myCommand.Parameters.AddWithValue("@AgeBracket", employee.AgeBracket);
                    myCommand.Parameters.AddWithValue("@Gender", employee.Gender);
                    myCommand.Parameters.AddWithValue("@MaritalStatus", employee.MaritalStatus);
                    myCommand.Parameters.AddWithValue("@SSS", employee.SSS);
                    myCommand.Parameters.AddWithValue("@PHIC", employee.PHIC);
                    myCommand.Parameters.AddWithValue("@HDMF", employee.HDMF);
                    myCommand.Parameters.AddWithValue("@TIN", employee.TIN);
                    myCommand.Parameters.AddWithValue("@HRANID", employee.HRANID);
                    myCommand.Parameters.AddWithValue("@ContactNumber", employee.ContactNumber);
                    myCommand.Parameters.AddWithValue("@EmailAddress", employee.EmailAddress);

                    myCon.Open();
                    myCommand.ExecuteNonQuery();
                    myCon.Close();
                }
            }
        }
        //Data Update
        [HttpPut]
        public JsonResult Put(Employee employee)
        {
            string query = @"
                    update dbo.Employee
                    set Name = @Name,
                        FirstName = @FirstName,
                        LastName = @LastName,
                        MiddleName = @MiddleName,
                        MaidenName = @MaidenName,
                        Birthdate = @Birthdate,
                        Age = @Age,
                        BirthMonth = @BirthMonth,
                        AgeBracket = @AgeBracket,
                        Gender = @Gender,
                        MaritalStatus = @MaritalStatus,
                        SSS = @SSS,
                        PHIC = @PHIC,
                        HDMF = @HDMF,
                        TIN = @TIN,
                        HRANID = @HRANID,
                        ContactNumber = @ContactNumber,
                        EmailAddress = @EmailAddress
                    where EmpID = @EmpID 
                    ";
            using (SqlConnection myCon = new SqlConnection(_configuration.GetConnectionString("EmployeeCon")))
            {
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@EmpID", employee.EmpID);
                    myCommand.Parameters.AddWithValue("@Name", employee.Name);
                    myCommand.Parameters.AddWithValue("@FirstName", employee.FirstName);
                    myCommand.Parameters.AddWithValue("@LastName", employee.LastName);
                    myCommand.Parameters.AddWithValue("@MiddleName", employee.MiddleName);
                    myCommand.Parameters.AddWithValue("@MaidenName", employee.MaidenName);
                    myCommand.Parameters.AddWithValue("@Birthdate", employee.Birthdate);
                    myCommand.Parameters.AddWithValue("@Age", employee.Age);
                    myCommand.Parameters.AddWithValue("@BirthMonth", employee.BirthMonth);
                    myCommand.Parameters.AddWithValue("@AgeBracket", employee.AgeBracket);
                    myCommand.Parameters.AddWithValue("@Gender", employee.Gender);
                    myCommand.Parameters.AddWithValue("@MaritalStatus", employee.MaritalStatus);
                    myCommand.Parameters.AddWithValue("@SSS", employee.SSS);
                    myCommand.Parameters.AddWithValue("@PHIC", employee.PHIC);
                    myCommand.Parameters.AddWithValue("@HDMF", employee.HDMF);
                    myCommand.Parameters.AddWithValue("@TIN", employee.TIN);
                    myCommand.Parameters.AddWithValue("@HRANID", employee.HRANID);
                    myCommand.Parameters.AddWithValue("@ContactNumber", employee.ContactNumber);
                    myCommand.Parameters.AddWithValue("@EmailAddress", employee.EmailAddress);

                    myCon.Open();
                    int rowsAffected = myCommand.ExecuteNonQuery();
                    myCon.Close();

                    if (rowsAffected > 0)
                    {
                        return new JsonResult("Data updated successfully!");
                    }
                    else
                    {
                        return new JsonResult("Failed to update data!");
                    }
                }
            }
        }

        //Data Deletion
        [HttpDelete("{EmpID}")]
        public JsonResult Delete(int EmpID)
        {
            string query = @"
                    delete from dbo.Employee
                    where EmpID = @EmpID 
                    ";
            using (SqlConnection myCon = new SqlConnection(_configuration.GetConnectionString("EmployeeCon")))
            {
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@EmpID", EmpID);

                    myCon.Open();
                    int rowsAffected = myCommand.ExecuteNonQuery();
                    myCon.Close();

                    if (rowsAffected > 0)
                    {
                        return new JsonResult("Data deleted successfully!");
                    }
                    else
                    {
                        return new JsonResult("Failed to delete data!");
                    }
                }
            }
        }

    }
}
