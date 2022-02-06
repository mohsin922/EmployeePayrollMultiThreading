using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace EmployeePayrollUsingThreads
{
    public class EmployeePayrollOperations
    {
        /* UC0:- Ability to create a payroll service database and have C# program connect to database.
                - Use the payroll_service database created in MSSQL.
                - Install System.Data.SqlClient Package.
                - Check if the database connection to payroll_service mssql DB is established.
        */

        public static string connectionString = @"Server=LAPTOP-AT654SBH\MSSQLSERVER01;Database=payroll_service;Trusted_Connection=True;"; 


        SqlConnection sqlConnection = new SqlConnection(connectionString);  

        public bool DataBaseConnection()
        {
            try
            {
                DateTime now = DateTime.Now;  
                sqlConnection.Open();
                using (sqlConnection)  
                {
                    Console.WriteLine($"Connection is created Successful {now}"); 

                }
                sqlConnection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return true;
        }

        /* UC1:- Ability to add multiple employee to payroll DB.
                 - Use the payroll_service database created in MS SQL
                 - Use addEmployeeToPayroll previously created along with ADO.NET Transaction.
                 - Record the start and stop time to essentially determine the time taken for the execution
                 - Use MSTest and TDD approach for all Use Cases.
                 - Ensure every Use Cases is a working code and is committed in GIT.
        */
        public List<EmployeeDetails> employeePayrollDetailList = new List<EmployeeDetails>();
        public void addEmployeeToPayroll(List<EmployeeDetails> employeePayrollDataList)
        {
            employeePayrollDataList.ForEach(employeeData =>
            {
                Console.WriteLine(" Employee being added: " + employeeData.EmployeeName);
                this.addEmployeePayroll(employeeData);
                Console.WriteLine(" Employee added: " + employeeData.EmployeeName);
            });

            Console.WriteLine(this.employeePayrollDetailList.ToString());
        }

        public void addEmployeePayroll(EmployeeDetails emp)
        {
            employeePayrollDetailList.Add(emp);
        }

        /* UC2:- Ability to add multiple employee to payroll DB using Threads so as to get a better response
                 - Use the payroll_service database created in MS SQL
                 - Ensure addEmployeeToPayroll is part of its own execution thread
                 - Record the start and stop time to essentially determine the time taken for the execution 
                 using Thread and without Thread to check the performance.
        */
        public void addEmployeeToPayrollWithThread(List<EmployeeDetails> employeePayrollDataList)
        {
            employeePayrollDataList.ForEach(employeeData =>
            {
                Task thread = new Task(() =>
                {
                    Console.WriteLine(" Employee being added: " + employeeData.EmployeeName);
                    this.addEmployeePayroll(employeeData);
                    Console.WriteLine(" Employee added: " + employeeData.EmployeeName);
                });
                thread.Start();
            });
            Console.WriteLine(this.employeePayrollDetailList.Count);
        }

        public bool AddEmployeeToDataBase(EmployeeDetails model)
        {
            try
            {
                using (sqlConnection)
                {
                    SqlCommand command = new SqlCommand("dbo.SpAddEmpolyeeDetails", this.sqlConnection);   //Creating a stored Procedure for adding employees into database

                    command.CommandType = CommandType.StoredProcedure; 

                    command.Parameters.AddWithValue("@EmployeeName", model.EmployeeName);
                    command.Parameters.AddWithValue("@PhoneNumber", model.PhoneNumber);
                    command.Parameters.AddWithValue("@Address", model.Address);
                    command.Parameters.AddWithValue("@Department", model.Department);
                    command.Parameters.AddWithValue("@Gender", model.Gender);
                    command.Parameters.AddWithValue("@BasicPay", model.BasicPay);
                    command.Parameters.AddWithValue("@Deductions", model.Deductions);
                    command.Parameters.AddWithValue("@TaxablePay", model.TaxablePay);
                    command.Parameters.AddWithValue("@Tax", model.Tax);
                    command.Parameters.AddWithValue("@NetPay", model.NetPay);
                    command.Parameters.AddWithValue("@StartDate", model.StartDate);
                    command.Parameters.AddWithValue("@City", model.City);
                    command.Parameters.AddWithValue("@Country", model.Country);
                    sqlConnection.Open();
                    var result = command.ExecuteNonQuery();
                    sqlConnection.Close();

                    if (result != 0)
                    {
                        return true;
                    }
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                sqlConnection.Close();
            }
        }
        public int EmployeeCount()
        {
            return this.employeePayrollDetailList.Count;
        }

        public void Display()
        {
            foreach (EmployeeDetails employee in this.employeePayrollDetailList)
            {
                Console.WriteLine(employee.EmployeeID + " " + employee.EmployeeName + " " + employee.PhoneNumber + " " + employee.Address
                    + " " + employee.Department + " " + employee.Gender + " " + employee.BasicPay + " " + employee.Deductions + " " +
                    employee.TaxablePay + " " + employee.Tax + " " + employee.NetPay + " " + employee.StartDate + " " + employee.City
                    + " " + employee.Gender);
            }
        }

    }
}