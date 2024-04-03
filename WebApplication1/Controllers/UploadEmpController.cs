using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using OfficeOpenXml;
using System.Data.SqlClient;
using System.Data;
using System;
using WebApplication1.Models;

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
        //data insertion
        [HttpPost("upload")]
        public IActionResult UploadExcelFile([FromForm] Microsoft.AspNetCore.Http.IFormFile file)
        {
            if (file == null || file.Length <= 0)
            {
                return BadRequest("No file uploaded.");
            }

            try
            {
                using (var package = new ExcelPackage(file.OpenReadStream()))
                {
                    var worksheet = package.Workbook.Worksheets[0]; // Assuming data is in the first sheet

                    // Iterate through rows, skipping the header row
                    for (int row = 2; row <= worksheet.Dimension.Rows; row++)
                    {
                        var employee = new Employee
                        {
                            Name = worksheet.Cells[row, 1].Value?.ToString(),
                            FirstName = worksheet.Cells[row, 2].Value?.ToString(),
                            MiddleName = worksheet.Cells[row, 3].Value?.ToString(),
                            LastName = worksheet.Cells[row, 4].Value?.ToString(),
                            MaidenName = worksheet.Cells[row, 5].Value?.ToString(),
                            Birthdate = worksheet.Cells[row, 6].Value?.ToString(),
                            Age = worksheet.Cells[row, 7].Value?.ToString(),
                            BirthMonth = worksheet.Cells[row, 8].Value?.ToString(),
                            AgeBracket = worksheet.Cells[row, 9].Value?.ToString(),
                            Gender = worksheet.Cells[row, 10].Value?.ToString(),
                            MaritalStatus = worksheet.Cells[row, 11].Value?.ToString(),
                            SSS = worksheet.Cells[row, 12].Value?.ToString(),
                            PHIC = worksheet.Cells[row, 13].Value?.ToString(),
                            HDMF = worksheet.Cells[row, 14].Value?.ToString(),
                            TIN = worksheet.Cells[row, 15].Value?.ToString(),
                            HRANID = worksheet.Cells[row, 16].Value?.ToString(),
                            ContactNumber = worksheet.Cells[row, 17].Value?.ToString(),
                            EmailAddress = worksheet.Cells[row, 18].Value?.ToString()
                        };

                        InsertEmployee(employee);
                    }
                }

                return Ok("Data inserted successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        private void InsertEmployee(Employee employee)
        {
            string query = @"
                INSERT INTO dbo.Employee (Name, FirstName, MiddleName, LastName, MaidenName, Birthdate, Age, BirthMonth, 
                AgeBracket, Gender, MaritalStatus, SSS, PHIC, HDMF, TIN, HRANID, ContactNumber, EmailAddress) 
                VALUES (@Name, @FirstName, @MiddleName, @LastName, @MaidenName, @Birthdate, @Age, @BirthMonth, @AgeBracket, 
                @Gender, @MaritalStatus, @SSS, @PHIC, @HDMF, @TIN, @HRANID, @ContactNumber, @EmailAddress)";

            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("EmployeeCon")))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Name", employee.Name);
                    command.Parameters.AddWithValue("@FirstName", employee.FirstName);
                    command.Parameters.AddWithValue("@MiddleName", employee.MiddleName);
                    command.Parameters.AddWithValue("@LastName", employee.LastName);
                    command.Parameters.AddWithValue("@MaidenName", employee.MaidenName);
                    command.Parameters.AddWithValue("@Birthdate", employee.Birthdate);
                    command.Parameters.AddWithValue("@Age", employee.Age);
                    command.Parameters.AddWithValue("@BirthMonth", employee.BirthMonth);
                    command.Parameters.AddWithValue("@AgeBracket", employee.AgeBracket);
                    command.Parameters.AddWithValue("@Gender", employee.Gender);
                    command.Parameters.AddWithValue("@MaritalStatus", employee.MaritalStatus);
                    command.Parameters.AddWithValue("@SSS", employee.SSS);
                    command.Parameters.AddWithValue("@PHIC", employee.PHIC);
                    command.Parameters.AddWithValue("@HDMF", employee.HDMF);
                    command.Parameters.AddWithValue("@TIN", employee.TIN);
                    command.Parameters.AddWithValue("@HRANID", employee.HRANID);
                    command.Parameters.AddWithValue("@ContactNumber", employee.ContactNumber);
                    command.Parameters.AddWithValue("@EmailAddress", employee.EmailAddress);

                    connection.Open();
                    command.ExecuteNonQuery();
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
