using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finale_Project_CSharp
{
    class ExtraClass
    {
         #region Case 1: Change Role Of Employee 
//                            case 1:
//                                ChangeEmployeeRole:
//                                PrintDatetimeAndUserStatistic();
//        Console.WriteLine("\nChoose a master to change the role.\n");
//                                List<Master> viewMasterListInChangeRole = dbChangeRole.Masters.ToList();

//                                foreach (Master master in viewMasterListInChangeRole)
//                                {
//                                    Console.WriteLine($"Id:{master.Id}" +
//                                                      $"\tName:{master.Name}" +
//                                                      $"\tLastName:{master.LastName}" +
//                                                      $"\tPhonenumber:{master.Phonenumber}\n" +
//                                                      $"FieldOfStudy:{master.FieldOfStudy}" +
//                                                      $"\tSalary:{master.Salary}" +
//                                                      $"\tRegisterdate:{master.Registerdate}" +
//                                                      $"\tActive:{master.IsActive}");
//                                    Console.WriteLine("--------------------------------------------------------------------------------");
//                                }
//    Console.Write("\nMaster id: ");
//                                int answerSelectMasterChangeRple;
//                                while (!int.TryParse(Console.ReadLine(), out answerSelectMasterChangeRple))
//                                {
//                                    PrintJustNumbers("Master Id!");
//    PrintDatetimeAndUserStatistic();
//                                    foreach (Master master in viewMasterListInChangeRole)
//                                    {
//                                        Console.WriteLine($"Id:{master.Id}" +
//                                                          $"\tName:{master.Name}" +
//                                                          $"\tLastName:{master.LastName}" +
//                                                          $"\tPhonenumber:{master.Phonenumber}\n" +
//                                                          $"FieldOfStudy:{master.FieldOfStudy}" +
//                                                          $"\tSalary:{master.Salary}" +
//                                                          $"\tRegisterdate:{master.Registerdate}" +
//                                                          $"\tActive:{master.IsActive}");
//                                        Console.WriteLine("--------------------------------------------------------------------------------");
//                                    }
//Console.Write("\nMaster id: ");
//                                }
//                                Master selectMasterChangeRole = dbChangeRole.Masters.Find(answerSelectMasterChangeRple);
//if (selectMasterChangeRole != null)
//{
//    goto SelectForeignObjectChangeRoleMaster;
//}
//else
//{
//    PrintDatetimeAndUserStatistic();
//    IdNotFound("Master list!", answerSelectMasterChangeRple);
//    goto ChangeMasterRole;
//}
//SelectForeignObjectChangeRoleMaster:
//PrintDatetimeAndUserStatistic();
//Console.WriteLine($"\nPlease select another role for: {selectMasterChangeRole.Name} {selectMasterChangeRole.LastName}\n");
//Console.WriteLine("\t\t1.Employee" +
//                  "\n\t\t2.Student");
//Console.WriteLine("\nAnswer: ");
//int answerSelectRoleForMaster;
//while (!int.TryParse(Console.ReadLine(), out answerSelectRoleForMaster))
//{
//    PrintJustNumbers("Use only items of the list!");
//    PrintDatetimeAndUserStatistic();
//    Console.WriteLine($"\nPlease select another role for: {selectMasterChangeRole.Name} {selectMasterChangeRole.LastName}\n");
//    Console.WriteLine("\t\t1.Employee" +
//                      "\n\t\t2.Student");
//    Console.WriteLine("\nAnswer: ");
//}
////    PrintDatetimeAndUserStatistic();
////    Console.WriteLine("\nChoose an employee to change the role.\n");
////    List<Employee> viewEmployeeListInChangeRole = dbChangeRole.Employees.ToList();
////    foreach (Employee employee in viewEmployeeListInChangeRole)
////    {
////        Console.WriteLine($"Id:{employee.Id}" +
////                          $"\tName:{employee.Name}" +
////                          $"\tLast Name:{employee.LastName}" +
////                          $"\tDepartment:{employee.DepartmentName}\n" +
////                          $"Phonenumber:{employee.Phonenumber}" +
////                          $"\tSalary:{employee.Salary}" +
////                          $"\tRegisterdate:{employee.Registerdate}" +
////                          $"\tActive:{employee.IsActive}");
////        Console.WriteLine("--------------------------------------------------------------------------------");
////    }
////    Console.Write("\nEmployee id: ");
////    int answerSelectEmployeeChangeRole;
////    while (!int.TryParse(Console.ReadLine(),out answerSelectEmployeeChangeRole))
////    {
////        PrintJustNumbers("Employee Id!");
////        PrintDatetimeAndUserStatistic();
////        foreach (Employee employee in viewEmployeeListInChangeRole)
////        {
////            Console.WriteLine($"Id:{employee.Id}" +
////                              $"\tName:{employee.Name}" +
////                              $"\tLast Name:{employee.LastName}" +
////                              $"\tDepartment:{employee.DepartmentName}\n" +
////                              $"Phonenumber:{employee.Phonenumber}" +
////                              $"\tSalary:{employee.Salary}" +
////                              $"\tRegisterdate:{employee.Registerdate}" +
////                              $"\tActive:{employee.IsActive}");
////            Console.WriteLine("--------------------------------------------------------------------------------");
////        }
////        Console.Write("\nEmployee id: ");
////    }
////    var selectEmployeeChangeRole = dbChangeRole.Employees.Find(answerSelectEmployeeChangeRole);
////    if (selectEmployeeChangeRole != null)
////    {
////        goto SelectForeignObjectChangeRole;
////    }
////    else
////    {
////        PrintDatetimeAndUserStatistic();
////        IdNotFound("Employee list!",answerSelectEmployeeChangeRole);
////        goto ChangeEmployeeRole;
////    }
////SelectForeignObjectChangeRole:
////    PrintDatetimeAndUserStatistic();
////    Console.WriteLine();
////    Console.WriteLine($"\nPlease select another role for: {selectEmployeeChangeRole.Name} {selectEmployeeChangeRole.LastName}\n");
////    Console.WriteLine("\t\t1.Master" +
////                      "\n\t\t2.Student");
////    Console.WriteLine("\nAnswer: ");
////    int answerSelectRoleForEmployee;
////    while (!int.TryParse(Console.ReadLine(),out answerSelectRoleForEmployee))
////    {
////        PrintJustNumbers("Use only items of the list!");
////        PrintDatetimeAndUserStatistic();
////        Console.WriteLine($"\nPlease select another role for: {selectEmployeeChangeRole.Name} {selectEmployeeChangeRole.LastName}\n");
////        Console.WriteLine("\t\t1.Master" +
////                          "\n\t\t2.Student");
////        Console.WriteLine("\nAnswer: ");
////    }
//switch (answerSelectRoleForEmployee)
//{
//    //Change to Master
//    case 1:

//        dbChangeRole.Masters.Add(new Master("", selectEmployeeChangeRole.Salary, selectEmployeeChangeRole.Name, selectEmployeeChangeRole.LastName, selectEmployeeChangeRole.Phonenumber, selectEmployeeChangeRole.Password, dbChangeRole.Roles.Find(3), 0));
//        dbChangeRole.Employees.Remove(selectEmployeeChangeRole);
//        PrintDatetimeAndUserStatistic();
//        Console.WriteLine($"\n\t{selectEmployeeChangeRole.Name} {selectEmployeeChangeRole.LastName} => Role has changed to (Master)!");
//        goto FinishChangeRole;

//    //Change to Student 
//    case 2:

//        dbChangeRole.Students.Add(new Student("", selectEmployeeChangeRole.Name, selectEmployeeChangeRole.LastName, selectEmployeeChangeRole.Phonenumber, selectEmployeeChangeRole.Password, dbChangeRole.Roles.Find(4), 0));
//        dbChangeRole.Employees.Remove(selectEmployeeChangeRole);
//        PrintDatetimeAndUserStatistic();
//        Console.WriteLine($"\n\t{selectEmployeeChangeRole.Name} {selectEmployeeChangeRole.LastName} => Role has changed to (Student)!");
//        goto FinishChangeRole;

//    default:
//        goto SelectForeignObjectChangeRole;
 
                            #endregion
    }
}
