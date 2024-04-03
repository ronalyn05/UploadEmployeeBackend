using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Data;
using WebApplication1.Models;
using System;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserAccountController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public UserAccountController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        [HttpPost("login")]
        public IActionResult Login(UserAccount userCredentials)
        {
            string query = @"
                    SELECT UserId, LastName, FirstName, MiddleName, UserName, Email, Password
                    FROM dbo.UserAccount
                    WHERE Email = @Email AND Password = @Password
                    ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeCon");
            SqlDataReader myReader;

            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@Email", userCredentials.Email);
                    myCommand.Parameters.AddWithValue("@Password", userCredentials.Password);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }

            if (table.Rows.Count == 1)
            {
                DataRow row = table.Rows[0];
                UserAccount user = new UserAccount
                {
                    UserId = Convert.ToInt32(row["UserId"]),
                    LastName = Convert.ToString(row["LastName"]),
                    FirstName = Convert.ToString(row["FirstName"]),
                    MiddleName = Convert.ToString(row["MiddleName"]),
                    UserName = Convert.ToString(row["UserName"]),
                    Email = Convert.ToString(row["Email"]),
                    Password = Convert.ToString(row["Password"])
                };
                // Store user's name in session
                HttpContext.Session.SetString("UserName", user.UserName);

                return Ok(user);
            }
            else
            {
                return Unauthorized(); // Return 401 if login failed
            }
        }

        // Data Insertion for User Registration
        [HttpPost("register")]
        public IActionResult Register(UserAccount userAccount)
        {
            string query = @"
            INSERT INTO dbo.UserAccount (LastName, FirstName, MiddleName, UserName, Email, Password)
            VALUES (@LastName, @FirstName, @MiddleName, @UserName, @Email, @Password)
            ";
            string sqlDataSource = _configuration.GetConnectionString("EmployeeCon");

            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@FirstName", userAccount.FirstName);
                    myCommand.Parameters.AddWithValue("@LastName", userAccount.LastName);
                    myCommand.Parameters.AddWithValue("@MiddleName", userAccount.MiddleName);
                    myCommand.Parameters.AddWithValue("@UserName", userAccount.UserName);
                    myCommand.Parameters.AddWithValue("@Email", userAccount.Email);
                    myCommand.Parameters.AddWithValue("@Password", userAccount.Password);

                    myCon.Open();
                    int rowsAffected = myCommand.ExecuteNonQuery();
                    myCon.Close();

                    if (rowsAffected > 0)
                    {
                        return Ok(new { message = "Account registered successfully!" });
                    }
                    else
                    {
                        return BadRequest(new { message = "Failed to register account!" });
                    }
                }
            }
        }

        //Data Deletion
        [HttpDelete("{UserId}")]
        public JsonResult Delete(int UserId)
        {
            string query = @"
                    delete from dbo.UserAccount
                    where UserId = @UserId 
                    ";
            using (SqlConnection myCon = new SqlConnection(_configuration.GetConnectionString("EmployeeCon")))
            {
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@UserId", UserId);

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

        // Data retrieval
        [HttpGet]
        public JsonResult Get()
        {
            string query = @"
                    select UserId, LastName, FirstName, MiddleName, UserName, Email, Password
                    from
                    dbo.UserAccount
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

            // Check if any data is retrieved
            if (table.Rows.Count > 0)
            {
                return new JsonResult(table);
            }
            else
            {
                return new JsonResult("No data found");
            }
        }

        ////Data Insertion
        //[HttpPost]
        //public JsonResult Post(UserAccount userAccount)
        //{
        //    string query = @"
        //            insert into dbo.UserAccount
        //            values (@LastName, @FirstName, @MiddleName, @UserName, @Email, @Password)
        //            ";
        //    string sqlDataSource = _configuration.GetConnectionString("EmployeeCon");

        //    using (SqlConnection myCon = new SqlConnection(sqlDataSource))
        //    {
        //        using (SqlCommand myCommand = new SqlCommand(query, myCon))
        //        {
        //            myCommand.Parameters.AddWithValue("@FirstName", userAccount.FirstName);
        //            myCommand.Parameters.AddWithValue("@LastName", userAccount.LastName);
        //            myCommand.Parameters.AddWithValue("@MiddleName", userAccount.MiddleName);
        //            myCommand.Parameters.AddWithValue("@UserName", userAccount.UserName);
        //            myCommand.Parameters.AddWithValue("@Email", userAccount.Email);
        //            myCommand.Parameters.AddWithValue("@Password", userAccount.Password);

        //            myCon.Open();
        //            int rowsAffected = myCommand.ExecuteNonQuery();
        //            myCon.Close();

        //            if (rowsAffected > 0)
        //            {
        //                return new JsonResult("Account added successfully!");
        //            }
        //            else
        //            {
        //                return new JsonResult("Failed to add account!");
        //            }
        //        }
        //    }
        //}

    }
}
