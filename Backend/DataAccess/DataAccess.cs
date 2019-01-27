using Backend.Models;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;

namespace Backend.DataAccess
{
    public class DataAccess: IDataAccess
    {
        private readonly IConfiguration _configuration;
        private readonly string _mySqlConnectionString;

        public DataAccess(IConfiguration configuration)
        {
            _configuration = configuration;
            _mySqlConnectionString = _configuration["MySqlConnectionString"];
        }

        public async Task<bool> CreateUser(User user)
        {
            string query = @"INSERT INTO testerds.user (
                                id,
                                creation_date,
                                email,
                                name,
                                password,
                                matricula,
                                faculdade,
                                is_teacher) VALUES (
                                @UserId,
                                @CreationDate,
                                @Email,
                                @Name,
                                @Password,
                                @Matricula,
                                @Faculdade,
                                @IsTeacher);
            ";
            using (var connection = new MySqlConnection(_mySqlConnectionString))
            {
                return 1 == (await connection.ExecuteAsync(query, user));
            }
        }


        public async Task<IEnumerable<User>> GetAllUsers()
        {
            string query = @"SELECT
                                id AS UserId,
                                creation_date AS CreationDate,
                                email AS Email,
                                name AS Name,
                                password AS Password,
                                matricula AS Matricula,
                                faculdade AS Faculdade,
                                is_teacher AS IsTeacher
                            FROM
                                testerds.user;
            ";

            using (var connection = new MySqlConnection(_mySqlConnectionString))
            {
                return await connection.QueryAsync<User>(query);
            }
        }

        public async Task<User> GetUserByUsername(string userName)
        {
            string query = @"SELECT
                                id AS UserId,
                                creation_date AS CreationDate,
                                email AS Email,
                                name AS Name,
                                password AS Password,
                                matricula AS Matricula,
                                faculdade AS Faculdade,
                                is_teacher AS IsTeacher
                            FROM
                                testerds.user
                            WHERE
                                email = @userName;
            ";

            using (var connection = new MySqlConnection(_mySqlConnectionString))
            {
                return (await connection.QueryAsync<User>(query,new { userName })).FirstOrDefault();
            }
        }

        public async Task<bool> DeleteUserById(Guid userId)
        {
            string query = @"DELETE FROM testerds.user WHERE id = @userId";
            using (var connection = new MySqlConnection(_mySqlConnectionString))
            {
                return 1 == await connection.ExecuteAsync(query, new { userId });
            }
        }
    }
}
