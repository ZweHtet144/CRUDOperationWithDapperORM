using Dapper;
using DapperCRUDOperation.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace DapperCRUDOperation.Controllers
{
    public class StudentController : GetConnectionStringController
    {
        private IConfiguration _configuration;
        public StudentController(IConfiguration configuration) : base(configuration)
        {
            this._configuration = configuration;
        }
        [HttpGet]
        [Route("getstudent")]
        public async Task<string> GetAllStudent()
        {
            try
            {
                string Query = "SELECT * from TBLStudent";
                SqlConnection con = this.OpenConnection("DESKTOP-B20FQO7");
                var Student = await con.QueryAsync<StudentModel>(Query);
                this.CloseConnection();
                var lst = Student.ToList();
                return JsonConvert.SerializeObject(lst);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        [Route("create")]
        public async Task<string> CreateStudent([FromBody] StudentModel model)
        {
            try
            {
                var Name = CheckStudentNameExist(model.StudentName);
                if (Name != null)
                {
                    return "StudentName Already Exist";
                }
                String Query = "INSERT INTO TBLStudent(StudentCode,StudentName) VALUES(@StudentCode,@StudentName)";
                var parameter = new DynamicParameters();
                parameter.Add("@StudentCode", model.StudentCode);
                parameter.Add("@StudentName", model.StudentName);
                SqlConnection con = this.OpenConnection("DESKTOP-B20FQO7");
                var result = await con.ExecuteAsync(Query, parameter);
                this.CloseConnection();
                return "true";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public StudentModel CheckStudentNameExist(string studentName)
        {
            try
            {
                string Query = "SELECT StudentName FROM TBLStudent WHERE StudentName=@StudentName";
                SqlConnection con = this.OpenConnection("DESKTOP-B20FQO7");
                var std = con.Query<StudentModel>(Query, new { studentName = studentName }).FirstOrDefault();
                this.CloseConnection();
                return std;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPut]
        [Route("update")]
        public async Task<bool> UpdateStudent([FromBody] StudentModel model)
        {
            try
            {
                string Query = "UPDATE TBLStudent SET StudentName=@StudentName WHERE StudentId=@StudentId";
                var parameter = new DynamicParameters();
                parameter.Add("@StudentId", model.StudentId);
                parameter.Add("@StudentName", model.StudentName);
                SqlConnection con = this.OpenConnection("DESKTOP-B20FQO7");
                await con.ExecuteAsync(Query, parameter);
                this.CloseConnection();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        [Route("createbystoreprocedure")]
        public async Task<bool> InsertStudent([FromBody] StudentModel model)
        {
            try
            {
                SqlConnection con = this.OpenConnection("DESKTOP-B20FQO7");
                var parameter = new DynamicParameters();
                parameter.Add("@StudentCode", model.StudentCode);
                parameter.Add("@StudentName", model.StudentName);
                var result = await con.ExecuteAsync("CreateStudent", parameter, commandType: CommandType.StoredProcedure);
                this.CloseConnection();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [Route("getbystoreprocedure")]
        public async Task<List<StudentModel>> GetAllStudents()
        {
            try
            {
                SqlConnection con = this.OpenConnection("DESKTOP-B20FQO7");
                var result = await con.QueryAsync<StudentModel>("GetAllStudent", commandType: CommandType.StoredProcedure);
                this.CloseConnection();
                var lstStudent = result.ToList();
                return lstStudent;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        [Route("getstudentbyId")]
        public async Task<StudentModel> GetStudentById(int Id)
        {
            try
            {
                SqlConnection con = this.OpenConnection("DESKTOP-B20FQO7");
                var parameter = new DynamicParameters();
                parameter.Add("@StudentId", Id);
                var result = await con.QueryAsync<StudentModel>("GetStudentById", parameter, commandType: CommandType.StoredProcedure);
                this.CloseConnection();
                var model = result.FirstOrDefault();
                return model;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
