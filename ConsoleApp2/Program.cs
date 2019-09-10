using System;
using System.Collections.Generic;
using CompanyApp.Controller;
using CompanyApp.Model;
using ConsoleApp.Controller;

namespace ConsoleApp
{
    class Program
    {
        
        const string CONNECTION_STRING = "Data Source=tappqa;Initial Catalog=Training-MM-Company;Integrated Security=True";
        static void Main(string[] args)
        {
            CompanyController companyController = new CompanyController(CONNECTION_STRING);
            AddressController addressController = new AddressController(CONNECTION_STRING);
            Controller.EmployeeController employeeController = new Controller.EmployeeController(CONNECTION_STRING);
            Controller.DepartmentController departmentController = new Controller.DepartmentController(CONNECTION_STRING);

            Console.BackgroundColor = ConsoleColor.Red;
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.White;

            Console.WriteLine("Choose an Option!");
            Console.WriteLine("-----------------");
            Console.WriteLine("1. Company!");
            Console.WriteLine("2. Employee!");
            Console.WriteLine("3. Address!");
            Console.WriteLine("4. Department!");
            Console.WriteLine("5. Close Window!");
            Console.WriteLine("-----------------");

            switch (Console.ReadLine())
            {
                case "1":
                    Console.WriteLine("Choose an Option!");
                    Console.WriteLine("-----------------");
                    Console.WriteLine("1. Add a company !");
                    Console.WriteLine("2. Read all companies!");
                    Console.WriteLine("3. Update a company!");
                    Console.WriteLine("4. Delete a company!");
                    Console.WriteLine("5. Close window!");
                    Console.WriteLine("-----------------");
                    switch (Console.ReadLine())
                    {
                        case "1": CreateCompany(companyController); break;
                        case "2": ReadCompanies(companyController); break;
                        case "3": UpdateCompany(companyController); break;
                        case "4": DeleteCompany(companyController); break;
                        case "5": Environment.Exit(0); break;
                        default: break;
                    }
                    Console.Write("Press any key to come back to the main menu...");
                    Console.ReadKey();
                    Main(args);
                    break;

                case "2":
                    Console.WriteLine("Choose an Option!");
                    Console.WriteLine("-----------------");
                    Console.WriteLine("1. Add a employee !");
                    Console.WriteLine("2. Read all employees!");
                    Console.WriteLine("3. Update a employee!");
                    Console.WriteLine("4. Delete a employee!");
                    Console.WriteLine("5. Close window!");
                    Console.WriteLine("-----------------");
                    switch (Console.ReadLine())
                    {
                        case "1": CreateEmployee(employeeController); break;
                        case "2": ReadEmployees(employeeController); break;
                        case "3": UpdateEmployee(employeeController); break;
                        case "4": DeleteEmployee(employeeController); break;
                        case "5": Environment.Exit(0); break;
                        default: break;
                    }
                    Console.Write("Press any key to come back to the main menu...");
                    Console.ReadKey();
                    Main(args);
                    break;

                case "3":
                    Console.WriteLine("Choose an Option!");
                    Console.WriteLine("-----------------");
                    Console.WriteLine("1. Add an address !");
                    Console.WriteLine("2. Read all address!");
                    Console.WriteLine("3. Update an address!");
                    Console.WriteLine("4. Delete an address!");
                    Console.WriteLine("5. Close window!");
                    Console.WriteLine("-----------------");
                    switch (Console.ReadLine())
                    {
                        case "1": CreateAddress(addressController); break;
                        case "2": ReadAddress(addressController); break;
                        case "3": UpdateAddress(addressController); break;
                        case "4": DeleteAddress(addressController); break;
                        case "5": Environment.Exit(0); break;
                        default: break;
                    }
                    Console.Write("Press any key to come back to the main menu...");
                    Console.ReadKey();
                    Main(args);
                    break;

                //case "4":
                //    Console.WriteLine("Choose an Option!");
                //    Console.WriteLine("-----------------");
                //    Console.WriteLine("1. Add a department!");
                //    Console.WriteLine("2. Read all department!");
                //    Console.WriteLine("3. Update an department!");
                //    Console.WriteLine("4. Delete an department!");
                //    Console.WriteLine("5. Close window!");
                //    Console.WriteLine("-----------------");
                //    switch (Console.ReadLine())
                //    {
                //        case "1": CreateDepartment(departmentController); break;
                //        case "2": ReadDepartment(departmentController); break;
                //        case "3": UpdateDepartment(departmentController); break;
                //        case "4": DeleteDepartment(departmentController); break;
                //        case "5": Environment.Exit(0); break;
                //        default: break;
                //    }
                //    Console.Write("Press any key to come back to the main menu...");
                //    Console.ReadKey();
                //    Main(args);
                //    break;

                case "5": Environment.Exit(0); break;

                default: break;




                 
            }
            

            Console.Write("Press any key to come back to the main menu...");
            Console.ReadKey();
            Main(args);
        }

