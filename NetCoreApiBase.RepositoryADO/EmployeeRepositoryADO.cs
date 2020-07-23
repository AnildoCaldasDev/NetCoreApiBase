using Microsoft.Extensions.Configuration;
using NetCoreApiBase.Contracts;
using NetCoreApiBase.Domain.Models;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Data;

namespace NetCoreApiBase.RepositoryADO
{
    public class EmployeeRepositoryADO : IEmployeeRepositoryADO
    {
        //Ver este artigo para deixar mais organizado o acesso a dados:
        //http://www.ijz.today/2016/09/net-core-10-connecting-sql-server.html


        private string GetConnectionStringADO()
        {
            return @"Data Source=.\SQLExpress;database=NetCoreBaseADO;User ID=sa;Password=sa2020";
        }


        public IQueryable<Employee> FindAll()
        {
            List<Employee> listEmplyees = new List<Employee>();

            using (SqlConnection con = new SqlConnection(this.GetConnectionStringADO()))
            {
                SqlCommand cmd = new SqlCommand("SpGetAllEmployees", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                SqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    var employee = new Employee()
                    {
                        Id = Convert.ToInt32(rdr["Id"]),
                        Name = rdr["Name"].ToString(),
                        Position = rdr["Position"].ToString(),
                        Office = rdr["Office"].ToString(),
                        Age = Convert.ToInt32(rdr["Age"]),
                        Salary = Convert.ToInt32(rdr["Salary"]),
                    };

                    listEmplyees.Add(employee);
                }
            }

            return listEmplyees.AsQueryable();
        }

        public IQueryable<Employee> FindByCondition(Expression<Func<Employee, bool>> expression)
        {
            List<Employee> listEmplyees = new List<Employee>();

            using (SqlConnection con = new SqlConnection(this.GetConnectionStringADO()))
            {
                SqlCommand cmd = new SqlCommand("SpGetEmployeeById", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", 2);//only for tests
                con.Open();

                SqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    var employee = new Employee()
                    {
                        Id = Convert.ToInt32(rdr["Id"]),
                        Name = rdr["Name"].ToString(),
                        Position = rdr["Position"].ToString(),
                        Office = rdr["Office"].ToString(),
                        Age = Convert.ToInt32(rdr["Age"]),
                        Salary = Convert.ToInt32(rdr["Salary"]),
                    };

                    listEmplyees.Add(employee);
                }
            }

            return listEmplyees.AsQueryable();
        }

        public async Task Create(Employee employee)
        {
            using (SqlConnection con = new SqlConnection(this.GetConnectionStringADO()))
            {
                SqlCommand cmd = new SqlCommand("SpAddEmployee", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Name", employee.Name);
                cmd.Parameters.AddWithValue("@Position", employee.Position);
                cmd.Parameters.AddWithValue("@Office", employee.Office);
                cmd.Parameters.AddWithValue("@Age", employee.Age);
                cmd.Parameters.AddWithValue("@Salary", employee.Salary);
                await con.OpenAsync();

                await cmd.ExecuteNonQueryAsync();

                await con.CloseAsync();
            }
        }

        public async Task Delete(Employee employee)
        {
            using (SqlConnection con = new SqlConnection(this.GetConnectionStringADO()))
            {
                var cmd = new SqlCommand("SpDeleteEmployee", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", employee.Id);

                await con.OpenAsync();

                await cmd.ExecuteNonQueryAsync();

                await con.CloseAsync();
            }
        }

        public async Task Update(Employee employee)
        {
            using (SqlConnection con = new SqlConnection(this.GetConnectionStringADO()))
            {
                var cmd = new SqlCommand("SpUpdateEmployee", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", employee.Id);
                cmd.Parameters.AddWithValue("@Name", employee.Name);
                cmd.Parameters.AddWithValue("@Position", employee.Position);
                cmd.Parameters.AddWithValue("@Office", employee.Office);
                cmd.Parameters.AddWithValue("@Age", employee.Age);
                cmd.Parameters.AddWithValue("@Salary", employee.Salary);
                await con.OpenAsync();

                await cmd.ExecuteNonQueryAsync();

                await con.CloseAsync();
            }
        }
    }
}
