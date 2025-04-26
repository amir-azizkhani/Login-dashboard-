using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Threading;
using System.Text.RegularExpressions;
using System.Data.Entity;
using System.Data.Entity.Validation;

namespace Finale_Project_CSharp
{
    class Program
    {
        static void Main(string[] args)
        {
            #region Actions

            Action PrintDatetimeAndUserStatistic = () =>
            {
                Console.Clear();
                PersianCalendar pc = new PersianCalendar();
                Console.WriteLine($"{pc.GetYear(DateTime.Now)}/{pc.GetMonth(DateTime.Now)}/{pc.GetDayOfMonth(DateTime.Now)}  ,  {pc.GetHour(DateTime.Now)}:{pc.GetMinute(DateTime.Now)}:{pc.GetSecond(DateTime.Now)}");

                using (DbProjectContext dbPrintTime = new DbProjectContext("ProjectConnStr"))
                {
                    Console.WriteLine($"\n***\nDashboard Statistic:\tEmployee:{dbPrintTime.Employees.Count()}\tMaster:{dbPrintTime.Masters.Count()}\tStudents:{dbPrintTime.Students.Count()}\tCourses:{dbPrintTime.Courses.Count()}");
                }
                Console.WriteLine("_____________________________________________________________________________________");
            };

            Action<string> PrintJustNumbers = delegate (string title)
            {
                PrintDatetimeAndUserStatistic();
                Console.WriteLine("Warning!\n");
                Thread.Sleep(2000);
                Console.WriteLine("\t Only numbers approved for {0}", title);
                Console.WriteLine("\tPlease wait a moment.");
                Thread.Sleep(5000);
            };

            Action<string, int> IdNotFound = delegate (string TitleList, int id)
            {
                PrintDatetimeAndUserStatistic();
                Console.WriteLine("\n\tWarning!");
                Thread.Sleep(1000);
                Console.WriteLine($"\tId:[{id}] Not found in {TitleList}!");
                Thread.Sleep(3000);
            };



            #endregion

            #region Creating database and adding default values

            Console.WriteLine("\n\n\tDatabase is loading...");

            using (DbProjectContext db = new DbProjectContext("ProjectConnStr"))
            {
                db.Database.CreateIfNotExists();
                Console.WriteLine();
                if (!db.Roles.Any())
                {
                    db.Roles.Add(new Roles(1, "Admin", "Manager"));
                    db.Roles.Add(new Roles(2, "Poshtiban", "Operator"));
                    db.Roles.Add(new Roles(3, "Ostad", "Master"));
                    db.Roles.Add(new Roles(4, "Daneshjoo", "Student"));
                }
                if (!db.Employees.Any())
                {
                    db.Employees.Add(new Employee("Finanical", 12000000, "Morad", "Goli", "09901567943", "123456", db.Roles.Find(2), 42));
                    db.Employees.Add(new Employee("Administrative", 10000000, "Abaas", "Miri", "09196482579", "1598753", db.Roles.Find(2), 38));
                    db.Employees.Add(new Employee("Managment", 15000000, "Maasoumeh", "Ahmadi", "09121223048", "025522", db.Roles.Find(2), 52));
                }
                if (!db.Masters.Any())
                {
                    db.Masters.Add(new Master("English literature", 18000000, "Ensieh", "Darzinezhad", "09221246849", "123456", db.Roles.Find(3), 40));
                    db.Masters.Add(new Master("English literature", 15000000, "Hadiseh", "Alishiri", "09356782459", "125678", db.Roles.Find(3), 45));
                    db.Masters.Add(new Master("English literature", 20000000, "Keyvan", "Shokri", "09164765821", "126789", db.Roles.Find(3), 50));

                }
                if (!db.Students.Any())
                {
                    db.Students.Add(new Student("English literature", "Hadiseh", "Farahizadeh", "09198944827", "11581158", db.Roles.Find(4), 22));
                    db.Students.Add(new Student("English literature", "Mahsa", "Azizkhani", "09354721509", "13861386", db.Roles.Find(4), 17));
                    db.Students.Add(new Student("English literature", "Amir ", "Azizkhani", "09904841439", "11581158", db.Roles.Find(4), 23));

                    Student student = new Student("Csharp", "Hajar", "mostafavi", "09106890282", "559980", db.Roles.Find(4), 48);
                    Master master = new Master("Csharp", 12000000, "Ali", "Mmgghh", "09192346798", "12345678", db.Roles.Find(3), 28);
                    Course course = new Course(2285, "Sql", 3, master);
                    student.Courses.Add(course);
                    db.Students.Add(student);
                }
                if (!db.Courses.Any())
                {
                    db.Courses.Add(new Course(95955, "History of literatur", 4, db.Masters.Add(new Master("English literature", 17000000, "Maasoumeh", "Bakhtiari", "09201246785", "124567", db.Roles.Find(3), 52))));
                    db.Courses.Add(new Course(23159, "Csharp language", 4, db.Masters.Add(new Master("Csharp Programming", 30000000, "Alireza", "Moghadam", "09122314508", "123589", db.Roles.Find(3), 28))));
                    Console.Clear();
                    Console.WriteLine("\n\n\tDatabase created successfully!");
                }
                db.SaveChanges();
                Console.WriteLine("\n\tRedirecting to login form.");
                Console.WriteLine("\tWait a moment!");
                Thread.Sleep(3000);
            }

        #endregion

        #region Login Page and the options

        LoginPage:

            Console.Clear();
            Console.WriteLine("Wellcome to login page.\n\tFill the information correctly to contuinue!");
            Console.Write("\n\n\tUsername: \n\t(!Your phonenumber consider as username!)");
            string username = Console.ReadLine();
            string phonePattern = @"^09[0-9]{9}$";
            while (!Regex.IsMatch(username, phonePattern))
            {
                Console.WriteLine("\n\tUsername is not in valid format.\n\tTry again.");
                Thread.Sleep(3000);
                Console.Clear();
                Console.WriteLine("Login page.\n\tFill the information correctly to contuinue!");
                Console.Write("\n\n\tPhonenumber: ");
                username = Console.ReadLine();
            }

            Console.Write("Password: ");
            string password = Console.ReadLine();

            string adminName = "";

            using (DbProjectContext dbLogin = new DbProjectContext("ProjectConnStr"))
            {
                var admin = dbLogin.Employees.FirstOrDefault(t => t.Phonenumber == username && t.Password == password);
                if (admin == null)
                {
                    Console.Clear();
                    Console.WriteLine("\nLogin page.");
                    Console.WriteLine("\n\tIncorrect username or password.");
                    Thread.Sleep(3000);
                    Console.Clear();
                    goto LoginPage;
                }
                else if (admin.IsActive == false)
                {
                    Console.Clear();
                    Console.WriteLine("\nLogin page.");
                    Console.WriteLine("\n\tSorry!\tYour access has been blocked!");
                    Thread.Sleep(3000);
                    Console.Clear();
                    goto LoginPage;
                }
                else
                {
                    adminName = admin.Name + " " + admin.LastName;
                    goto Menu;
                }
            }

        #endregion

        #region Menu options
        Menu:
            PrintDatetimeAndUserStatistic();

            Console.Write("\n\t1.View Employees list.");
            Console.WriteLine("\t\t\t2.View Students list.");
            Console.Write("\t3.View Masters list.");
            Console.WriteLine("\t\t\t4.View Courses list.");

            Console.Write("\n\t5.Add a new Employee.");
            Console.WriteLine("\t\t\t6.Add a new Student.");
            Console.Write("\t7.Add a new Master.");
            Console.WriteLine("\t\t\t8.Add a new Course.");

            Console.Write("\n\t9.Edit Employee Information.");
            Console.WriteLine("\t\t\t10.Edit Student Information.");
            Console.Write("\t11.Edit Master Information.");
            Console.WriteLine("\t\t\t12.Edit course Information.");

            Console.Write("\n\t13.Remove Employee.");
            Console.WriteLine("\t\t\t14.Remove Student.");
            Console.Write("\t15.Remove Master");
            Console.WriteLine("\t\t\t16.Remove Course.");

            Console.Write("\n\t17.Change Role of Users.");
            Console.WriteLine("\t\t\t18.Change Background color.");

            Console.Write("\n\t19.Exit Dashboard.");

            Console.Write("\nChoose an option : ");
            int option;
            while (!int.TryParse(Console.ReadLine(), out option))
            {
                PrintJustNumbers("Menu");
                Console.Clear();
                PrintDatetimeAndUserStatistic();
                goto Menu;
            }
            #endregion

            #region Switches
            switch (option)
            {
                #region Case 1: View employee
                case 1:

                    PrintDatetimeAndUserStatistic();
                    using (DbProjectContext dbViewEmployee = new DbProjectContext("ProjectConnStr"))
                    {
                        Console.WriteLine("\nEmployee:\n");
                        List<Employee> viewEmployee = dbViewEmployee.Employees.ToList();
                        foreach (Employee employee in viewEmployee)
                        {
                            Console.WriteLine($"Id:{employee.Id}\tName:{employee.Name}\tLast Name:{employee.LastName}\tDepartment:{employee.DepartmentName}\n" +
                                $"Phonenumber:{employee.Phonenumber}\tSalary:{employee.Salary}\tRegisterdate:{employee.Registerdate}\tActive:{employee.IsActive}");

                            Console.WriteLine("--------------------------------------------------------------------------------");
                        }
                    }
                    Console.Write("Press any key to return to menu.");
                    Console.ReadKey();
                    goto Menu;
                #endregion

                #region Case 2: View Students 
                case 2:

                    PrintDatetimeAndUserStatistic();
                    using (DbProjectContext dbViewStudent = new DbProjectContext("ProjectConnStr"))
                    {
                        Console.WriteLine("\nStudent:\n");
                        IEnumerable<Student> viewStudent = dbViewStudent.Students.ToList();
                        foreach (Student student in viewStudent)
                        {
                            Console.WriteLine($"Id:{student.Id}\tStudentCode:{student.StudentCode}\tName:{student.Name}\tLastName:{student.LastName}\tPhonenumber:{student.Phonenumber}\tBirthdate:{student.Birthdate}\n" +
                                              $"FieldOfStudy:{student.FieldOfStudy}\tTuitionOfEachTerm:{student.TuitionOfEachTerm}\tRegister Date:{student.Registerdate}\tActive:{student.IsActive}");
                            if (!student.Courses.Any())
                            {
                                Console.WriteLine("\n\tCourse:");
                                foreach (Course course in student.Courses)
                                {
                                    Console.WriteLine($"Id:{course.Id}\tCourseName:{course.CourseName}\tCourseUnit:{course.CourseUnit}\tIsActive:{course.IsActive}");
                                }
                                Console.WriteLine("--------------------------------------------------------------------------------");
                            }
                        }
                        Console.Write("Press any key to return to menu.");
                        Console.ReadKey();
                        goto Menu;
                    }
                #endregion

                #region Case 3: View Masters
                case 3:

                    PrintDatetimeAndUserStatistic();
                    using (DbProjectContext dbViewMaster = new DbProjectContext("ProjectConnStr"))
                    {
                        Console.WriteLine("\nMaster:\n");
                        List<Master> viewMaster = dbViewMaster.Masters.ToList();

                        foreach (Master master in viewMaster)
                        {
                            Console.WriteLine($"Id:{master.Id}\tName:{master.Name}\tLastName:{master.LastName}\tPhonenumber:{master.Phonenumber}\n" +
                                              $"FieldOfStudy:{master.FieldOfStudy}\tSalary:{master.Salary}\tRegisterdate:{master.Registerdate}\tActive:{master.IsActive}");
                            List<Course> courseMaster = dbViewMaster.Courses.Where(a => a.Master.Id == master.Id).ToList();

                            for (int i = 0; i < courseMaster.Count; i++)
                            {
                                if (i == 0)
                                {
                                    Console.WriteLine("\n\tCourses:");
                                }
                                Console.WriteLine($"\tId:{courseMaster[i].Id}\tName:{courseMaster[i].CourseName}\tUnit:{courseMaster[i].CourseUnit}\tActive:{courseMaster[i].IsActive}");
                            }
                            Console.WriteLine("--------------------------------------------------------------------------------");
                        }
                        Console.Write("Press any key to return to menu.");
                        Console.ReadKey();
                        goto Menu;
                    }
                #endregion

                #region Case 4: View Courses
                case 4:

                    PrintDatetimeAndUserStatistic();
                    using (DbProjectContext dbCourseView = new DbProjectContext("ProjectConnStr"))
                    {
                        Console.WriteLine("\nCourses:\n");
                        List<Course> courseView = dbCourseView.Courses.ToList();
                        foreach (Course course in courseView)
                        {
                            Console.WriteLine($"CourseId:{course.Id}\tCourseName:{course.CourseName}\tCourseUnit:{course.CourseUnit}\tRegisterDate:{course.RegisterDate}\tActive:{course.IsActive}");
                            if (course.Master != null)
                            {
                                Console.WriteLine("\tMaster:");
                                Console.WriteLine($"Id:{course.Master.Id}\tName:{course.Master.Name}\tFamily:{course.Master.LastName}\tActive:{course.Master.IsActive}");
                            }
                            Console.WriteLine("--------------------------------------------------------------------------------");
                        }
                        Console.Write("Press any key to return to menu.");
                        Console.ReadKey();
                        goto Menu;
                    }
                #endregion

                #region Case 5: New Employee Registration Form
                case 5:

                    PrintDatetimeAndUserStatistic();
                    DbProjectContext dbNewEmployee = new DbProjectContext("ProjectConnStr");
                    using (dbNewEmployee)
                    {
                        Console.WriteLine("\n(Employee) Registration form!");
                        Console.Write("\n\tDepartment name  : ");
                        string newEmployeeDeparment = Console.ReadLine();
                        Console.Write("\tMonthly salary   : ");
                        float newEmployeeSalary = Convert.ToInt32(Console.ReadLine());
                        while (!float.TryParse(Console.ReadLine(), out newEmployeeSalary))
                        {
                            PrintJustNumbers("Monthly salary:");
                            Console.Clear();
                            PrintDatetimeAndUserStatistic();
                            Console.WriteLine("\n(Employee) Registration form!");
                            Console.Write($"\n\tDepartment name:{newEmployeeDeparment}");
                            Console.Write("\n\tMonthly salary:");
                        }
                        Console.Write("\tFirst name  : ");
                        string newEmployeeName = Console.ReadLine();
                        Console.Write("\tLast name   : ");
                        string newEmployeeLastName = Console.ReadLine();
                        Console.Write("\tPhonenumber : ");
                        string newEmployeePhone = Console.ReadLine();
                        while (!Regex.IsMatch(newEmployeePhone, phonePattern))
                        {
                            Console.WriteLine("\n\tIncorrect Phonenumber!");
                            Thread.Sleep(3000);
                            Console.Write("\tPhonenumber : ");
                            newEmployeePhone = Console.ReadLine();
                        }
                        Console.Write("Password      : ");
                        string newEmployeePassword = Console.ReadLine();
                        Console.Write("Age           : ");
                        int newEmployeeAge = Convert.ToInt32(Console.ReadLine());
                        dbNewEmployee.Employees.Add(new Employee($"{newEmployeeDeparment}", newEmployeeSalary, $"{newEmployeeName}", $"{newEmployeeLastName}", $"{newEmployeePhone}", $"{newEmployeePassword}", dbNewEmployee.Roles.Find(2), newEmployeeAge));
                        PrintDatetimeAndUserStatistic();
                        Console.WriteLine($"\n\t{newEmployeeName} {newEmployeeLastName} was registered!\n");
                        dbNewEmployee.SaveChanges();
                        Console.Write("Press any key to return to menu.");
                        Console.ReadKey();
                        goto Menu;
                    }
                #endregion

                #region case 6: New student registration form
                case 6:

                    PrintDatetimeAndUserStatistic();
                    DbProjectContext dbNewStudent = new DbProjectContext("ProjectConnStr");
                    using (dbNewStudent)
                    {
                        Console.WriteLine("\n(Student) Registeration form!");
                        Console.Write("\n\tField of Study  : ");
                        string newStudentFieldOfStudy = Console.ReadLine();
                        Console.Write("\tFirst name      : ");
                        string newStudentName = Console.ReadLine();
                        Console.Write("\tLast name       : ");
                        string newStudentLastName = Console.ReadLine();
                        Console.Write("\tPhonenumber   : ");
                        string newStudentPhone = Console.ReadLine();
                        while (!Regex.IsMatch(newStudentPhone, phonePattern))
                        {
                            Console.WriteLine("\n\tIncorrect Phonenumber!");
                            Thread.Sleep(3000);
                            Console.Write("\tPhonenumber : ");
                            newStudentPhone = Console.ReadLine();
                        }
                        Console.Write("\tPassword      : ");
                        string newStudentPassword = Console.ReadLine();
                        Console.Write("\tAge           : ");
                        int newStudentAge = Convert.ToInt32(Console.ReadLine());
                        dbNewStudent.Students.Add(new Student($"{newStudentFieldOfStudy}", $"{newStudentName}", $"{newStudentLastName}", $"{newStudentPhone}", $"{newStudentPassword}", dbNewStudent.Roles.Find(4), newStudentAge));
                        PrintDatetimeAndUserStatistic();
                        Console.WriteLine($"\n\t{newStudentName} {newStudentLastName} was registered!\n");
                        dbNewStudent.SaveChanges();
                        Console.Write("Press any key to return to menu.");
                        Console.ReadKey();
                        goto Menu;
                    }
                #endregion

                #region case 7: New Master registration form
                case 7:

                    PrintDatetimeAndUserStatistic();
                    DbProjectContext dbNewMaster = new DbProjectContext("ProjectConnStr");
                    using (dbNewMaster)
                    {
                        Console.WriteLine("\n(Master) Registeration form!");
                        Console.Write("\n\tFieldOfStudy: ");
                        string newMasterFieldOfStudy = Console.ReadLine();
                        Console.Write("\tMonthly Salary: ");
                        float newMasterSalary;
                        while (!float.TryParse(Console.ReadLine(), out newMasterSalary))
                        {
                            PrintJustNumbers("Monthly Salary");
                            Console.Clear();
                            PrintDatetimeAndUserStatistic();
                            Console.WriteLine("\n(Master) Registeration form!");
                            Console.Write($"\n\tFieldOfStudy: {newMasterFieldOfStudy}");
                            Console.Write("\tMonthly Salary: ");
                        }
                        Console.Write("\tFirst name : ");
                        string newMasterName = Console.ReadLine();
                        Console.Write("\tLast Name  : ");
                        string newMasterLastName = Console.ReadLine();
                        Console.Write("\tPhonenumber: ");
                        string newMasterPhone = Console.ReadLine();
                        while (!Regex.IsMatch(newMasterPhone, phonePattern))
                        {
                            Console.WriteLine("\n\tIncorrect Phonenumber!");
                            Thread.Sleep(3000);
                            Console.Write("\tPhonenumber : ");
                            newMasterPhone = Console.ReadLine();
                        }
                        Console.Write("\tPassword   : ");
                        string newMasterPass = Console.ReadLine();
                        Console.Write("\tAge        : ");
                        int newMasterAge = Convert.ToInt32(Console.ReadLine());
                        dbNewMaster.Masters.Add(new Master($"{newMasterFieldOfStudy}", newMasterSalary, $"{newMasterName}", $"{newMasterLastName}", $"{newMasterPhone}", $"{newMasterPass}", dbNewMaster.Roles.Find(3), newMasterAge));
                        PrintDatetimeAndUserStatistic();
                        Console.WriteLine($"\n\t{newMasterName} {newMasterLastName} was registered!\n");
                        dbNewMaster.SaveChanges();
                        Console.Write("Press any key to return to menu.");
                        Console.ReadKey();
                        goto Menu;
                    }
                #endregion

                #region case 8: New Course registretion form
                case 8:

                    PrintDatetimeAndUserStatistic();
                    DbProjectContext dbNewCourse = new DbProjectContext("ProjectConnStr");
                    using (dbNewCourse)
                    {
                        Console.WriteLine("\n(Course) Registration form!");
                        Console.Write("\n\tCourse name  : ");
                        string newCourseName = Console.ReadLine();
                        Console.Write("\tCourse unit    : ");
                        int newCourseUnit;
                        while (!int.TryParse(Console.ReadLine(), out newCourseUnit))
                        {
                            PrintJustNumbers("Course unit    : ");
                            Console.Clear();
                            PrintDatetimeAndUserStatistic();
                            Console.WriteLine("\n(Course) Registration form!");
                            Console.Write($"\n\tCourse name  : {newCourseUnit}");
                            Console.Write("\n\tCourse unit: ");
                        }
                    AnswerChooseMaster:
                        PrintDatetimeAndUserStatistic();
                        Console.WriteLine($"\nDose {newCourseName} have a new master?");
                        Console.WriteLine($"\n\t\t 1. Yes\n\t\t2. No");
                        Console.Write("Pick any to continue: ");
                        int answerChooseMaster;
                        while (!int.TryParse(Console.ReadLine(), out answerChooseMaster))
                        {
                            PrintJustNumbers("Choose Master!");
                            Console.Clear();
                            PrintDatetimeAndUserStatistic();
                            Console.WriteLine($"\nDose {newCourseName} have a master?");
                            Console.WriteLine($"\n\t\t 1. Yes\n\t\t2. No");
                            Console.Write("Pick any to continue: ");
                        }
                        #region Yes or No option for choosing master of the new course
                        switch (answerChooseMaster)
                        {
                            #region Yes option
                            case 1:
                                PrintDatetimeAndUserStatistic();
                                Console.WriteLine($"\nNew master registration form for {newCourseName} course.");
                                Console.Write("\n\tFieldOfStudy: ");
                                string masterFieldOfStudyC = Console.ReadLine();
                                float masterSalaryC;
                                while (!float.TryParse(Console.ReadLine(), out masterSalaryC))
                                {
                                    PrintDatetimeAndUserStatistic();
                                    PrintJustNumbers("Monthly Salary");
                                    Thread.Sleep(3000);
                                    Console.Clear();
                                    PrintDatetimeAndUserStatistic();
                                    Console.WriteLine($"\nNew master registration form for {newCourseName} course.");
                                    Console.Write($"\n\n\tFieldOfStudy: {masterFieldOfStudyC}");
                                    Console.Write("\tMonthly Salary: ");
                                }
                                Console.Write("\tFirst name : ");
                                string masterNameC = Console.ReadLine();
                                Console.Write("\tLast Name  : ");
                                string masterLastNameC = Console.ReadLine();
                                Console.Write("\tPhonenumber: ");
                                string masterPhoneC = Console.ReadLine();
                                while (!Regex.IsMatch(masterPhoneC, phonePattern))
                                {
                                    Console.WriteLine("\n\tIncorrect Phonenumber!");
                                    Thread.Sleep(3000);
                                    Console.Clear();
                                    PrintDatetimeAndUserStatistic();
                                    Console.WriteLine($"\nNew master registration form for {newCourseName} course.");
                                    Console.Write($"\n\n\tFieldOfStudy: {masterFieldOfStudyC}");
                                    Console.Write($"\n\tMonthly Salary: {masterSalaryC}");
                                    Console.Write($"\n\tFirst name    : {masterNameC}");
                                    Console.Write($"\n\tLast Name     : {masterLastNameC}");
                                    Console.Write("\tPhonenumber: ");
                                    masterPhoneC = Console.ReadLine();
                                }
                                Console.Write("\tPassword       : ");
                                string masterPassC = Console.ReadLine();
                                Console.Write("\tAge            : ");
                                int masterAgeC = Convert.ToInt32(Console.ReadLine());
                                Master chooseMaster = new Master($"{masterFieldOfStudyC}", masterSalaryC, $"{masterNameC}", $"{masterLastNameC}", $"{masterPhoneC}", $"{masterPassC}", dbNewCourse.Roles.Find(3), masterAgeC);
                                dbNewCourse.Courses.Add(new Course($"{newCourseName}", newCourseUnit, chooseMaster));
                                PrintDatetimeAndUserStatistic();
                                Console.WriteLine($"\n\t{newCourseName} with this({masterNameC} {masterLastNameC})master was registered!");
                                goto FinishNewCourse;
                            #endregion

                            #region No option
                            case 2:
                            chooseMasterIdForCourse:
                                PrintDatetimeAndUserStatistic();
                                Console.WriteLine($"\nPlease choose a master for {newCourseName}\n");
                                List<Master> masterList = dbNewCourse.Masters.ToList();
                                foreach (Master master in masterList)
                                {
                                    Console.WriteLine($"Id:{master.Id}\tName:{master.Name}\tLastName:{master.LastName}\tPhonenumber:{master.Phonenumber}\n" +
                                            $"FieldOfStudy:{master.FieldOfStudy}\tSalary:{master.Salary}\tRegisterdate:{master.Registerdate}\tActive:{master.IsActive}");
                                    Console.WriteLine("\t--------------------------------------------------------------------------------");
                                }
                                Console.Write("\nChoose the master: ");
                                int chooseMasterId;
                                while (!int.TryParse(Console.ReadLine(), out chooseMasterId))
                                {
                                    PrintJustNumbers("Choose the master:");
                                    Console.Clear();
                                    PrintDatetimeAndUserStatistic();
                                    Console.WriteLine($"\nPlease choose a master for {newCourseName}\n");
                                    foreach (Master master in masterList)
                                    {
                                        Console.WriteLine($"Id:{master.Id}\tName:{master.Name}\tLastName:{master.LastName}\tPhonenumber:{master.Phonenumber}\n" +
                                                $"FieldOfStudy:{master.FieldOfStudy}\tSalary:{master.Salary}\tRegisterdate:{master.Registerdate}\tActive:{master.IsActive}");
                                        Console.WriteLine("\t--------------------------------------------------------------------------------");
                                    }
                                    Console.Write("\nChoose the master: ");
                                }
                                var selectMaster = dbNewCourse.Masters.Find(chooseMasterId);
                                if (selectMaster != null)
                                {
                                    dbNewCourse.Courses.Add(new Course($"{newCourseName}", newCourseUnit, selectMaster));
                                    PrintDatetimeAndUserStatistic();
                                    Console.WriteLine($"\n\t{newCourseName} was registered\n");
                                }
                                else
                                {
                                    PrintDatetimeAndUserStatistic();
                                    IdNotFound("Master list", chooseMasterId);
                                    goto chooseMasterIdForCourse;
                                }
                                goto FinishNewCourse;

                            default:
                                goto AnswerChooseMaster;
                                #endregion
                        }
                    FinishNewCourse:
                        dbNewCourse.SaveChanges();
                        Console.Write("Press any key to return to menu.");
                        Console.ReadKey();
                        goto Menu;

                        #endregion
                    }
                #endregion

                #region Case 9: Employee info editing form
                case 9:
                EmployeeListForEdit:
                    PrintDatetimeAndUserStatistic();
                    Console.WriteLine("\nPlease select an employee to edit.\n");
                    DbProjectContext dbEditEmployee = new DbProjectContext("ProjectConnStr");
                    using (dbEditEmployee)
                    {
                        List<Employee> viewEmployeeList = dbEditEmployee.Employees.ToList();
                        foreach (Employee employee in viewEmployeeList)
                        {
                            Console.WriteLine($"Id:{employee.Id}\tName:{employee.Name}\tLast Name:{employee.LastName}\tDepartment:{employee.DepartmentName}\n" +
                                $"Phonenumber:{employee.Phonenumber}\tSalary:{employee.Salary}\tRegisterdate:{employee.Registerdate}\tActive:{employee.IsActive}");

                            Console.WriteLine("--------------------------------------------------------------------------------");
                        }
                        Console.Write("\nEmployee id: ");
                        int employeeIdForEdit;
                        while (!int.TryParse(Console.ReadLine(), out employeeIdForEdit))
                        {
                            PrintJustNumbers("Employee id!");
                            Console.Clear();
                            PrintDatetimeAndUserStatistic();
                            foreach (Employee employee in viewEmployeeList)
                            {
                                Console.WriteLine($"Id:{employee.Id}\tName:{employee.Name}\tLast Name:{employee.LastName}\tDepartment:{employee.DepartmentName}\n" +
                                    $"Phonenumber:{employee.Phonenumber}\tSalary:{employee.Salary}\tRegisterdate:{employee.Registerdate}\tActive:{employee.IsActive}");

                                Console.WriteLine("--------------------------------------------------------------------------------");
                            }
                            Console.Write("\nEmployee id: ");
                        }
                        PrintDatetimeAndUserStatistic();
                        var chooseEmployeeToEdit = dbEditEmployee.Employees.Find(employeeIdForEdit);
                        if (chooseEmployeeToEdit != null)
                        {
                            goto StartEditingEmployee;
                        }
                        else
                        {
                            PrintDatetimeAndUserStatistic();
                            IdNotFound("Employee List.", employeeIdForEdit);
                            goto EmployeeListForEdit;
                        }
                    StartEditingEmployee://///////////////////////////////////////
                        Console.WriteLine($"\nChoose your desired property for editing:{chooseEmployeeToEdit.Name} {chooseEmployeeToEdit.LastName}");
                        Console.WriteLine("\n\t\t1. Name" +
                                          "\n\t\t2. Last name" +
                                          "\n\t\t3. Department" +
                                          "\n\t\t4. Phonenumber" +
                                          "\n\t\t5. Salary" +
                                          "\n\t\t6. Password" +
                                          "\n\t\t7. Active");
                        Console.Write("\nChoose a number to continue.\n\tYour answer:");
                        int answerEditEmployeeProperty;
                        while (!int.TryParse(Console.ReadLine(), out answerEditEmployeeProperty))
                        {
                            PrintJustNumbers("Select the items in the menu.");
                            PrintDatetimeAndUserStatistic();
                            Console.WriteLine($"\nChoose your desired property for editing:{chooseEmployeeToEdit.Name} {chooseEmployeeToEdit.LastName}");
                            Console.WriteLine("\n\t\t1. Name" +
                                              "\n\t\t2. Last name" +
                                              "\n\t\t3. Department" +
                                              "\n\t\t4. Phonenumber" +
                                              "\n\t\t5. Salary" +
                                              "\n\t\t6. Password" +
                                              "\n\t\t7. Active");
                            Console.Write("\nChoose a number to continue.\n\tYour answer:");
                        }
                        PrintDatetimeAndUserStatistic();

                        switch (answerEditEmployeeProperty)
                        {
                            #region Case1: Edit First Name
                            case 1:

                                Console.WriteLine($"\n(Employee Edit!)\n\tEdit form for: {chooseEmployeeToEdit.Name}{chooseEmployeeToEdit.LastName}");
                                Console.WriteLine($"\n\tName: {chooseEmployeeToEdit.Name}");
                                Console.Write("\n\tNew first name: ");
                                string newFirstNameForEmployee = Console.ReadLine();
                                string firstName = chooseEmployeeToEdit.Name;
                                chooseEmployeeToEdit.Name = newFirstNameForEmployee;
                                PrintDatetimeAndUserStatistic();
                                Console.WriteLine($"\n\tFirst name changed to:{newFirstNameForEmployee} for({firstName} {chooseEmployeeToEdit.LastName})\n");
                                goto FinishEditEmployee;

                            #endregion

                            #region Case2: Edit Last Name
                            case 2:

                                Console.WriteLine($"\n(Employee Edit!)\n\tEdit form for: {chooseEmployeeToEdit.Name}{chooseEmployeeToEdit.LastName}");
                                Console.WriteLine($"\n\tLast name: {chooseEmployeeToEdit.LastName}");
                                Console.Write("\n\tNew last name: ");
                                string newLastNameForEmployee = Console.ReadLine();
                                string lastName = chooseEmployeeToEdit.LastName;
                                chooseEmployeeToEdit.LastName = newLastNameForEmployee;
                                PrintDatetimeAndUserStatistic();
                                Console.WriteLine($"\n\tLast name changed to:{newLastNameForEmployee} for({chooseEmployeeToEdit.Name} {lastName})\n");
                                goto FinishEditEmployee;

                            #endregion

                            #region Case3: Edit Department
                            case 3:

                                Console.WriteLine($"\n(Employee Edit!)\n\tEdit form for: {chooseEmployeeToEdit.Name}{chooseEmployeeToEdit.LastName}");
                                Console.WriteLine($"\n\tDepartment name: {chooseEmployeeToEdit.DepartmentName}");
                                Console.Write("\n\tNew Department: ");
                                string newDeprtmentForEmployee = Console.ReadLine();
                                chooseEmployeeToEdit.DepartmentName = newDeprtmentForEmployee;
                                PrintDatetimeAndUserStatistic();
                                Console.WriteLine($"\n\tDepartment changed to:{newDeprtmentForEmployee} for({chooseEmployeeToEdit.Name} {chooseEmployeeToEdit.LastName})\n");
                                goto FinishEditEmployee;
                            #endregion

                            #region Case4: Edit Phonenumber
                            case 4:

                                Console.WriteLine($"\n(Employee Edit!)\n\tEdit form for: {chooseEmployeeToEdit.Name}{chooseEmployeeToEdit.LastName}");
                                Console.WriteLine($"\n\tPhonenumber: {chooseEmployeeToEdit.Phonenumber}");
                                Console.Write("\n\tNew phonenumber: ");
                                string newPhoneForEmployee = Console.ReadLine();
                                while (!Regex.IsMatch(newPhoneForEmployee, phonePattern))
                                {
                                    Console.WriteLine("\n\tIncorrect Phonenumber!");
                                    Thread.Sleep(3000);
                                    Console.Write("\n\tNew phonenumber: ");
                                    newPhoneForEmployee = Console.ReadLine();
                                }
                                chooseEmployeeToEdit.Phonenumber = newPhoneForEmployee;
                                PrintDatetimeAndUserStatistic();
                                Console.WriteLine($"\n\tPhonenumber changed to:{newPhoneForEmployee} for({chooseEmployeeToEdit.Name} {chooseEmployeeToEdit.LastName})\n");
                                goto FinishEditEmployee;

                            #endregion

                            #region Case5: Edit Salary
                            case 5:

                                Console.WriteLine($"\n(Employee Edit!)\n\tEdit form for: {chooseEmployeeToEdit.Name}{chooseEmployeeToEdit.LastName}");
                                Console.WriteLine($"\n\tSalary: {chooseEmployeeToEdit.Salary}");
                                Console.Write("\n\tNew Salary: ");
                                float newSalaryForEmployee;
                                while (!float.TryParse(Console.ReadLine(), out newSalaryForEmployee))
                                {
                                    PrintJustNumbers("Monthly Salary!");
                                    PrintDatetimeAndUserStatistic();
                                    Console.WriteLine($"\n(Employee Edit!)\n\tEdit form for: {chooseEmployeeToEdit.Name}{chooseEmployeeToEdit.LastName}");
                                    Console.WriteLine($"\n\tSalary: {chooseEmployeeToEdit.Salary}");
                                    Console.Write("\n\tNew Salary: ");
                                }
                                chooseEmployeeToEdit.Salary = newSalaryForEmployee;
                                PrintDatetimeAndUserStatistic();
                                Console.WriteLine($"\n\tSalary changed to:{newSalaryForEmployee} for({chooseEmployeeToEdit.Name} {chooseEmployeeToEdit.LastName})\n");
                                goto FinishEditEmployee;
                            #endregion

                            #region Case6: Edit Password
                            case 6:

                                Console.WriteLine($"\n(Employee Edit!)\n\tEdit form for: {chooseEmployeeToEdit.Name}{chooseEmployeeToEdit.LastName}");
                                Console.WriteLine($"\n\tPassword: {chooseEmployeeToEdit.Password}");
                                Console.Write("\n\tNew password: ");
                                string newPasswordForEmployee = Console.ReadLine();
                                chooseEmployeeToEdit.Password = newPasswordForEmployee;
                                PrintDatetimeAndUserStatistic();
                                Console.WriteLine($"\n\tPassword changed to:{newPasswordForEmployee} for({chooseEmployeeToEdit.Name} {chooseEmployeeToEdit.LastName})\n");
                                goto FinishEditEmployee;

                            #endregion

                            #region Case7: Edit IsActive
                            case 7:

                                if (chooseEmployeeToEdit.IsActive == true)
                                {
                                    chooseEmployeeToEdit.IsActive = false;
                                }
                                else
                                {
                                    chooseEmployeeToEdit.IsActive = true;
                                }
                                PrintDatetimeAndUserStatistic();
                                Console.WriteLine($"\n\tAccess changed to:{chooseEmployeeToEdit.IsActive} for({chooseEmployeeToEdit.Name} {chooseEmployeeToEdit.LastName})\n");
                                goto FinishEditEmployee;

                            #endregion

                            default:
                                goto StartEditingEmployee;
                        }
                    FinishEditEmployee:
                        dbEditEmployee.SaveChanges();
                        Console.Write("Press any key to return to menu.");
                        Console.ReadKey();
                        goto Menu;
                    }
                #endregion

                #region Case 10: Student info editing form

                case 10:
                    PrintDatetimeAndUserStatistic();
                    Console.WriteLine("\nPlease select an student to edit.\n");
                    DbProjectContext dbEditStudent = new DbProjectContext("ProjectConnStr");
                    using (dbEditStudent)
                    {
                        List<Student> viewStudentList = dbEditStudent.Students.ToList();
                        foreach (Student student in viewStudentList)
                        {
                            Console.WriteLine($"Id:{student.Id}" +
                                              $"\tStudentCode:{student.StudentCode}" +
                                              $"\tName:{student.Name}" +
                                              $"\tLastName:{student.LastName}" +
                                              $"\tPhonenumber:{student.Phonenumber}" +
                                              $"\tBirthdate:{student.Birthdate}\n" +
                                              $"FieldOfStudy:{student.FieldOfStudy}" +
                                              $"\tTuitionOfEachTerm:{student.TuitionOfEachTerm}" +
                                              $"\tRegister Date:{student.Registerdate}" +
                                              $"\tActive:{student.IsActive}");
                            Console.WriteLine("--------------------------------------------------------------------------------");
                        }
                        Console.Write("\nStudent id: ");
                        int studentIdForEdit;
                        while (!int.TryParse(Console.ReadLine(), out studentIdForEdit))
                        {
                            PrintJustNumbers("Student id!");
                            Console.Clear();
                            PrintDatetimeAndUserStatistic();
                            foreach (Student student in viewStudentList)
                            {
                                Console.WriteLine($"Id:{student.Id}" +
                                                  $"\tStudentCode:{student.StudentCode}" +
                                                  $"\tName:{student.Name}" +
                                                  $"\tLastName:{student.LastName}" +
                                                  $"\tPhonenumber:{student.Phonenumber}" +
                                                  $"\tBirthdate:{student.Birthdate}\n" +
                                                  $"FieldOfStudy:{student.FieldOfStudy}" +
                                                  $"\tTuitionOfEachTerm:{student.TuitionOfEachTerm}" +
                                                  $"\tRegister Date:{student.Registerdate}" +
                                                  $"\tActive:{student.IsActive}");
                                Console.WriteLine("--------------------------------------------------------------------------------");
                            }
                            Console.Write("\nStudent id: ");
                        }
                        PrintDatetimeAndUserStatistic();
                        var chooseStudentToEdit = dbEditStudent.Students.Find(studentIdForEdit);
                        if (chooseStudentToEdit != null)
                        {
                            goto StartEditingStudent;
                        }
                        else
                        {
                            PrintDatetimeAndUserStatistic();
                            IdNotFound("Student List.", studentIdForEdit);
                            goto EmployeeListForEdit;
                        }
                    StartEditingStudent:
                        Console.WriteLine($"\nChoose your desired property for editing:{chooseStudentToEdit.Name} {chooseStudentToEdit.LastName}");
                        Console.WriteLine("\n\t\t1. Name" +
                                          "\n\t\t2. Last name" +
                                          "\n\t\t3. Department" +
                                          "\n\t\t4. Phonenumber" +
                                          "\n\t\t5. Salary" +
                                          "\n\t\t6. Password" +
                                          "\n\t\t7. Active");
                        Console.Write("\nChoose a number to continue.\n\tYour answer:");
                        int answerEditStudentProperty;
                        while (!int.TryParse(Console.ReadLine(), out answerEditStudentProperty))
                        {
                            PrintJustNumbers("Select the items in the menu.");
                            PrintDatetimeAndUserStatistic();
                            Console.WriteLine($"\nChoose your desired property for editing:{chooseStudentToEdit.Name} {chooseStudentToEdit.LastName}");
                            Console.WriteLine("\n\t\t1. Name" +
                                              "\n\t\t2. Last name" +
                                              "\n\t\t3. Phonenumber" +
                                              "\n\t\t4. Password" +
                                              "\n\t\t5. Active" +
                                              "\n\t\t6. Add Course");
                            Console.Write("\nChoose a number to continue.\n\tYour answer:");
                        }
                        PrintDatetimeAndUserStatistic();

                        switch (answerEditStudentProperty)
                        {

                            #region Case1: Edit First Name
                            case 1:

                                Console.WriteLine($"\n(Student Edit!)\n\tEdit form for: {chooseStudentToEdit.Name}{chooseStudentToEdit.LastName}");
                                Console.WriteLine($"\n\tName: {chooseStudentToEdit.Name}");
                                Console.Write("\n\tNew first name: ");
                                string newFirstNameForStudent = Console.ReadLine();
                                string firstNameStu = chooseStudentToEdit.Name;
                                chooseStudentToEdit.Name = newFirstNameForStudent;
                                PrintDatetimeAndUserStatistic();
                                Console.WriteLine($"\n\tFirst name changed to:{newFirstNameForStudent} for({firstNameStu} {chooseStudentToEdit.LastName})\n");
                                goto FinishEditStudent;

                            #endregion

                            #region Case2: Edit LastName
                            case 2:

                                Console.WriteLine($"\n(Student Edit!)\n\tEdit form for: {chooseStudentToEdit.Name}{chooseStudentToEdit.LastName}");
                                Console.WriteLine($"\n\tLast name: {chooseStudentToEdit.LastName}");
                                Console.Write("\n\tNew last name: ");
                                string newLastNameForStudent = Console.ReadLine();
                                string lastNameStu = chooseStudentToEdit.Name;
                                chooseStudentToEdit.LastName = newLastNameForStudent;
                                PrintDatetimeAndUserStatistic();
                                Console.WriteLine($"\n\tLast name changed to:{newLastNameForStudent} for({chooseStudentToEdit.Name} {lastNameStu})\n");
                                goto FinishEditStudent;

                            #endregion

                            #region Case3: Edit Phonenumber
                            case 3:

                                Console.WriteLine($"\n(Student Edit!)\n\tEdit form for: {chooseStudentToEdit.Name}{chooseStudentToEdit.LastName}");
                                Console.WriteLine($"\n\tPhonenumber: {chooseStudentToEdit.Phonenumber}");
                                Console.Write("\n\tNew phonenumber: ");
                                string newPhoneForStudent = Console.ReadLine();
                                while (!Regex.IsMatch(newPhoneForStudent, phonePattern))
                                {
                                    Console.WriteLine("\n\tIncorrect Phonenumber!");
                                    Thread.Sleep(3000);
                                    Console.Write("\n\tNew phonenumber: ");
                                    newPhoneForStudent = Console.ReadLine();
                                }
                                chooseStudentToEdit.Phonenumber = newPhoneForStudent;
                                PrintDatetimeAndUserStatistic();
                                Console.WriteLine($"\n\tPhonenumber changed to:{newPhoneForStudent} for({chooseStudentToEdit.Name} {chooseStudentToEdit.LastName})\n");
                                goto FinishEditStudent;

                            #endregion

                            #region Case4: Edit Password
                            case 4:

                                Console.WriteLine($"\n(Student Edit!)\n\tEdit form for: {chooseStudentToEdit.Name}{chooseStudentToEdit.LastName}");
                                Console.WriteLine($"\n\tPassword: {chooseStudentToEdit.Password}");
                                Console.Write("\n\tNew password: ");
                                string newPasswordForStudent = Console.ReadLine();
                                chooseStudentToEdit.Password = newPasswordForStudent;
                                PrintDatetimeAndUserStatistic();
                                Console.WriteLine($"\n\tPassword changed to:{newPasswordForStudent} for({chooseStudentToEdit.Name} {chooseStudentToEdit.LastName})\n");
                                goto FinishEditStudent;

                            #endregion

                            #region Case5: Edit IsActive
                            case 5:

                                if (chooseStudentToEdit.IsActive == true)
                                {
                                    chooseStudentToEdit.IsActive = false;
                                }
                                else
                                {
                                    chooseStudentToEdit.IsActive = true;
                                }
                                PrintDatetimeAndUserStatistic();
                                Console.WriteLine($"\n\tAccess changed to:{chooseStudentToEdit.IsActive} for({chooseStudentToEdit.Name} {chooseStudentToEdit.LastName})\n");
                                goto FinishEditStudent;

                            #endregion

                            #region Case6: Add Course
                            case 6:
                                PrintDatetimeAndUserStatistic();
                            AddCourseForStudent:
                                Console.WriteLine($"\n(Student Edit!)\n\tEdit form for: {chooseStudentToEdit.Name}{chooseStudentToEdit.LastName}");
                                Console.WriteLine("\n\tAre you adding a new course?");
                                Console.WriteLine("\n\t\t1. Select old course" +
                                                  "\n\t\t2. Create new course");
                                Console.WriteLine("\nYour answer:");
                                int answerAddCourseForStudent;
                                while (!int.TryParse(Console.ReadLine(), out answerAddCourseForStudent))
                                {
                                    PrintJustNumbers("Select the items in the menu");
                                    PrintDatetimeAndUserStatistic();
                                    Console.WriteLine($"\n(Student Edit!)\n\tEdit form for: {chooseStudentToEdit.Name}{chooseStudentToEdit.LastName}");
                                    Console.WriteLine("\n\tAre you adding a new course?");
                                    Console.WriteLine("\n\t\t1. Select old course" +
                                                      "\n\t\t2. Create new course");
                                    Console.WriteLine("\nYour answer:");
                                }
                                PrintDatetimeAndUserStatistic();
                                switch (answerAddCourseForStudent)
                                {
                                    case 1:
                                    AddCourseOfList:
                                        Console.WriteLine($"\nPlease select a course for adding to :({chooseStudentToEdit.Name} {chooseStudentToEdit.LastName})\n");
                                        List<Course> courseList = dbEditStudent.Courses.ToList();
                                        foreach (Course course in courseList)
                                        {
                                            Console.WriteLine($"CourseId:{course.Id}\tCourseName:{course.CourseName}\tCourseUnit:{course.CourseUnit}\tRegisterDate:{course.RegisterDate}\tActive:{course.IsActive}");
                                            Console.WriteLine("--------------------------------------------------------------------------------");
                                        }
                                        Console.Write("Course id: ");
                                        int addCourseForStudentInList;
                                        while (!int.TryParse(Console.ReadLine(), out addCourseForStudentInList))
                                        {
                                            PrintJustNumbers("Course id!");
                                            PrintDatetimeAndUserStatistic();
                                            Console.WriteLine($"\nPlease select a course for adding to :({chooseStudentToEdit.Name} {chooseStudentToEdit.LastName})\n");
                                            foreach (Course course in courseList)
                                            {
                                                Console.WriteLine($"CourseId:{course.Id}\tCourseName:{course.CourseName}\tCourseUnit:{course.CourseUnit}\tRegisterDate:{course.RegisterDate}\tActive:{course.IsActive}");
                                                Console.WriteLine("--------------------------------------------------------------------------------");
                                            }
                                            Console.Write("Course id: ");
                                        }
                                        Course oldCourse = dbEditStudent.Courses.Find(addCourseForStudentInList);
                                        if (oldCourse != null)
                                        {
                                            chooseStudentToEdit.Courses.Add(oldCourse);
                                        }
                                        else
                                        {
                                            PrintDatetimeAndUserStatistic();
                                            IdNotFound("Course List.", addCourseForStudentInList);
                                            Console.Clear();
                                            goto AddCourseOfList;
                                        }
                                        PrintDatetimeAndUserStatistic();
                                        Console.WriteLine($"\n\t({oldCourse.CourseName}), Course added to:({chooseStudentToEdit.Name} {chooseStudentToEdit.LastName})");
                                        goto FinishEditStudent;

                                    case 2:

                                        Console.WriteLine($"\n New course registeration for :({chooseStudentToEdit.Name} {chooseStudentToEdit.LastName})");
                                        Console.Write("\n\tCourse name: ");
                                        string courseAddName = Console.ReadLine();
                                        Console.Write("\t Course unit : ");
                                        int courseAddunit;
                                        while (!int.TryParse(Console.ReadLine(), out courseAddunit))
                                        {
                                            PrintJustNumbers("Course unit!");
                                            PrintDatetimeAndUserStatistic();
                                            Console.WriteLine($"\n New course registeration for :({chooseStudentToEdit.Name} {chooseStudentToEdit.LastName})");
                                            Console.Write($"\n\tCourse name: {courseAddName}");
                                            Console.Write("\t Course unit : ");
                                        }
                                    AddNewCourseForStu:
                                        PrintDatetimeAndUserStatistic();
                                        Console.WriteLine($"\nDoes {courseAddName} have new master?");
                                        Console.WriteLine("\n\t\t1.Yes \n\t\t2.No ");
                                        Console.Write("Your answer would be: ");
                                        int addNewCourseEditingStu;
                                        while (!int.TryParse(Console.ReadLine(), out addNewCourseEditingStu))
                                        {
                                            PrintJustNumbers("Select the items in the menu.");
                                            Console.Clear();
                                            PrintDatetimeAndUserStatistic();
                                            Console.WriteLine($"\nDoes {courseAddName} have new master?");
                                            Console.WriteLine("\n\t\t1.Yes \n\t\t2.No ");
                                            Console.Write("Your answer would be: ");
                                        }
                                        PrintDatetimeAndUserStatistic();
                                        switch (addNewCourseEditingStu)
                                        {
                                            case 1:
                                                Console.WriteLine("\n(Master) Registeration form!");
                                                Console.Write("\n\tFieldOfStudy: ");
                                                string newMasterFieldOfStudyAddCourse = Console.ReadLine();
                                                Console.Write("\tMonthly Salary: ");
                                                float newMasterSalaryAddCourse;
                                                while (!float.TryParse(Console.ReadLine(), out newMasterSalaryAddCourse))
                                                {
                                                    PrintJustNumbers("Monthly Salary");
                                                    Console.Clear();
                                                    PrintDatetimeAndUserStatistic();
                                                    Console.WriteLine("\n(Master) Registeration form!");
                                                    Console.Write($"\n\tFieldOfStudy: {newMasterFieldOfStudyAddCourse}");
                                                    Console.Write("\tMonthly Salary: ");
                                                }
                                                Console.Write("\tFirst name : ");
                                                string newMasterNameAddCourse = Console.ReadLine();
                                                Console.Write("\tLast Name  : ");
                                                string newMasterLastNameAddCourse = Console.ReadLine();
                                                Console.Write("\tPhonenumber: ");
                                                string newMasterPhoneAddCourse = Console.ReadLine();
                                                while (!Regex.IsMatch(newMasterPhoneAddCourse, phonePattern))
                                                {
                                                    Console.WriteLine("\n\tIncorrect Phonenumber!");
                                                    Thread.Sleep(3000);
                                                    PrintDatetimeAndUserStatistic();
                                                    Console.WriteLine("\n(Master) Registeration form for{0}", courseAddName);
                                                    Console.Write($"\n\tField of study: {newMasterFieldOfStudyAddCourse}");
                                                    Console.Write($"\n\tMonthly salary: {newMasterSalaryAddCourse}");
                                                    Console.Write($"\n\tFirst name: {newMasterNameAddCourse}");
                                                    Console.Write($"\n\tLast name: {newMasterLastNameAddCourse}");
                                                    Console.Write("\tPhonenumber : ");
                                                    newMasterPhoneAddCourse = Console.ReadLine();
                                                }
                                                Console.Write("\tPassword   : ");
                                                string newMasterPassAddCourse = Console.ReadLine();
                                                Console.Write("\tAge        : ");
                                                int newMasterAgeAddCourse = Convert.ToInt32(Console.ReadLine());
                                                Course newAddCourse = new Course($"{courseAddName}", courseAddunit, new Master($"{newMasterFieldOfStudyAddCourse}", newMasterSalaryAddCourse, $"{newMasterNameAddCourse}", $"{newMasterLastNameAddCourse}", $"{newMasterPhoneAddCourse}", $"{newMasterPassAddCourse}", dbEditStudent.Roles.Find(3), newMasterAgeAddCourse));
                                                dbEditStudent.Courses.Add(newAddCourse);
                                                chooseStudentToEdit.Courses.Add(newAddCourse);
                                                PrintDatetimeAndUserStatistic();
                                                Console.WriteLine($"\n\t{newAddCourse.CourseName} added to :{newMasterNameAddCourse} {newMasterLastNameAddCourse}\n");
                                                goto FinishEditStudent;
                                            case 2:
                                            AddCourseIdForStudent:
                                                Console.WriteLine($"\nPlease select the master for this:({courseAddName}) course!");
                                                List<Master> masterList = dbEditStudent.Masters.ToList();
                                                foreach (Master master in masterList)
                                                {
                                                    Console.WriteLine($"Id:{master.Id}" +
                                                                      $"\tName:{master.Name}" +
                                                                      $"\tLastName:{master.LastName}" +
                                                                      $"\tPhonenumber:{master.Phonenumber}\n" +
                                                                      $"FieldOfStudy:{master.FieldOfStudy}" +
                                                                      $"\tSalary:{master.Salary}" +
                                                                      $"\tRegisterdate:{master.Registerdate}" +
                                                                      $"\tActive:{master.IsActive}");
                                                    Console.WriteLine("--------------------------------------------------------------------------------");
                                                }
                                                Console.Write("\nMaster id: ");
                                                int answerSelectNewCourse;
                                                while (!int.TryParse(Console.ReadLine(), out answerSelectNewCourse))
                                                {
                                                    PrintJustNumbers("Master id!");
                                                    Console.Clear();
                                                    PrintDatetimeAndUserStatistic();
                                                    foreach (Master master in masterList)
                                                    {
                                                        Console.WriteLine($"Id:{master.Id}" +
                                                                          $"\tName:{master.Name}" +
                                                                          $"\tLastName:{master.LastName}" +
                                                                          $"\tPhonenumber:{master.Phonenumber}\n" +
                                                                          $"FieldOfStudy:{master.FieldOfStudy}" +
                                                                          $"\tSalary:{master.Salary}" +
                                                                          $"\tRegisterdate:{master.Registerdate}" +
                                                                          $"\tActive:{master.IsActive}");
                                                        Console.WriteLine("--------------------------------------------------------------------------------");
                                                    }
                                                    Console.Write("\nMaster id: ");
                                                }
                                                var selectExistingMaster = dbEditStudent.Masters.Find(answerSelectNewCourse);
                                                if (selectExistingMaster != null)
                                                {
                                                    goto AddNewCourseByMaster;
                                                }
                                                else
                                                {
                                                    PrintDatetimeAndUserStatistic();
                                                    IdNotFound("Master list!", answerSelectNewCourse);
                                                    PrintDatetimeAndUserStatistic();
                                                    goto AddCourseIdForStudent;
                                                }
                                            AddNewCourseByMaster:
                                                Course newCourseByMaster = new Course($"{courseAddName}", courseAddunit, selectExistingMaster);
                                                dbEditStudent.Courses.Add(newCourseByMaster);
                                                chooseStudentToEdit.Courses.Add(newCourseByMaster);
                                                PrintDatetimeAndUserStatistic();
                                                Console.WriteLine($"\n\t{newCourseByMaster.CourseName} added to :{chooseStudentToEdit.Name} {chooseStudentToEdit.LastName}\n");
                                                goto FinishEditStudent;

                                            default:
                                                goto AddNewCourseForStu;
                                        }
                                    default:
                                        goto AddCourseForStudent;
                                }
                            default:
                                goto StartEditingStudent;
                                #endregion
                        }
                    FinishEditStudent:
                        dbEditStudent.SaveChanges();
                        Console.Write("Press any key to return to menu.");
                        Console.ReadKey();
                        goto Menu;
                    }
                #endregion

                #region Case 11: Master info editing form

                case 11:
                    MastersForEdit:
                    PrintDatetimeAndUserStatistic();
                    Console.WriteLine("\nPlease select the desired master for edit.");
                    DbProjectContext dbEditMaster = new DbProjectContext("ProjectConnStr");
                    using (dbEditMaster)
                    {
                        List<Master> viewMasterList = dbEditMaster.Masters.ToList();
                        foreach (Master master in viewMasterList)
                        {
                            Console.WriteLine($"Id:{master.Id}" +
                                              $"\tName:{master.Name}" +
                                              $"\tLastName:{master.LastName}" +
                                              $"\tPhonenumber:{master.Phonenumber}\n" +
                                              $"FieldOfStudy:{master.FieldOfStudy}" +
                                              $"\tSalary:{master.Salary}" +
                                              $"\tRegisterdate:{master.Registerdate}" +
                                              $"\tActive:{master.IsActive}");
                            Console.WriteLine("--------------------------------------------------------------------------------");
                        }
                        Console.Write("\nMaster id: ");
                        int answerSelectMasterEdit;
                        while (!int.TryParse(Console.ReadLine(), out answerSelectMasterEdit))
                        {
                            PrintJustNumbers("Master id!");
                            Console.Clear();
                            PrintDatetimeAndUserStatistic();
                            Console.WriteLine("\nPlease select the desired master for edit.");
                            foreach (Master master in viewMasterList)
                            {
                                Console.WriteLine($"Id:{master.Id}" +
                                                  $"\tName:{master.Name}" +
                                                  $"\tLastName:{master.LastName}" +
                                                  $"\tPhonenumber:{master.Phonenumber}\n" +
                                                  $"FieldOfStudy:{master.FieldOfStudy}" +
                                                  $"\tSalary:{master.Salary}" +
                                                  $"\tRegisterdate:{master.Registerdate}" +
                                                  $"\tActive:{master.IsActive}");
                                Console.WriteLine("--------------------------------------------------------------------------------");
                            }
                            Console.Write("\nMaster id: ");
                        }
                        PrintDatetimeAndUserStatistic();
                        var chooseMasterForEdit = dbEditMaster.Masters.Find(answerSelectMasterEdit);
                        if (chooseMasterForEdit != null)
                        {
                            goto SelectMasterPropertyEdit;
                        }
                        else
                        {
                            PrintDatetimeAndUserStatistic();
                            IdNotFound("Master list!", answerSelectMasterEdit);
                            goto MastersForEdit;
                        }
                    SelectMasterPropertyEdit:
                        Console.WriteLine($"\nChoose your desired property for editing:{chooseMasterForEdit.Name} {chooseMasterForEdit.LastName}");
                        Console.WriteLine("\n\t\t1. Name" +
                                          "\n\t\t2. Last name" +
                                          "\n\t\t3. Phonenumber" +
                                          "\n\t\t4. Salary" +
                                          "\n\t\t5. Active" +
                                          "\n\t\t6. Password");
                        Console.Write("\nChoose a number to continue.\n\tYour answer:");
                        int selectMasterPropertyEdit;
                        while (!int.TryParse(Console.ReadLine(), out selectMasterPropertyEdit))
                        { 
                            PrintJustNumbers("Select the items in the menu.");
                            Thread.Sleep(3000);
                            PrintDatetimeAndUserStatistic();
                            Console.WriteLine($"\nChoose your desired property for editing:{chooseMasterForEdit.Name} {chooseMasterForEdit.LastName}");
                            Console.WriteLine("\n\t\t1. Name" +
                                              "\n\t\t2. Last name" +
                                              "\n\t\t3. Phonenumber" +
                                              "\n\t\t4. Salary" +
                                              "\n\t\t5. Active" +
                                              "\n\t\t6. Password");
                            Console.Write("\nChoose a number to continue.\n\tYour answer:");
                        }
                        PrintDatetimeAndUserStatistic();

                        switch (selectMasterPropertyEdit)
                        {
                            #region Case1: Edit First Name
                            case 1:

                                Console.WriteLine($"\n(Master Edit!)\n\tEdit form for: {chooseMasterForEdit.Name}{chooseMasterForEdit.LastName}");
                                Console.WriteLine($"\n\tName: {chooseMasterForEdit.Name}");
                                Console.Write("\n\tNew first name: ");
                                string newFirstNameForMaster = Console.ReadLine();
                                string firstNameMaster = chooseMasterForEdit.Name;
                                chooseMasterForEdit.Name = newFirstNameForMaster;
                                PrintDatetimeAndUserStatistic();
                                Console.WriteLine($"\n\tFirst name changed to:{newFirstNameForMaster} for({firstNameMaster} {chooseMasterForEdit.LastName})\n");
                                goto FinishEditMaster;

                            #endregion

                            #region Case2: Edit Last Name
                            case 2:

                                Console.WriteLine($"\n(Master Edit!)\n\tEdit form for: {chooseMasterForEdit.Name}{chooseMasterForEdit.LastName}");
                                Console.WriteLine($"\n\tLast name: {chooseMasterForEdit.LastName}");
                                Console.Write("\n\tNew last name: ");
                                string newLastNameForMaster = Console.ReadLine();
                                string lastNameMaster = chooseMasterForEdit.Name;
                                chooseMasterForEdit.LastName = newLastNameForMaster;
                                PrintDatetimeAndUserStatistic();
                                Console.WriteLine($"\n\tLast name changed to:{newLastNameForMaster} for({chooseMasterForEdit.Name} {lastNameMaster})\n");
                                goto FinishEditMaster;

                            #endregion

                            #region Case3: Edit Phonenumber
                            case 3:

                                Console.WriteLine($"\n(Master Edit!)\n\tEdit form for: {chooseMasterForEdit.Name}{chooseMasterForEdit.LastName}");
                                Console.WriteLine($"\n\tPhonenumber: {chooseMasterForEdit.Phonenumber}");
                                Console.Write("\n\tNew phonenumber: ");
                                string newPhoneForMaster = Console.ReadLine();
                                while (!Regex.IsMatch(newPhoneForMaster, phonePattern))
                                {
                                    Console.WriteLine("\n\tIncorrect Phonenumber!");
                                    Thread.Sleep(3000);
                                    Console.Write("\n\tNew phonenumber: ");
                                    newPhoneForMaster = Console.ReadLine();
                                }
                                chooseMasterForEdit.Phonenumber = newPhoneForMaster;
                                PrintDatetimeAndUserStatistic();
                                Console.WriteLine($"\n\tPhonenumber changed to:{newPhoneForMaster} for({chooseMasterForEdit.Name} {chooseMasterForEdit.LastName})\n");
                                goto FinishEditMaster;

                            #endregion

                            #region Case4: Edit Salary
                            case 4:

                                Console.WriteLine($"\n(Master Edit!)\n\tEdit form for: {chooseMasterForEdit.Name}{chooseMasterForEdit.LastName}");
                                Console.WriteLine($"\n\tSalary: {chooseMasterForEdit.Salary}");
                                Console.Write("\n\tNew Salary: ");
                                float newSalaryForMaster;
                                while (!float.TryParse(Console.ReadLine(), out newSalaryForMaster))
                                {
                                    PrintJustNumbers("Monthly Salary!");
                                    PrintDatetimeAndUserStatistic();
                                    Console.WriteLine($"\n(Master Edit!)\n\tEdit form for: {chooseMasterForEdit.Name}{chooseMasterForEdit.LastName}");
                                    Console.WriteLine($"\n\tSalary: {chooseMasterForEdit.Salary}");
                                    Console.Write("\n\tNew Salary: ");
                                }
                                chooseMasterForEdit.Salary = newSalaryForMaster;
                                PrintDatetimeAndUserStatistic();
                                Console.WriteLine($"\n\tSalary changed to:{newSalaryForMaster} for({chooseMasterForEdit.Name} {chooseMasterForEdit.LastName})\n");
                                goto FinishEditMaster;

                            #endregion

                            #region Case5: Edit IsActive
                            case 5:

                                if (chooseMasterForEdit.IsActive == true)
                                {
                                    chooseMasterForEdit.IsActive = false;
                                }
                                else
                                {
                                    chooseMasterForEdit.IsActive = true;
                                }
                                PrintDatetimeAndUserStatistic();
                                Console.WriteLine($"\n\tAccess changed to:{chooseMasterForEdit.IsActive} for({chooseMasterForEdit.Name} {chooseMasterForEdit.LastName})\n");
                                goto FinishEditMaster;

                            #endregion

                            #region Case6: Edit Password
                            case 6:

                                Console.WriteLine($"\n(Master Edit!)\n\tEdit form for: {chooseMasterForEdit.Name}{chooseMasterForEdit.LastName}");
                                Console.WriteLine($"\n\tPassword: {chooseMasterForEdit.Password}");
                                Console.Write("\n\tNew password: ");
                                string newPasswordForMaster = Console.ReadLine();
                                chooseMasterForEdit.Name = newPasswordForMaster;
                                PrintDatetimeAndUserStatistic();
                                Console.WriteLine($"\n\tPassword changed to:{newPasswordForMaster} for({chooseMasterForEdit.Name} {chooseMasterForEdit.LastName})\n");
                                goto FinishEditMaster;

                            #endregion

                            default:
                                goto SelectMasterPropertyEdit;
                        }
                    FinishEditMaster:
                        dbEditMaster.SaveChanges();
                        Console.Write("Press any key to return to menu.");
                        Console.ReadKey();
                        goto Menu;
                    }

                #endregion

                #region Case 12: Course Info Editing Form

                case 12:
                CoursesForEdit:
                    PrintDatetimeAndUserStatistic();
                    Console.WriteLine("\nPlease select the desired master for edit.");
                    DbProjectContext dbEditCourse = new DbProjectContext("ProjectConnStr");
                    using (dbEditCourse)
                    {
                        List<Course> viewCourseList = dbEditCourse.Courses.ToList();
                        foreach (Course course in viewCourseList)
                        {
                            Console.WriteLine($"CourseId:{course.Id}\tCourseName:{course.CourseName}\tCourseUnit:{course.CourseUnit}\tRegisterDate:{course.RegisterDate}\tActive:{course.IsActive}");
                            if (course.Master != null)
                            {
                                Console.WriteLine("\tMaster:");
                                Console.WriteLine($"Id:{course.Master.Id}\tName:{course.Master.Name}\tFamily:{course.Master.LastName}\tActive:{course.Master.IsActive}");
                            }
                            Console.WriteLine("--------------------------------------------------------------------------------");
                        }
                        Console.Write("\nCourse id:");
                        int answerSelectCourseId;
                        while (!int.TryParse(Console.ReadLine(),out answerSelectCourseId))
                        {
                            PrintJustNumbers("Course Id!");
                            PrintDatetimeAndUserStatistic();
                            foreach (Course course in viewCourseList)
                            {
                                Console.WriteLine($"CourseId:{course.Id}" +
                                    $"\tCourseName:{course.CourseName}" +
                                    $"\tCourseUnit:{course.CourseUnit}" +
                                    $"\tRegisterDate:{course.RegisterDate}" +
                                    $"\tActive:{course.IsActive}");
                                if (course.Master != null)
                                {
                                    Console.WriteLine("\tMaster:");
                                    Console.WriteLine($"Id:{course.Master.Id}\tName:{course.Master.Name}\tFamily:{course.Master.LastName}\tActive:{course.Master.IsActive}");
                                }
                                Console.WriteLine("--------------------------------------------------------------------------------");
                            }
                            Console.Write("\nCourse id:");
                        }
                        PrintDatetimeAndUserStatistic();
                        var chooseCourseForEdit = dbEditCourse.Courses.Find(answerSelectCourseId);
                        if (chooseCourseForEdit != null)
                        {
                            goto SelectCoursePropertyEdit;
                        }
                        else
                        {
                            PrintDatetimeAndUserStatistic();
                            IdNotFound("course list!", answerSelectCourseId);
                            goto CoursesForEdit;
                        }
                    SelectCoursePropertyEdit:
                        Console.WriteLine($"\nChoose your desired property for editing:{chooseCourseForEdit.CourseName}");
                        Console.WriteLine("\n\t\t1. Name" +
                                          "\n\t\t2. Unit" +
                                          "\n\t\t3. Master" +
                                          "\n\t\t4. Active");
                        Console.Write("\nChoose a number to continue.\n\tYour answer:");
                        int selectCoursePropertyEdit;
                        while (!int.TryParse(Console.ReadLine(),out selectCoursePropertyEdit))
                        {
                            PrintJustNumbers("Select the items in the menu.");
                            Thread.Sleep(3000);
                            PrintDatetimeAndUserStatistic();
                            Console.WriteLine($"\nChoose your desired property for editing:{chooseCourseForEdit.CourseName}");
                            Console.WriteLine("\n\t\t1. Name" +
                                              "\n\t\t2. Unit" +
                                              "\n\t\t3. Master" +
                                              "\n\t\t4. Active");
                            Console.Write("\nChoose a number to continue.\n\tYour answer:");
                        }
                        PrintDatetimeAndUserStatistic();

                        switch (selectCoursePropertyEdit)
                        {
                            #region Case1: Edit Course Name
                            case 1:

                                Console.WriteLine($"\n(Course Edit!)\n\tEdit form for: {chooseCourseForEdit.CourseName}");
                                Console.WriteLine($"\n\tName: {chooseCourseForEdit.CourseName}");
                                Console.Write("\n\tNew Course name: ");
                                string newNameForCourse = Console.ReadLine();
                                string courseOldName = chooseCourseForEdit.CourseName;
                                chooseCourseForEdit.CourseName = newNameForCourse;
                                PrintDatetimeAndUserStatistic();
                                Console.WriteLine($"\n\tCourse name changed to:{newNameForCourse} from({courseOldName})\n");
                                goto FinishEditCourse;

                            #endregion

                            #region Case2: Edit Course Unit
                            case 2: 

                                Console.WriteLine($"\n(Course Edit!)\n\tEdit form for: {chooseCourseForEdit.CourseName}");
                                Console.WriteLine($"\n\tUnit: {chooseCourseForEdit.CourseUnit}");
                                Console.Write("\n\tNew course unit: ");
                                int newUnitForCourse;
                                while (!int.TryParse(Console.ReadLine(), out newUnitForCourse))
                                {
                                    PrintJustNumbers("Course Unit!");
                                    PrintDatetimeAndUserStatistic();
                                    Console.WriteLine($"\n(Course Edit!)\n\tEdit form for: {chooseCourseForEdit.CourseName}");
                                    Console.WriteLine($"\n\tUnit: {chooseCourseForEdit.CourseUnit}");
                                    Console.Write("\n\tNew course unit: ");
                                }
                                int courseOldUnit = chooseCourseForEdit.CourseUnit;
                                chooseCourseForEdit.CourseUnit = newUnitForCourse;
                                PrintDatetimeAndUserStatistic();
                                Console.WriteLine($"\n\tUnit changed to:{newUnitForCourse} for({chooseCourseForEdit.CourseUnit})\n");
                                goto FinishEditCourse;

                            #endregion

                            #region Case3: Edit Master For Course
                            case 3:
                                ChooseMasterForCourseInEditSection:
                                PrintDatetimeAndUserStatistic();
                                Console.WriteLine($"\n(Course Edit!)\n\tEdit form for: {chooseCourseForEdit.CourseName}");
                                if (chooseCourseForEdit.Master!=null)
                                {
                                    Console.WriteLine($"\n\tMaster: {chooseCourseForEdit.Master.Name} {chooseCourseForEdit.Master.LastName}\n");
                                }
                                else
                                {
                                    PrintDatetimeAndUserStatistic();
                                    Console.WriteLine("Sorry, No registered master was found for this course!" +
                                                      "\n\tYour should register a new one(Case1), or choose from the list(Case2).");
                                    
                                }
                                Console.WriteLine($"\nDoes {chooseCourseForEdit.CourseName} have new master?");
                                Console.WriteLine("\n\t\t1.Yes \n\t\t2.No ");
                                Console.Write("Your answer would be: ");
                                int newMasterForCourse;
                                while (!int.TryParse(Console.ReadLine(), out newMasterForCourse))
                                {
                                    PrintJustNumbers("Select the items in the menu.");
                                    Console.Clear();
                                    PrintDatetimeAndUserStatistic();
                                    Console.WriteLine($"\n(Course Edit!)\n\tEdit form for: {chooseCourseForEdit.CourseName}");
                                    Console.WriteLine($"\n\tMaster: {chooseCourseForEdit.Master.Name} {chooseCourseForEdit.Master.LastName}\n");
                                    Console.WriteLine($"\nDoes {chooseCourseForEdit.CourseName} have new master?");
                                    Console.WriteLine("\n\t\t1.Yes \n\t\t2.No ");
                                    Console.Write("Your answer would be: ");
                                }
                                PrintDatetimeAndUserStatistic();
                                switch (newMasterForCourse)
                                {

                                    case 1:
                                        Console.WriteLine("\n(Master) Registeration form!");
                                        Console.Write("\n\tFieldOfStudy: ");
                                        string newMasterFieldOfStudyinEditSection = Console.ReadLine();
                                        Console.Write("\tMonthly Salary: ");
                                        float newMasterSalaryinEditSection;
                                        while (!float.TryParse(Console.ReadLine(), out newMasterSalaryinEditSection))
                                        {
                                            PrintJustNumbers("Monthly Salary");
                                            Console.Clear();
                                            PrintDatetimeAndUserStatistic();
                                            Console.WriteLine("\n(Master) Registeration form!");
                                            Console.Write($"\n\tFieldOfStudy: {newMasterFieldOfStudyinEditSection}");
                                            Console.Write("\tMonthly Salary: ");
                                        }
                                        Console.Write("\tFirst name : ");
                                        string newMasterNameinEditSection = Console.ReadLine();
                                        Console.Write("\tLast Name  : ");
                                        string newMasterLastNameinEditSection = Console.ReadLine();
                                        Console.Write("\tPhonenumber: ");
                                        string newMasterPhoneinEditSection = Console.ReadLine();
                                        while (!Regex.IsMatch(newMasterPhoneinEditSection, phonePattern))
                                        {
                                            Console.WriteLine("\n\tIncorrect Phonenumber!");
                                            Thread.Sleep(3000);
                                            PrintDatetimeAndUserStatistic();
                                            Console.WriteLine("\n(Master) Registeration form for{0}", chooseCourseForEdit.CourseName);
                                            Console.Write($"\n\tField of study: {newMasterFieldOfStudyinEditSection}");
                                            Console.Write($"\n\tMonthly salary: {newMasterSalaryinEditSection}");
                                            Console.Write($"\n\tFirst name: {newMasterNameinEditSection}");
                                            Console.Write($"\n\tLast name: {newMasterLastNameinEditSection}");
                                            Console.Write("\tPhonenumber : ");
                                            newMasterPhoneinEditSection = Console.ReadLine();
                                        }
                                        Console.Write("\tPassword   : ");
                                        string newMasterPassinEditSection = Console.ReadLine();
                                        Console.Write("\tAge        : ");
                                        int newMasterAgeinEditSection = Convert.ToInt32(Console.ReadLine());
                                        Master newAddMaster = new Master($"{newMasterFieldOfStudyinEditSection}", newMasterSalaryinEditSection, $"{newMasterNameinEditSection}", $"{newMasterLastNameinEditSection}", $"{newMasterPhoneinEditSection}", $"{newMasterPassinEditSection}", dbEditCourse.Roles.Find(3), newMasterAgeinEditSection);
                                        dbEditCourse.Masters.Add(newAddMaster);
                                        chooseCourseForEdit.Master = newAddMaster;
                                        PrintDatetimeAndUserStatistic();
                                        Console.WriteLine($"\n\tMaster of this ({chooseCourseForEdit.CourseName})course,\nHas been changed to{chooseCourseForEdit.Master.Name} {chooseCourseForEdit.Master.LastName}     <(From)>     {newMasterNameinEditSection} {newMasterLastNameinEditSection}\n");
                                        goto FinishEditCourse;


                                    case 2:

                                    OldMasterSelectionForCourse:
                                        PrintDatetimeAndUserStatistic();
                                        Console.WriteLine($"\nPlease select the master for this:({chooseCourseForEdit.CourseName}) course!");
                                        List<Master> masterListInEditSection = dbEditCourse.Masters.ToList();
                                        foreach (Master master in masterListInEditSection)
                                        {
                                            Console.WriteLine($"Id:{master.Id}" +
                                                              $"\tName:{master.Name}" +
                                                              $"\tLastName:{master.LastName}" +
                                                              $"\tPhonenumber:{master.Phonenumber}\n" +
                                                              $"FieldOfStudy:{master.FieldOfStudy}" +
                                                              $"\tSalary:{master.Salary}" +
                                                              $"\tRegisterdate:{master.Registerdate}" +
                                                              $"\tActive:{master.IsActive}");
                                            Console.WriteLine("--------------------------------------------------------------------------------");
                                        }
                                        Console.Write("\nMaster id: ");
                                        int answerSelectNewCourseInEditSection;
                                        while (!int.TryParse(Console.ReadLine(), out answerSelectNewCourseInEditSection))
                                        {
                                            PrintJustNumbers("Master id!");
                                            PrintDatetimeAndUserStatistic();
                                            foreach (Master master in masterListInEditSection)
                                            {
                                                Console.WriteLine($"Id:{master.Id}" +
                                                                  $"\tName:{master.Name}" +
                                                                  $"\tLastName:{master.LastName}" +
                                                                  $"\tPhonenumber:{master.Phonenumber}\n" +
                                                                  $"FieldOfStudy:{master.FieldOfStudy}" +
                                                                  $"\tSalary:{master.Salary}" +
                                                                  $"\tRegisterdate:{master.Registerdate}" +
                                                                  $"\tActive:{master.IsActive}");
                                                Console.WriteLine("--------------------------------------------------------------------------------");
                                            }
                                            Console.Write("\nMaster id: ");
                                        }
                                        var selectExistingMasterInEditSection = dbEditCourse.Masters.Find(answerSelectNewCourseInEditSection);
                                        if (selectExistingMasterInEditSection != null)
                                        {
                                            goto CourseEditSection;
                                        }
                                        else
                                        {
                                            PrintDatetimeAndUserStatistic();
                                            IdNotFound("Master list!", answerSelectNewCourseInEditSection);
                                            goto OldMasterSelectionForCourse;
                                        }
                                    CourseEditSection:
                                        chooseCourseForEdit.Master = selectExistingMasterInEditSection;
                                        PrintDatetimeAndUserStatistic();
                                        Console.WriteLine($"\n\tMaster of this ({chooseCourseForEdit.CourseName})course,\nHas been changed to{selectExistingMasterInEditSection.Name} {selectExistingMasterInEditSection.LastName}\n");
                                        goto FinishEditCourse;

                                    default:
                                        goto ChooseMasterForCourseInEditSection;
                                }
                            #endregion

                            #region Case4: Edit IsActive
                            case 4:

                                if (chooseCourseForEdit.IsActive == true)
                                {
                                    chooseCourseForEdit.IsActive = false;
                                }
                                else
                                {
                                    chooseCourseForEdit.IsActive = true;
                                }
                                PrintDatetimeAndUserStatistic();
                                Console.WriteLine($"\n\tAccess changed to:{chooseCourseForEdit.IsActive} for({chooseCourseForEdit.CourseName} with ({chooseCourseForEdit.CourseUnit})Units.)\n");
                                //goto FinishEditCourse;


                                break;
                            #endregion
                            
                            default:
                                goto SelectCoursePropertyEdit;
                        }
                    FinishEditCourse:
                        dbEditCourse.SaveChanges();
                        Console.Write("Press any key to return to menu.");
                        Console.ReadKey();
                        goto Menu;
                    }
                #endregion

                #region Case 13: Remove Employee

                case 13:
                RemoveEmployee:
                    PrintDatetimeAndUserStatistic();
                    Console.WriteLine("\nPlease select an employee to remove.\n");
                    DbProjectContext dbRemoveEmployee = new DbProjectContext("ProjectConnStr");
                    using (dbRemoveEmployee)
                    {
                        List<Employee> viewEmployeeToDelete = dbRemoveEmployee.Employees.ToList();
                        foreach (Employee employee in viewEmployeeToDelete)
                        {
                            Console.WriteLine($"Id:{employee.Id}" +
                                              $"\tName:{employee.Name}" +
                                              $"\tLast Name:{employee.LastName}" +
                                              $"\tDepartment:{employee.DepartmentName}\n" +
                                              $"Phonenumber:{employee.Phonenumber}" +
                                              $"\tSalary:{employee.Salary}" +
                                              $"\tRegisterdate:{employee.Registerdate}" +
                                              $"\tActive:{employee.IsActive}");

                            Console.WriteLine("--------------------------------------------------------------------------------");
                        }
                        Console.Write("Employee id: ");
                        int answerSelectRemoveEmployee;
                        while (!int.TryParse(Console.ReadLine(),out answerSelectRemoveEmployee))
                        {
                            PrintJustNumbers("Select the items in the menu.");
                            Thread.Sleep(3000);
                            PrintDatetimeAndUserStatistic();
                            foreach (Employee employee in viewEmployeeToDelete)
                            {
                                Console.WriteLine($"Id:{employee.Id}" +
                                                  $"\tName:{employee.Name}" +
                                                  $"\tLast Name:{employee.LastName}" +
                                                  $"\tDepartment:{employee.DepartmentName}\n" +
                                                  $"Phonenumber:{employee.Phonenumber}" +
                                                  $"\tSalary:{employee.Salary}" +
                                                  $"\tRegisterdate:{employee.Registerdate}" +
                                                  $"\tActive:{employee.IsActive}");

                                Console.WriteLine("--------------------------------------------------------------------------------");
                            }
                            Console.Write("Employee id: ");
                        }
                        var removeEmployee = dbRemoveEmployee.Employees.Find(answerSelectRemoveEmployee);
                        if (removeEmployee!=null)
                        {
                            goto RemoveEmployeeById;
                        }
                        else
                        {
                            PrintDatetimeAndUserStatistic();
                            IdNotFound("Employee List!",answerSelectRemoveEmployee);
                            goto RemoveEmployee;
                        }
                    RemoveEmployeeById:
                        dbRemoveEmployee.Employees.Remove(removeEmployee);
                        PrintDatetimeAndUserStatistic();
                        Console.WriteLine($"\n\t{removeEmployee.Name} {removeEmployee.LastName} wad successfuly removed.\n");
                        dbRemoveEmployee.SaveChanges();
                    }
                    Console.Write("Press any key to return to menu.");
                    Console.ReadKey();
                    goto Menu;

                #endregion

                #region Case 14: Remove Student

                case 14:
                RemoveStudent:
                    PrintDatetimeAndUserStatistic();
                    Console.WriteLine("\nPlease select an student to remove.\n");
                    DbProjectContext dbRemoveStudent = new DbProjectContext("ProjectConnStr");
                    using (dbRemoveStudent)
                    {
                        IEnumerable<Student> viewStudentToRemove = dbRemoveStudent.Students.ToList();
                        foreach (Student student in viewStudentToRemove)
                        {
                            Console.WriteLine($"Id:{student.Id}" +
                                              $"\tStudentCode:{student.StudentCode}" +
                                              $"\tName:{student.Name}" +
                                              $"\tLastName:{student.LastName}" +
                                              $"\tPhonenumber:{student.Phonenumber}" +
                                              $"\tBirthdate:{student.Birthdate}\n" +
                                              $"FieldOfStudy:{student.FieldOfStudy}" +
                                              $"\tTuitionOfEachTerm:{student.TuitionOfEachTerm}" +
                                              $"\tRegister Date:{student.Registerdate}" +
                                              $"\tActive:{student.IsActive}");
                            Console.WriteLine("--------------------------------------------------------------------------------");
                        }
                        Console.Write("Student id: ");
                        int answerSelectRemoveStudent;
                        while (!int.TryParse(Console.ReadLine(), out answerSelectRemoveStudent))
                        {
                            PrintJustNumbers("Select the items in the menu.");
                            Thread.Sleep(3000);
                            PrintDatetimeAndUserStatistic();
                            foreach (Student student in viewStudentToRemove)
                            {
                                Console.WriteLine($"Id:{student.Id}" +
                                                  $"\tStudentCode:{student.StudentCode}" +
                                                  $"\tName:{student.Name}" +
                                                  $"\tLastName:{student.LastName}" +
                                                  $"\tPhonenumber:{student.Phonenumber}" +
                                                  $"\tBirthdate:{student.Birthdate}\n" +
                                                  $"FieldOfStudy:{student.FieldOfStudy}" +
                                                  $"\tTuitionOfEachTerm:{student.TuitionOfEachTerm}" +
                                                  $"\tRegister Date:{student.Registerdate}" +
                                                  $"\tActive:{student.IsActive}");
                                Console.WriteLine("--------------------------------------------------------------------------------");
                            }
                            Console.Write("Student id: ");
                        }
                        var removeStudent = dbRemoveStudent.Students.Find(answerSelectRemoveStudent);
                        if (removeStudent != null)
                        {
                            goto RemoveStudentById;
                        }
                        else
                        {
                            PrintDatetimeAndUserStatistic();
                            IdNotFound("Student List!", answerSelectRemoveStudent);
                            goto RemoveStudent;
                        }
                    RemoveStudentById:
                        dbRemoveStudent.Students.Remove(removeStudent);
                        PrintDatetimeAndUserStatistic();
                        Console.WriteLine($"\n\t{removeStudent.Name} {removeStudent.LastName} wad successfuly removed.\n");
                        dbRemoveStudent.SaveChanges();
                    }
                    Console.Write("Press any key to return to menu.");
                    Console.ReadKey();
                    goto Menu;

                #endregion

                #region Case 15: Remove Master

                case 15:
                RemoveMaster:
                    PrintDatetimeAndUserStatistic();
                    Console.WriteLine("\nPlease select a master to remove.\n");
                    DbProjectContext dbRemoveMaster = new DbProjectContext("ProjectConnStr");
                    using (dbRemoveMaster)
                    {
                        List<Master> viewMasterToRemove = dbRemoveMaster.Masters.ToList();

                        foreach (Master master in viewMasterToRemove)
                        {
                            Console.WriteLine($"Id:{master.Id}" +
                                              $"\tName:{master.Name}" +
                                              $"\tLastName:{master.LastName}" +
                                              $"\tPhonenumber:{master.Phonenumber}\n" +
                                              $"FieldOfStudy:{master.FieldOfStudy}" +
                                              $"\tSalary:{master.Salary}" +
                                              $"\tRegisterdate:{master.Registerdate}" +
                                              $"\tActive:{master.IsActive}");
                            Console.WriteLine("--------------------------------------------------------------------------------");

                        }
                        Console.Write("Master id: ");
                        int answerSelectRemoveMaster;
                        while (!int.TryParse(Console.ReadLine(),out answerSelectRemoveMaster))
                        {
                            PrintJustNumbers("Select the items in the menu.");
                            Thread.Sleep(3000);
                            PrintDatetimeAndUserStatistic();
                            foreach (Master master in viewMasterToRemove)
                            {
                                Console.WriteLine($"Id:{master.Id}" +
                                                  $"\tName:{master.Name}" +
                                                  $"\tLastName:{master.LastName}" +
                                                  $"\tPhonenumber:{master.Phonenumber}\n" +
                                                  $"FieldOfStudy:{master.FieldOfStudy}" +
                                                  $"\tSalary:{master.Salary}" +
                                                  $"\tRegisterdate:{master.Registerdate}" +
                                                  $"\tActive:{master.IsActive}");
                                Console.WriteLine("--------------------------------------------------------------------------------");
                            }
                            Console.Write("Student id: ");
                        }
                        var removeMaster = dbRemoveMaster.Masters.Find(answerSelectRemoveMaster);
                        if (removeMaster != null)
                        {
                            goto RemoveMasterById;
                        }
                        else
                        {
                            PrintDatetimeAndUserStatistic();
                            IdNotFound("Master List!", answerSelectRemoveMaster);
                            goto RemoveMaster;
                        }
                    RemoveMasterById:
                        List<Course> removeCourseOfMaster = dbRemoveMaster.Courses.Where(t => t.Master.Id == removeMaster.Id).ToList();                        //Let it be here
                        //if (!dbRemoveMaster.Courses.Any())
                        //{
                        //    foreach (Course course in removeCourseOfMaster)
                        //    {
                        //        dbRemoveMaster.Courses.Remove(course);
                        //    }
                        //}
                        dbRemoveMaster.Masters.Remove(removeMaster);
                        PrintDatetimeAndUserStatistic();
                        Console.WriteLine($"\n\t{removeMaster.Name} {removeMaster.LastName} wad successfuly removed.\n");
                        dbRemoveMaster.SaveChanges();
                    }
                    Console.Write("Press any key to return to menu.");
                    Console.ReadKey();
                    goto Menu;

                #endregion

                #region Case 16: Remove Course

                case 16:
                RemoveCourse:
                    PrintDatetimeAndUserStatistic();
                    Console.WriteLine("\nPlease select a course to remove.\n");
                    DbProjectContext dbRemoveCourse = new DbProjectContext("ProjectConnStr");
                    using (dbRemoveCourse)
                    {
                        Console.WriteLine("\nCourse:\n");
                        List<Course> viewCourseToRemove = dbRemoveCourse.Courses.ToList();
                        foreach (Course course in viewCourseToRemove)
                        {
                            Console.WriteLine($"CourseId:{course.Id}" +
                                              $"\tCourseName:{course.CourseName}" +
                                              $"\tCourseUnit:{course.CourseUnit}" +
                                              $"\tRegisterDate:{course.RegisterDate}" +
                                              $"\tActive:{course.IsActive}");
                            Console.WriteLine("--------------------------------------------------------------------------------");
                        }
                        Console.Write("Course id: ");
                        int answerSelectRemoveCourse;
                        while (!int.TryParse(Console.ReadLine(),out answerSelectRemoveCourse))
                        {
                            PrintJustNumbers("Select the items in the menu.");
                            Thread.Sleep(3000);
                            PrintDatetimeAndUserStatistic();
                            foreach (Course course in viewCourseToRemove)
                            {
                                Console.WriteLine($"CourseId:{course.Id}" +
                                                  $"\tCourseName:{course.CourseName}" +
                                                  $"\tCourseUnit:{course.CourseUnit}" +
                                                  $"\tRegisterDate:{course.RegisterDate}" +
                                                  $"\tActive:{course.IsActive}");
                                Console.WriteLine("--------------------------------------------------------------------------------");
                            }
                        }
                        var removeCourse = dbRemoveCourse.Courses.Find(answerSelectRemoveCourse);
                        if (removeCourse != null)
                        {
                            goto RemoveCourseById;
                        }
                        else
                        {
                            PrintDatetimeAndUserStatistic();
                            IdNotFound("Course List!", answerSelectRemoveCourse);
                            goto RemoveCourse;
                        }
                    RemoveCourseById:
                        dbRemoveCourse.Courses.Remove(removeCourse);
                        PrintDatetimeAndUserStatistic();
                        Console.WriteLine($"\n\t{removeCourse.CourseName} wad successfuly removed.\n");
                        dbRemoveCourse.SaveChanges();
                    }
                    Console.Write("Press any key to return to menu.");
                    Console.ReadKey();
                    goto Menu;

                #endregion

                #region Case 17: Chnage Object Roles

                case 17:
                ChangeRole:
                    PrintDatetimeAndUserStatistic();
                    Console.WriteLine("\nPlease choose a role to change.\n");
                    Console.WriteLine("\t\t1.Employee" +
                                      "\n\t\t2.Master" +
                                      "\n\t\t3.Student" +
                                      "\n\t\t4.Exit menu");
                    Console.Write("Answer: ");
                    int answerChangeRole;
                    while (!int.TryParse(Console.ReadLine(),out answerChangeRole))
                    {
                        PrintJustNumbers("Use only items of the list!");
                        PrintDatetimeAndUserStatistic();
                        Console.WriteLine("\nPlease choose a role to change.\n");
                        Console.WriteLine("\t\t1.Employee" +
                                          "\n\t\t2.Master" +
                                          "\n\t\t3.Student" +
                                          "\n\t\t4.Exit menu");
                        Console.Write("Answer: ");
                    }
                    DbProjectContext dbChangeRole = new DbProjectContext("ProjectConnStr");
                    using (dbChangeRole)
                    {
                        switch (answerChangeRole)
                        {
                            #region Case 1: Change Employee Role
                            case 1:
                            ChangeEmployeeRole:
                                PrintDatetimeAndUserStatistic();
                                Console.WriteLine("\nChoose an employee to change the role.\n");
                                List<Employee> viewEmployeeListInChangeRole = dbChangeRole.Employees.ToList();
                                foreach (Employee employee in viewEmployeeListInChangeRole)
                                {
                                    Console.WriteLine($"Id:{employee.Id}" +
                                                      $"\tName:{employee.Name}" +
                                                      $"\tLast Name:{employee.LastName}" +
                                                      $"\tDepartment:{employee.DepartmentName}\n" +
                                                      $"Phonenumber:{employee.Phonenumber}" +
                                                      $"\tSalary:{employee.Salary}" +
                                                      $"\tRegisterdate:{employee.Registerdate}" +
                                                      $"\tActive:{employee.IsActive}");

                                    Console.WriteLine("--------------------------------------------------------------------------------");
                                }
                                Console.Write("\nEmployee id: ");
                                int answerSelectEmployeeChangeRole;
                                while (!int.TryParse(Console.ReadLine(),out answerSelectEmployeeChangeRole))
                                {
                                    PrintJustNumbers("Employee Id!");
                                    PrintDatetimeAndUserStatistic();
                                    foreach (Employee employee in viewEmployeeListInChangeRole)
                                    {
                                        Console.WriteLine($"Id:{employee.Id}" +
                                                          $"\tName:{employee.Name}" +
                                                          $"\tLast Name:{employee.LastName}" +
                                                          $"\tDepartment:{employee.DepartmentName}\n" +
                                                          $"Phonenumber:{employee.Phonenumber}" +
                                                          $"\tSalary:{employee.Salary}" +
                                                          $"\tRegisterdate:{employee.Registerdate}" +
                                                          $"\tActive:{employee.IsActive}");

                                        Console.WriteLine("--------------------------------------------------------------------------------");
                                    }
                                    Console.Write("\nEmployee id: ");
                                }
                                Employee selectEmployeeChangeRole = dbChangeRole.Employees.Find(answerSelectEmployeeChangeRole);
                                if (selectEmployeeChangeRole != null)
                                {
                                    goto SelectForeignObjectChangeRoleEmployee;
                                }
                                else
                                {
                                    PrintDatetimeAndUserStatistic();
                                    IdNotFound("Employee list!", answerSelectEmployeeChangeRole);
                                    goto ChangeEmployeeRole;
                                }
                            SelectForeignObjectChangeRoleEmployee:
                                PrintDatetimeAndUserStatistic();
                                Console.WriteLine($"\nPlease select another role for: {selectEmployeeChangeRole.Name} {selectEmployeeChangeRole.LastName}\n");
                                Console.WriteLine("\t\t1.Master" +
                                                  "\n\t\t2.Student");
                                Console.WriteLine("\nAnswer: ");
                                int answerSelectRoleForEmployee;
                                while (!int.TryParse(Console.ReadLine(), out answerSelectRoleForEmployee))
                                {
                                    PrintJustNumbers("Use only items of the list!");
                                    PrintDatetimeAndUserStatistic();
                                    Console.WriteLine($"\nPlease select another role for: {selectEmployeeChangeRole.Name} {selectEmployeeChangeRole.LastName}\n");
                                    Console.WriteLine("\t\t1.Master" +
                                                      "\n\t\t2.Student");
                                    Console.WriteLine("\nAnswer: ");
                                }
                                switch (answerSelectRoleForEmployee)
                                {
                                    #region Change role to master
                                    case 1:

                                        dbChangeRole.Masters.Add(new Master("", selectEmployeeChangeRole.Salary, selectEmployeeChangeRole.Name, selectEmployeeChangeRole.LastName, selectEmployeeChangeRole.Phonenumber, selectEmployeeChangeRole.Password, dbChangeRole.Roles.Find(3), 0));
                                        dbChangeRole.Employees.Remove(selectEmployeeChangeRole);
                                        PrintDatetimeAndUserStatistic();
                                        Console.WriteLine($"\n\t{selectEmployeeChangeRole.Name} {selectEmployeeChangeRole.LastName} => Role has changed to (Master)!");
                                        goto FinishChangeRole;

                                    #endregion

                                    #region Change role to student
                                    case 2:
                                        dbChangeRole.Students.Add(new Student());
                                        dbChangeRole.Employees.Remove(selectEmployeeChangeRole);
                                        PrintDatetimeAndUserStatistic();
                                        Console.WriteLine($"\n\t{selectEmployeeChangeRole.Name} {selectEmployeeChangeRole.LastName} => Role has changed to (Student)!");
                                        goto FinishChangeRole;

                                        #endregion
                                }




                                break;
                            #endregion

                            #region Case 2: Change Master Role
                            case 2:
                                ChangeMasterRole:
                                PrintDatetimeAndUserStatistic();
                                Console.WriteLine("\nChoose a master to change the role.\n");
                                List<Master> viewMasterListInChangeRole = dbChangeRole.Masters.ToList();

                                foreach (Master master in viewMasterListInChangeRole)
                                {
                                    Console.WriteLine($"Id:{master.Id}" +
                                                      $"\tName:{master.Name}" +
                                                      $"\tLastName:{master.LastName}" +
                                                      $"\tPhonenumber:{master.Phonenumber}\n" +
                                                      $"FieldOfStudy:{master.FieldOfStudy}" +
                                                      $"\tSalary:{master.Salary}" +
                                                      $"\tRegisterdate:{master.Registerdate}" +
                                                      $"\tActive:{master.IsActive}");
                                    Console.WriteLine("--------------------------------------------------------------------------------");
                                }
                                Console.Write("\nMaster id: ");
                                int answerSelectMasterChangeRple;
                                while (!int.TryParse(Console.ReadLine(), out answerSelectMasterChangeRple))
                                {
                                    PrintJustNumbers("Master Id!");
                                    PrintDatetimeAndUserStatistic();
                                    foreach (Master master in viewMasterListInChangeRole)
                                    {
                                        Console.WriteLine($"Id:{master.Id}" +
                                                          $"\tName:{master.Name}" +
                                                          $"\tLastName:{master.LastName}" +
                                                          $"\tPhonenumber:{master.Phonenumber}\n" +
                                                          $"FieldOfStudy:{master.FieldOfStudy}" +
                                                          $"\tSalary:{master.Salary}" +
                                                          $"\tRegisterdate:{master.Registerdate}" +
                                                          $"\tActive:{master.IsActive}");
                                        Console.WriteLine("--------------------------------------------------------------------------------");
                                    }
                                    Console.Write("\nMaster id: ");
                                }
                                Master selectMasterChangeRole = dbChangeRole.Masters.Find(answerSelectMasterChangeRple);
                                if (selectMasterChangeRole != null)
                                {
                                    goto SelectForeignObjectChangeRoleMaster;
                                }
                                else
                                {
                                    PrintDatetimeAndUserStatistic();
                                    IdNotFound("Master list!", answerSelectMasterChangeRple);
                                    goto ChangeMasterRole;
                                }
                            SelectForeignObjectChangeRoleMaster:
                                PrintDatetimeAndUserStatistic();
                                Console.WriteLine($"\nPlease select another role for: {selectMasterChangeRole.Name} {selectMasterChangeRole.LastName}\n");
                                Console.WriteLine("\t\t1.Employee" +
                                                  "\n\t\t2.Student");
                                Console.WriteLine("\nAnswer: ");
                                int answerSelectRoleForMaster;
                                while (!int.TryParse(Console.ReadLine(), out answerSelectRoleForMaster))
                                {
                                    PrintJustNumbers("Use only items of the list!");
                                    PrintDatetimeAndUserStatistic();
                                    Console.WriteLine($"\nPlease select another role for: {selectMasterChangeRole.Name} {selectMasterChangeRole.LastName}\n");
                                    Console.WriteLine("\t\t1.Employee" +
                                                      "\n\t\t2.Student");
                                    Console.WriteLine("\nAnswer: ");
                                }
                                switch (answerSelectRoleForMaster)
                                {
                                    
                                    #region Change role to employee
                                    case 1:

                                        dbChangeRole.Employees.Add(new Employee("",selectMasterChangeRole.Salary,selectMasterChangeRole.Name,selectMasterChangeRole.LastName,selectMasterChangeRole.Phonenumber,selectMasterChangeRole.Password,dbChangeRole.Roles.Find(2),0));
                                        dbChangeRole.Masters.Remove(selectMasterChangeRole);
                                        PrintDatetimeAndUserStatistic();
                                        Console.WriteLine($"\n\t{selectMasterChangeRole.Name} {selectMasterChangeRole.LastName} => Role has changed to (Employee)!");
                                        goto FinishChangeRole;
                                    #endregion

                                    #region Change role to student
                                    case 2:

                                        dbChangeRole.Students.Add(new Student(selectMasterChangeRole.FieldOfStudy,selectMasterChangeRole.Name,selectMasterChangeRole.LastName,selectMasterChangeRole.Phonenumber,selectMasterChangeRole.Password,dbChangeRole.Roles.Find(4),0));
                                        dbChangeRole.Masters.Remove(selectMasterChangeRole);
                                        PrintDatetimeAndUserStatistic();
                                        Console.WriteLine($"\n\t{selectMasterChangeRole.Name} {selectMasterChangeRole.LastName} => Role has changed to (Student)!");
                                        goto FinishChangeRole;

                                    #endregion

                                    default:
                                        goto SelectForeignObjectChangeRoleMaster;
                                }
                            #endregion

                            #region Case 3: Change Student Role
                            case 3:
                            ChangeStudentRole:
                                PrintDatetimeAndUserStatistic();
                                Console.WriteLine("\nChoose an student to change the role.\n");
                                List<Student> viewStudentListInChangeRole = dbChangeRole.Students.ToList();
                                foreach (Student student in viewStudentListInChangeRole)
                                {
                                    Console.WriteLine($"Id:{student.Id}" +
                                                      $"\tStudentCode:{student.StudentCode}" +
                                                      $"\tName:{student.Name}" +
                                                      $"\tLastName:{student.LastName}" +
                                                      $"\tPhonenumber:{student.Phonenumber}" +
                                                      $"\tBirthdate:{student.Birthdate}\n" +
                                                      $"FieldOfStudy:{student.FieldOfStudy}" +
                                                      $"\tTuitionOfEachTerm:{student.TuitionOfEachTerm}" +
                                                      $"\tRegister Date:{student.Registerdate}" +
                                                      $"\tActive:{student.IsActive}");
                                    Console.WriteLine("--------------------------------------------------------------------------------");
                                }
                                Console.Write("\nStudent id: ");
                                int answerSelectStudentChangeRple;
                                while (!int.TryParse(Console.ReadLine(),out answerSelectStudentChangeRple))
                                {
                                    PrintJustNumbers("Student Id!");
                                    PrintDatetimeAndUserStatistic();
                                    foreach (Student student in viewStudentListInChangeRole)
                                    {
                                        Console.WriteLine($"Id:{student.Id}" +
                                                          $"\tStudentCode:{student.StudentCode}" +
                                                          $"\tName:{student.Name}" +
                                                          $"\tLastName:{student.LastName}" +
                                                          $"\tPhonenumber:{student.Phonenumber}" +
                                                          $"\tBirthdate:{student.Birthdate}\n" +
                                                          $"FieldOfStudy:{student.FieldOfStudy}" +
                                                          $"\tTuitionOfEachTerm:{student.TuitionOfEachTerm}" +
                                                          $"\tRegister Date:{student.Registerdate}" +
                                                          $"\tActive:{student.IsActive}");
                                        Console.WriteLine("--------------------------------------------------------------------------------");
                                    }
                                    Console.Write("\nStudent id: ");
                                }
                                Student selectStudentChangeRole = dbChangeRole.Students.Find(answerSelectStudentChangeRple);
                                if (selectStudentChangeRole != null)
                                {
                                    goto SelectForeignObjectChangeRoleStudent;
                                }
                                else
                                {
                                    PrintDatetimeAndUserStatistic();
                                    IdNotFound("Student list!", answerSelectStudentChangeRple);
                                    goto ChangeStudentRole;
                                }
                            SelectForeignObjectChangeRoleStudent:
                                PrintDatetimeAndUserStatistic();
                                Console.WriteLine($"\nPlease select another role for: {selectStudentChangeRole.Name} {selectStudentChangeRole.LastName}\n");
                                Console.WriteLine("\t\t1.Employee" +
                                                  "\n\t\t2.Master");
                                Console.WriteLine("\nAnswer: ");
                                int answerSelectRoleForStudent;
                                while (!int.TryParse(Console.ReadLine(),out answerSelectRoleForStudent))
                                {
                                    PrintJustNumbers("Use only items of the list!");
                                    PrintDatetimeAndUserStatistic();
                                    Console.WriteLine($"\nPlease select another role for: {selectStudentChangeRole.Name} {selectStudentChangeRole.LastName}\n");
                                    Console.WriteLine("\t\t1.Employee" +
                                                      "\n\t\t2.Master");
                                    Console.WriteLine("\nAnswer: ");
                                }
                                switch (answerSelectRoleForStudent)
                                {
                                    #region Change role to employee
                                    case 1:

                                        dbChangeRole.Employees.Add(new Employee("", 0, selectStudentChangeRole.Name, selectStudentChangeRole.LastName, selectStudentChangeRole.Phonenumber, selectStudentChangeRole.Password, dbChangeRole.Roles.Find(2), 0));
                                        dbChangeRole.Students.Remove(selectStudentChangeRole);
                                        PrintDatetimeAndUserStatistic();
                                        Console.WriteLine($"\n\t{selectStudentChangeRole.Name} {selectStudentChangeRole.LastName} => Role has changed to (Student)!");
                                        goto FinishChangeRole;

                                    #endregion

                                    #region Change role to employee
                                    case 2:

                                        dbChangeRole.Masters.Add(new Master(selectStudentChangeRole.FieldOfStudy, 0,selectStudentChangeRole.Name, selectStudentChangeRole.LastName, selectStudentChangeRole.Phonenumber, selectStudentChangeRole.Password, dbChangeRole.Roles.Find(3), 0));
                                        dbChangeRole.Students.Remove(selectStudentChangeRole);
                                        PrintDatetimeAndUserStatistic();
                                        Console.WriteLine($"\n\t{selectStudentChangeRole.Name} {selectStudentChangeRole.LastName} => Role has changed to (master)!");
                                        goto FinishChangeRole;

                                    #endregion

                                    default:
                                        goto SelectForeignObjectChangeRoleStudent;
                                }
                            #endregion

                            #region Case 4: Exit menu
                            case 4:

                                PrintDatetimeAndUserStatistic();
                                Console.WriteLine("Exit menu.\n\tPlease wait a moment!");
                                Thread.Sleep(3000);
                                goto Menu;

                            #endregion

                            default:
                                goto ChangeRole;
                        }
                    FinishChangeRole:
                        dbChangeRole.SaveChanges();
                        Console.Write("Press any key to return to menu.");
                        Console.ReadKey();
                        goto Menu;
                    }

                #endregion

                #region Case 18: Change color background
                case 18:
                ColorBackground:
                    PrintDatetimeAndUserStatistic();
                    Console.WriteLine("\t\t1.Red\n\t\t2.Green\n\t\t3.Yellow\n\t\t4.Blue\n\t\t5.Cyan\n\t\t6.Magenta\n\t\t7.Black8.\n\t\tExit this menu");

                    Console.Write("Which color would you like to be your bakcground!\n\tAnswer: ");

                    int backGroundColorNum;
                    while (!int.TryParse(Console.ReadLine(), out backGroundColorNum))
                    {
                        PrintJustNumbers("Background Color menu");
                        Console.WriteLine("\t\t1.Red\n\t\t2.Green\n\t\t3.Yellow\n\t\t4.Blue\n\t\t5.Cyan\n\t\t6.Magenta\n\t\t7.Black");
                        Console.Write("Which color would you like to be your bakcground!\n\tAnswer: ");
                    }
                    switch (backGroundColorNum)
                    {
                        case 1:
                            
                                PrintDatetimeAndUserStatistic();
                                Console.WriteLine("Background will change into: Red\n\tPlease wait a moment!");
                                Thread.Sleep(3000);
                                Console.BackgroundColor = ConsoleColor.DarkRed;
                                Console.Clear();
                            goto FinishColorBackground;


                        case 2:
                            
                            
                                PrintDatetimeAndUserStatistic();
                                Console.WriteLine("Background will change into: Green\n\tPlease wait a moment!");
                                Thread.Sleep(3000);
                                Console.BackgroundColor = ConsoleColor.DarkGreen;
                                Console.Clear();
                                goto FinishColorBackground;


                        case 3:
                            
                            
                                PrintDatetimeAndUserStatistic();
                                Console.WriteLine("Background will change into: Yellow\n\tPlease wait a moment!");
                                Thread.Sleep(3000);
                                Console.BackgroundColor = ConsoleColor.DarkYellow;
                                Console.Clear();
                            goto FinishColorBackground;


                        case 4:
                            
                            
                                PrintDatetimeAndUserStatistic();
                                Console.WriteLine("Background will change into: Blue\n\tPlease wait a moment!");
                                Thread.Sleep(3000);
                                Console.BackgroundColor = ConsoleColor.DarkBlue;
                                Console.Clear();
                            goto FinishColorBackground;


                        case 5:
                            
                            
                                PrintDatetimeAndUserStatistic();
                                Console.WriteLine("Background will change into: Cyan\n\tPlease wait a moment!");
                                Thread.Sleep(3000);
                                Console.BackgroundColor = ConsoleColor.DarkCyan;
                                Console.Clear();
                            goto FinishColorBackground;



                        case 6:
                            
                            
                                PrintDatetimeAndUserStatistic();
                                Console.WriteLine("Background will change into: Magenta\n\tPlease wait a moment!");
                                Thread.Sleep(3000);
                                Console.BackgroundColor = ConsoleColor.DarkMagenta;
                                Console.Clear();
                            goto FinishColorBackground;

                        case 7:
                            
                            
                                PrintDatetimeAndUserStatistic();
                                Console.WriteLine("Background will change into: Black\n\tPlease wait a moment!");
                                Thread.Sleep(3000);
                                Console.BackgroundColor = ConsoleColor.Black;
                                Console.Clear();
                            goto FinishColorBackground;

                        case 8:

                                PrintDatetimeAndUserStatistic();
                                Console.WriteLine("Going back to main menu.\n\tPlease wait a moment!");
                                Thread.Sleep(3000);
                                goto Menu;
                            

                        default:
                            goto ColorBackground;
                    }
                    FinishColorBackground:
                    PrintDatetimeAndUserStatistic();
                    Console.Write("Press any key to return to menu.");
                    Console.ReadKey();
                    goto Menu;
                #endregion

                #region Case 19: Exit Login Page

                case 19:
                    PrintDatetimeAndUserStatistic();
                    Console.WriteLine($"\n\tHave a good day {adminName}");
                    Thread.Sleep(3000);
                    Console.WriteLine("Wish to see you again.\n\tBye.");
                    Thread.Sleep(3000);
                    goto LoginPage;

                    #endregion

            }
            #endregion

        }






    }
}