        static void CreateCompany(CompanyController companyController)
        {
            Console.WriteLine("--------------------------------------");
            Console.WriteLine("Name of the company?");
            Console.WriteLine("--------------------------------------");
            string nameOfCompany = Console.ReadLine();
            Console.WriteLine("--------------------------------------");

            Console.WriteLine("FoundedDate of the company?");
            Console.WriteLine("--------------------------------------");
            DateTime foundedDateOfCompany = Convert.ToDateTime(Console.ReadLine());
            Console.WriteLine("--------------------------------------");

            companyController.Create(nameOfCompany, foundedDateOfCompany);
        }

        static void ReadCompanies(CompanyController companyController)
        {
            List<Company> companies = companyController.Read();

                Console.WriteLine("".PadRight(80, '-'));
            Console.WriteLine($"| {"Id".PadRight(8)} | {"Name".PadRight(32)}| {"Founding Date".PadRight(32)}|");
            Console.WriteLine("".PadRight(80, '-'));
            for (int i = 0; i < companies.Count; i++)
            {
                Console.WriteLine($"| {companies[i].Id.ToString().PadRight(8)} | {companies[i].Name.PadRight(32)}| {companies[i].FoundedDate.ToString("MM/dd/yyyy").PadRight(32)}|");
            }
            Console.WriteLine("".PadRight(80, '-'));
        }

        static void UpdateCompany(CompanyController companyController)
        {
            Console.WriteLine("--------------------------------------");
            Console.WriteLine("Id of the company");
            Console.WriteLine("--------------------------------------");
            int id = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("--------------------------------------");

            Console.WriteLine("Name of the company?");
            Console.WriteLine("--------------------------------------");
            string nameOfCompany = Console.ReadLine();
            Console.WriteLine("--------------------------------------");


            Console.WriteLine("FoundedDate of the company?");
            Console.WriteLine("--------------------------------------");
            DateTime foundedDateOfCompany = Convert.ToDateTime(Console.ReadLine());
            Console.WriteLine("--------------------------------------");

            companyController.Update(id, nameOfCompany, foundedDateOfCompany);
        }

