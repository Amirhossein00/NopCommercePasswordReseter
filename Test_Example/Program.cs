using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Test_Example
{
    class Constants
    {
        public const string Passssssword = "amir!=amir0059644";

        public const string HashFormat = "SHA512";

        public const int SaltKeySize = 5;
    }
    class Program
    {


        #region Utilities
        public static void ResetColor()
        {
            Console.ResetColor();
        }
        #endregion

        static void Main(string[] args)
        {
            ConsoleColor.Blue.WriteLineWithCurrentColor("wellcome !!!!!!!!!!!!!!!!!!!!");

            bool login = false;
            while (!login)
            {
                ConsoleColor.DarkYellow.WriteLineWithCurrentColor("Enter password !");
                var answer = Console.ReadLine();
                if (answer.Equals(Constants.Passssssword))
                {
                    login = true;
                }
            }
            while (true)
            {

                ConsoleColor.DarkYellow.WriteLineWithCurrentColor("Insert your choosen password :");
                var password = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(password))
                {
                    ConsoleColor.Red.WriteLineWithCurrentColor("Password was not valid");
                    Console.ReadKey();
                    continue;
                }
                var saltKey = CreateSalt(Constants.SaltKeySize);
                var concat = string.Concat(password, saltKey);
                var passByteArray = Encoding.UTF8.GetBytes(concat);

                var algorithm = (HashAlgorithm)CryptoConfig.CreateFromName(Constants.HashFormat);
                var result = BitConverter.ToString(algorithm.ComputeHash(passByteArray)).Replace("-", string.Empty);
                ConsoleColor.Green.UpdateColor();
                Console.WriteLine("Password generated successfully ");
                Console.WriteLine("now if you want to use this application to update password press \"Y\": in other case press enter");
                ResetColor();
                var answer = Console.ReadLine().ToLower();
                if (answer.Equals("y"))
                {
                    //Get sql server name
                    Console.WriteLine("Enter required values =>");
                    ConsoleColor.DarkYellow.WriteLineWithCurrentColor("SqlServer name(include ip if its not on localhost):");
                    var serverName = Console.ReadLine();

                    //Get database name
                    ConsoleColor.DarkYellow.UpdateColor();
                    Console.WriteLine("Database:");
                    ResetColor();
                    var databaseName = Console.ReadLine();

                    //Define Integrated security
                    ConsoleColor.DarkYellow.WriteLineWithCurrentColor("If connection string include passwrod press \"Y\" :");
                    answer = Console.ReadLine().ToLower();

                    var loginAccountUserId = string.Empty;
                    var loginAccountPassword = string.Empty;
                    if (answer.Equals("y"))
                    {
                        //Get login info user id
                        ConsoleColor.DarkYellow.WriteLineWithCurrentColor("Login info - UserId:");
                        loginAccountUserId = Console.ReadLine();

                        //Get Login info password
                        ConsoleColor.DarkYellow.WriteLineWithCurrentColor("Login info - Password:");
                        loginAccountPassword = Console.ReadLine();

                    }
                    //Get user id
                    ConsoleColor.DarkYellow.WriteLineWithCurrentColor("User id (leave it empty if you want update admin user):");
                    var userId = Console.ReadLine();
                    ConsoleColor.Blue.WriteLineWithCurrentColor("Loading . . . . . .");
                    var dataProvider = new DataProvider(serverName, databaseName, userId: loginAccountUserId, password: loginAccountUserId);
                    if (string.IsNullOrWhiteSpace(userId))
                    {
                        dataProvider.UpdateAdminPassword(result, saltKey, out string errorMessage);

                        if (!string.IsNullOrEmpty(errorMessage))
                        {
                            ConsoleColor.DarkYellow.UpdateColor();
                            Console.Write("Error message: ");
                            ConsoleColor.Red.UpdateColor();
                            Console.WriteLine(errorMessage);
                            ResetColor();
                        }
                    }
                    else
                    {
                        dataProvider.UpdateUserPasswordByUserId(result, saltKey, Convert.ToInt32(userId), out string errorMessage);
                        if (!string.IsNullOrWhiteSpace(errorMessage))
                        {
                            ConsoleColor.DarkYellow.UpdateColor();
                            Console.Write("Error message: ");
                            ConsoleColor.Red.UpdateColor();
                            Console.WriteLine(errorMessage);
                            ResetColor();
                        }
                    }
                }
                Console.WriteLine(string.Empty);
                WritePassword(password + "\n" + result + "\n" + saltKey);
                ConsoleColor.Green.WriteLineWithCurrentColor("Password information updated in note !");
                Console.ReadKey();
            }
        }

        public static void WritePassword(string password)
        {
            var fileName = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory).GetBackToRootPath() + "generatedPassword.txt";
            File.WriteAllText(fileName, password);
        }

        public static string CreateSalt(int size)
        {
            using (var provider = new RNGCryptoServiceProvider())
            {
                var buff = new byte[size];
                provider.GetBytes(buff);

                return Convert.ToBase64String(buff);
            }
        }
    }
}
