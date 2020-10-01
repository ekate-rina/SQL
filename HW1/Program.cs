using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Globalization;

namespace hm1_1
{
    class Program
    {
        static string connectionString = "Server=(localdb)\\mssqllocaldb;Database=Minions;Trusted_Connection=True";
        static void Main(string[] args)
        {
            //Select2_2();

            //for Select2_3
            //int id = Int32.Parse(Console.ReadLine());

            //for Insert2_4
            /*string minionStr = Console.ReadLine();
            string[] minionArr = minionStr.Split(" ");
            string villainName = Console.ReadLine();
            
            Insert2_4(minionArr[0], Int32.Parse(minionArr[1]), minionArr[2], villainName);*/

            //for Delete2_5
            /*int id = Int32.Parse(Console.ReadLine());
            Delete2_5(id);*/

            //for Update2_6
            string minionId = Console.ReadLine();
            int[] minionIdArr = Array.ConvertAll(minionId.Split(" "), int.Parse);
            for (int i = 0; i < minionIdArr.Length; i++)
            {
                Update2_6(minionIdArr[i]);
            }
            Select2_6();

        }
        static void Select2_2()
        {
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            using (connection)
            {
                string selectionCommandString = "SELECT Villains.name, COUNT(Minions.Name) FROM Villains " +
                    "LEFT JOIN MinionsVillains " +
                    "ON Villains.Id = MinionsVillains.VillainId " +
                    "LEFT JOIN Minions " +
                    "ON MinionsVillains.MinionId = Minions.Id " +
                    "GROUP BY Villains.name " +
                    "HAVING COUNT(Minions.name) > 3" +
                    "ORDER BY COUNT(Minions.name) DESC";
                SqlCommand command = new SqlCommand(selectionCommandString, connection);
                SqlDataReader reader = command.ExecuteReader();
                using (reader)
                {
                    while (reader.Read())
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            Console.Write($"{reader[i]}");
                        }
                        Console.WriteLine();

                    }
                }
            }
        }

        static void Select2_3(int id)
        {
            string selectionVillainsCommandString = $"SELECT Villains.Name FROM Villains " +
                $"WHERE Villains.Id = @Id ";
            SqlConnection connectionVillains = new SqlConnection(connectionString);
            SqlCommand commandVillains = new SqlCommand(selectionVillainsCommandString, connectionVillains);
            SqlParameter parameterVillains = new SqlParameter("@Id", SqlDbType.Int) { Value = id };
            commandVillains.Parameters.Add(parameterVillains);

            string selectionMinionsCommandString = $"SELECT Minions.Name, Minions.Age FROM Villains " +
                $"LEFT JOIN MinionsVillains " +
                $"ON Villains.Id = MinionsVillains.VillainId " +
                $"LEFT JOIN Minions " +
                $"ON MinionsVillains.MinionId = Minions.Id  " +
                $"WHERE Villains.Id = @Id " +
                $"ORDER BY Minions.Name";
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand command = new SqlCommand(selectionMinionsCommandString, connection);
            SqlParameter parameter = new SqlParameter("@id", SqlDbType.Int) { Value = id };
            command.Parameters.Add(parameter);

            connection.Open();
            connectionVillains.Open();
            using (connectionVillains)
            {
                SqlDataReader reader = commandVillains.ExecuteReader();
                using (reader)
                {
                    if (!reader.HasRows)
                    {
                        Console.WriteLine($"No villain with ID {id} exists in the database");
                    }
                    while (reader.Read())
                    {
                        Console.WriteLine($"Villain: {reader.GetSqlValue(0)} ");
                    }
                }
            }
            using (connection)
            {
                SqlDataReader reader = command.ExecuteReader();
                using (reader)
                {
                    while (reader.Read())

                    {
                        if (reader.IsDBNull(0))
                        {
                            Console.WriteLine("(no minions)");
                        }

                        else
                        {
                            Console.WriteLine($"{reader.GetSqlValue(0)} {reader.GetSqlValue(1)} ");

                        }

                    }
                }
            }
        }


        static void Insert2_4(string minionName, int age, string town, string villainName)
        {
            string selectionCommandString = $"SELECT Name FROM Towns WHERE Name = @town";
            SqlConnection connectionTown = new SqlConnection(connectionString);
            SqlCommand command = new SqlCommand(selectionCommandString, connectionTown);
            SqlParameter parameter = new SqlParameter("@town", SqlDbType.NVarChar, 50) { Value = town };
            command.Parameters.Add(parameter);

            bool flagTown = true; ;
            connectionTown.Open();
            using (connectionTown)
            {
                SqlDataReader reader = command.ExecuteReader();
                using (reader)
                {
                    if (reader.HasRows)
                    {
                        flagTown = false;
                    }
                    else
                    {
                        Console.WriteLine($"Город {town} был добавлен в базу данных.");
                    }
                }
            }
            if (flagTown == true)
            {
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                using (connection)
                {
                    SqlCommand cmd = new SqlCommand(
                  "INSERT INTO Towns " +
                  "(Name) VALUES " +
                  "(@town)", connection);

                    cmd.Parameters.AddWithValue("@town", town);

                    cmd.ExecuteNonQuery();
                }
            }
            string selectVillainCmdString = $"SELECT Name FROM Villains WHERE Name = @name";
            SqlConnection connectionVillain = new SqlConnection(connectionString);
            SqlCommand commandVillain = new SqlCommand(selectVillainCmdString, connectionVillain);
            SqlParameter parameterVillain = new SqlParameter("@name", SqlDbType.NVarChar, 50) { Value = villainName };
            commandVillain.Parameters.Add(parameterVillain);

            bool flagVillain = true; ;
            connectionVillain.Open();
            using (connectionVillain)
            {
                SqlDataReader reader = commandVillain.ExecuteReader();
                using (reader)
                {
                    if (reader.HasRows)
                    {
                        flagVillain = false;
                    }
                    else
                    {
                        Console.WriteLine($"Злодей {villainName} был добавлен в базу данных.");
                    }
                }
            }
            if (flagVillain == true)
            {
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                using (connection)
                {
                    SqlCommand cmd = new SqlCommand(
                  "INSERT INTO Villains " +
                  "(Name, EvilnessFactorId) VALUES " +
                  "(@name, @evilnessFactorId)", connection);

                    cmd.Parameters.AddWithValue("@name", villainName);
                    cmd.Parameters.AddWithValue("@evilnessFactorId", 4);

                    cmd.ExecuteNonQuery();
                }

            }
            SqlConnection connectionMinion = new SqlConnection(connectionString);
            connectionMinion.Open();
            using (connectionMinion)
            {
                SqlCommand cmdMinion = new SqlCommand(
                   "INSERT INTO Minions (Name, Age, TownId) " +
                   "SELECT @name, @age, (SELECT Towns.Id FROM Towns WHERE Towns.Name = @town)", connectionMinion);

                cmdMinion.Parameters.AddWithValue("@name", minionName);
                cmdMinion.Parameters.AddWithValue("@age", age);
                cmdMinion.Parameters.AddWithValue("@town", town);

                cmdMinion.ExecuteNonQuery();
                Console.WriteLine($"Успешно добавлен {minionName}, чтобы быть миньоном {villainName}.");
            }
            SqlConnection connectionMinionsVillains = new SqlConnection(connectionString);
            connectionMinionsVillains.Open();
            using (connectionMinionsVillains)
            {
                SqlCommand cmdMinionVillains = new SqlCommand(
                   "INSERT INTO MinionsVillains (MinionId, VillainId) " +
                   "SELECT (SELECT Minions.Id  FROM Minions WHERE Minions.Name = @minionName) , " +
                   "(SELECT Villains.Id FROM Villains WHERE Villains.Name = @villainName)", connectionMinionsVillains);

                cmdMinionVillains.Parameters.AddWithValue("@minionName", minionName);
                cmdMinionVillains.Parameters.AddWithValue("@villainName", villainName);

                cmdMinionVillains.ExecuteNonQuery();

            }
        }
        static void Delete2_5(int id)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            using (connection)
            {
                string selectionCommandString = "SELECT Villains.name, COUNT(Minions.Name) FROM Villains " +
                    "LEFT JOIN MinionsVillains " +
                    "ON Villains.Id = MinionsVillains.VillainId " +
                    "LEFT JOIN Minions " +
                    "ON MinionsVillains.MinionId = Minions.Id " +
                    "WHERE Villains.Id = @id " +
                    "GROUP BY Villains.Name ";
                SqlCommand command = new SqlCommand(selectionCommandString, connection);
                command.Parameters.AddWithValue("@id", id);
                SqlDataReader reader = command.ExecuteReader();
                using (reader)
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine($"{reader.GetString(0)} был удален. \n{reader.GetValue(1)} миньонов было освобождено.");
                        }
                        SqlConnection connectionDlt = new SqlConnection(connectionString);
                        connectionDlt.Open();
                        using (connectionDlt)
                        {
                            SqlCommand cmd = new SqlCommand(
                               "DELETE FROM MinionsVillains WHERE MinionsVillains.VillainId = @id", connectionDlt);

                            cmd.Parameters.AddWithValue("@id", id);

                            cmd.ExecuteNonQuery();

                        }

                        SqlConnection connectionDelete = new SqlConnection(connectionString);
                        connectionDelete.Open();
                        using (connectionDelete)
                        {
                            SqlCommand cmd = new SqlCommand(
                               "DELETE FROM Villains WHERE Villains.Id = @id", connectionDelete);

                            cmd.Parameters.AddWithValue("@id", id);

                            cmd.ExecuteNonQuery();

                        }
                    }

                    else
                    {
                        Console.WriteLine("Такой злодей не найден.");
                    }

                }

            }

        }
        static void Update2_6(int id)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            using (connection)
            {
                SqlCommand cmd = new SqlCommand(
                   "UPDATE Minions SET Age = Age + 1 WHERE Id = @id", connection);

                cmd.Parameters.AddWithValue("@id", id);

                cmd.ExecuteNonQuery();

            }



        }
        static void Select2_6()
        {
            SqlConnection connectionSelect = new SqlConnection(connectionString);
            connectionSelect.Open();
            using (connectionSelect)
            {
                SqlCommand command = new SqlCommand(
                   "SELECT * FROM Minions", connectionSelect);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {

                    Console.Write($"{reader[1]}  {reader[2]}");
                    Console.WriteLine();

                }

            }
        }
    }

}