        static void DeleteCompany(CompanyController companyController)
        {
            Console.WriteLine("Id of the company");
            int id = Convert.ToInt32(Console.ReadLine());

            companyController.Delete(id);
        }
        static void CreateEmployee(Controller.EmployeeController employeeController)
        {
            Console.WriteLine("--------------------------------------");
            Console.WriteLine("Firstname of the employee?");
            Console.WriteLine("--------------------------------------");
            string firstname = Console.ReadLine();

            Console.WriteLine("--------------------------------------");
            Console.WriteLine("Lastname of the employee?");
            Console.WriteLine("--------------------------------------");
            string lastname = Console.ReadLine();
            

            Console.WriteLine("--------------------------------------");
            Console.WriteLine("BeginDate of the employee?");
            Console.WriteLine("--------------------------------------");
            DateTime beginDate = Convert.ToDateTime(Console.ReadLine());

            Console.WriteLine("--------------------------------------");
            Console.WriteLine("DepartmentId of the employee?");
            Console.WriteLine("--------------------------------------");
            int departmentId = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("--------------------------------------");
            Console.WriteLine("AddressId of the employee?");
            Console.WriteLine("--------------------------------------");
            int addressId = Convert.ToInt32(Console.ReadLine());

            employeeController.Create(firstname, lastname, beginDate, departmentId, addressId);
        }
        static void ReadEmployees(Controller.EmployeeController employeeController)
        {
            List<ConsoleApp.Model.Employee> employees = employeeController.ReadDapper();

            Console.WriteLine("".PadRight(112, '-'));
            Console.WriteLine($"| {"Id".PadRight(8)} | {"Firstname".PadRight(16)} | {"Lastname".PadRight(16)} | {"Begin Date".PadRight(32)} | {"DepartmentId".PadRight(12)} | {"AddressId".PadRight(8)} |");
            Console.WriteLine("".PadRight(112, '-'));
            for (int i = 0; i < employees.Count; i++)
            {
                Console.WriteLine($"| {employees[i].Id.ToString().PadRight(8)} | {employees[i].FirstName.PadRight(16)} | {employees[i].LastName.PadRight(16)} | {employees[i].BeginDate.ToString("MM/dd/yyyy").PadRight(32)} | {employees[i].DepartmentId.ToString().PadRight(12)} | {employees[i].AddressId.ToString().PadRight(8)}  |");
            }
            Console.WriteLine("".PadRight(112, '-'));
        }
        static void UpdateEmployee(Controller.EmployeeController employeeController)
        {
            Console.WriteLine("--------------------------------------");
            Console.WriteLine("Id of the employee");
            Console.WriteLine("--------------------------------------");
            int id = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("--------------------------------------");

            Console.WriteLine("Firstname of the employee?");
            Console.WriteLine("--------------------------------------");
            string firstName = Console.ReadLine();
            Console.WriteLine("--------------------------------------");

            Console.WriteLine("Lastname of the employee?");
            Console.WriteLine("--------------------------------------");
            string lastName = Console.ReadLine();
            Console.WriteLine("--------------------------------------");

            Console.WriteLine("Begin date of the employee?");
            Console.WriteLine("--------------------------------------");
            DateTime beginDate = Convert.ToDateTime(Console.ReadLine());
            Console.WriteLine("--------------------------------------");

            Console.WriteLine("DepartmentId of the employee");
            Console.WriteLine("--------------------------------------");
            int departmentId = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("--------------------------------------");

            Console.WriteLine("AddressId of the employee");
            Console.WriteLine("--------------------------------------");
            int addressId = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("--------------------------------------");

            employeeController.Update(id, firstName, lastName, beginDate, departmentId, addressId);
        }
        static void DeleteEmployee(Controller.EmployeeController employeeController)
        {
            Console.WriteLine("Id of employee");
            int id = Convert.ToInt32(Console.ReadLine());

            employeeController.Delete(id);
        }

        static void CreateAddress(Controller.AddressController addressController)
        {
            Console.WriteLine("--------------------------------------");
            Console.WriteLine("Street?");
            Console.WriteLine("--------------------------------------");
            string street = Console.ReadLine();

            Console.WriteLine("--------------------------------------");
            Console.WriteLine("City?");
            Console.WriteLine("--------------------------------------");
            string city = Console.ReadLine();


            Console.WriteLine("--------------------------------------");
            Console.WriteLine("Zip?");
            Console.WriteLine("--------------------------------------");
            string zip = Console.ReadLine();

            Console.WriteLine("--------------------------------------");
            Console.WriteLine("Country?");
            Console.WriteLine("--------------------------------------");
            string country = Console.ReadLine();
           

            addressController.Create(street, city, zip, country);
        }
        static void ReadAddress(AddressController addressController)
        {
            List<ConsoleApp.Model.Address> addresses = addressController.ReadDapper();

            Console.WriteLine("".PadRight(88, '-'));
            Console.WriteLine($"| {"Id".PadRight(8)} | {"Street".PadRight(16)} | {"City".PadRight(16)} | {"ZIP".PadRight(16)} | {"Country".PadRight(16)} |");
            Console.WriteLine("".PadRight(88, '-'));
            for (int i = 0; i < addresses.Count; i++)
            {
                Console.WriteLine($"| {addresses[i].Id.ToString().PadRight(8)} | {addresses[i].Street.PadRight(16)} | {addresses[i].City.PadRight(16)} | {addresses[i].Zip.PadRight(16)} | {addresses[i].Country.PadRight(16)} |");
            }
            Console.WriteLine("".PadRight(88, '-'));
        }
        static void UpdateAddress(Controller.AddressController addressController)
        {
            Console.WriteLine("--------------------------------------");
            Console.WriteLine("Id of the address");
            Console.WriteLine("--------------------------------------");
            int id = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("--------------------------------------");

            Console.WriteLine("Street?");
            Console.WriteLine("--------------------------------------");
            string street = Console.ReadLine();
            Console.WriteLine("--------------------------------------");

            Console.WriteLine("City?");
            Console.WriteLine("--------------------------------------");
            string city = Console.ReadLine();
            Console.WriteLine("--------------------------------------");

            Console.WriteLine("ZIP?");
            Console.WriteLine("--------------------------------------");
            string zIP = Console.ReadLine();
            Console.WriteLine("--------------------------------------");

            Console.WriteLine("Country");
            Console.WriteLine("--------------------------------------");
            string country = Console.ReadLine();
            Console.WriteLine("--------------------------------------");
            

            addressController.Update(id, street, city, zIP, country);
        }
        static void DeleteAddress(Controller.AddressController addressController)
        {
            Console.WriteLine("Id of address");
            int id = Convert.ToInt32(Console.ReadLine());

            addressController.Delete(id);
        }

        //static void CreateDepartment(Controller.DepartmentController departmentController)
        //{
        //    Console.WriteLine("--------------------------------------");
        //    Console.WriteLine("Name?");
        //    Console.WriteLine("--------------------------------------");
        //    string street = Console.ReadLine();

        //    Console.WriteLine("--------------------------------------");
        //    Console.WriteLine("Description?");
        //    Console.WriteLine("--------------------------------------");
        //    string city = Console.ReadLine();


        //    Console.WriteLine("--------------------------------------");
        //    Console.WriteLine("CompanyId?");
        //    Console.WriteLine("--------------------------------------");
        //    int companyId = Console.ReadLine();


        //    departmentController.Create(street, city, zip, country);
        //}
        //static void ReadAddress(Controller.AddressController addressController)
        //{
        //    List<ConsoleApp.Model.Address> addresses = addressController.ReadDapper();

        //    Console.WriteLine("".PadRight(88, '-'));
        //    Console.WriteLine($"| {"Id".PadRight(8)} | {"Street".PadRight(16)} | {"City".PadRight(16)} | {"ZIP".PadRight(16)} | {"Country".PadRight(16)} |");
        //    Console.WriteLine("".PadRight(88, '-'));
        //    for (int i = 0; i < addresses.Count; i++)
        //    {
        //        Console.WriteLine($"| {addresses[i].Id.ToString().PadRight(8)} | {addresses[i].Street.PadRight(16)} | {addresses[i].City.PadRight(16)} | {addresses[i].Zip.PadRight(16)} | {addresses[i].Country.PadRight(16)} |");
        //    }
        //    Console.WriteLine("".PadRight(88, '-'));
        //}
        //static void UpdateAddress(Controller.AddressController addressController)
        //{
        //    Console.WriteLine("--------------------------------------");
        //    Console.WriteLine("Id of the address");
        //    Console.WriteLine("--------------------------------------");
        //    int id = Convert.ToInt32(Console.ReadLine());
        //    Console.WriteLine("--------------------------------------");

        //    Console.WriteLine("Street?");
        //    Console.WriteLine("--------------------------------------");
        //    string street = Console.ReadLine();
        //    Console.WriteLine("--------------------------------------");

        //    Console.WriteLine("City?");
        //    Console.WriteLine("--------------------------------------");
        //    string city = Console.ReadLine();
        //    Console.WriteLine("--------------------------------------");

        //    Console.WriteLine("ZIP?");
        //    Console.WriteLine("--------------------------------------");
        //    string zIP = Console.ReadLine();
        //    Console.WriteLine("--------------------------------------");

        //    Console.WriteLine("Country");
        //    Console.WriteLine("--------------------------------------");
        //    string country = Console.ReadLine();
        //    Console.WriteLine("--------------------------------------");


        //    addressController.Update(id, street, city, zIP, country);
        //}
        //static void DeleteAddress(Controller.AddressController addressController)
        //{
        //    Console.WriteLine("Id of address");
        //    int id = Convert.ToInt32(Console.ReadLine());

        //    addressController.Delete(id);
        //}
    }
}
