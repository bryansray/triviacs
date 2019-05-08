using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using CsvHelper;
using CsvHelper.Configuration;
using Npgsql;

namespace Trivia.Loader
{
    class Program
    {
        static void Main(string[] args)
        {
            const string ConnectionString = @"";
            for (var fileNumber = 3; fileNumber <= 7; fileNumber++)
            {
                using (var reader = new StreamReader($"./data/Database/{fileNumber} Answers-Table 1.csv"))
                {
                    using (var csv = new CsvReader(reader))
                    {
                        csv.Configuration.RegisterClassMap<QuestionMap>();
                        csv.Configuration.HeaderValidated = null;
                        csv.Configuration.MissingFieldFound = null;

                        var records = csv.GetRecords<QuestionRecord>();

                        using (var connection = new NpgsqlConnection(
                            ConnectionString)
                        )
                        {
                            connection.Open();

                            foreach (var record in records)
                            {
                                Console.Write(".");
//                            Console.WriteLine($"{record.Question} - {record.Answer1} ({record.Total1})");

                                using (var command = new NpgsqlCommand(
                                    "INSERT INTO \"Questions\" (\"Text\") VALUES (@Text) RETURNING \"Id\";",
                                    connection))
                                {
                                    var param1 = new NpgsqlParameter<string>("Text", record.Question);
                                    command.Parameters.Add(param1);

                                    var id = (int) command.ExecuteScalar();

                                    var commandText =
                                        @"INSERT INTO ""Answers"" (""QuestionId"", ""Text"", ""Count"") VALUES (@QuestionId, @Answer1, @Count1);
                                        INSERT INTO ""Answers"" (""QuestionId"", ""Text"", ""Count"") VALUES (@QuestionId, @Answer2, @Count2);
                                        INSERT INTO ""Answers"" (""QuestionId"", ""Text"", ""Count"") VALUES (@QuestionId, @Answer3, @Count3);";

                                    for (var i = 4; i < 7; i++)
                                    {
                                        if (i == 4 && record.Total4.HasValue)
                                        {
                                            commandText +=
                                                @"INSERT INTO ""Answers"" (""QuestionId"", ""Text"", ""Count"") VALUES (@QuestionId, @Answer4, @Count4);";
                                        }

                                        if (i == 5 && record.Total5.HasValue)
                                        {
                                            commandText +=
                                                @"INSERT INTO ""Answers"" (""QuestionId"", ""Text"", ""Count"") VALUES (@QuestionId, @Answer5, @Count5);";
                                        }

                                        if (i == 6 && record.Total6.HasValue)
                                        {
                                            commandText +=
                                                @"INSERT INTO ""Answers"" (""QuestionId"", ""Text"", ""Count"") VALUES (@QuestionId, @Answer6, @Count6);";
                                        }

                                        if (i == 7 && record.Total7.HasValue)
                                        {
                                            commandText +=
                                                @"INSERT INTO ""Answers"" (""QuestionId"", ""Text"", ""Count"") VALUES (@QuestionId, @Answer7, @Count7);";
                                        }
                                    }


                                    using (var answerCommand = new NpgsqlCommand(commandText, connection))
                                    {
                                        answerCommand.Parameters.Add(new NpgsqlParameter<int>("QuestionId", id));
                                        answerCommand.Parameters.Add(
                                            new NpgsqlParameter<string>("Answer1", record.Answer1));
                                        answerCommand.Parameters.Add(new NpgsqlParameter<int>("Count1", record.Total1));

                                        answerCommand.Parameters.Add(
                                            new NpgsqlParameter<string>("Answer2", record.Answer2));
                                        answerCommand.Parameters.Add(new NpgsqlParameter<int>("Count2", record.Total2));

                                        answerCommand.Parameters.Add(
                                            new NpgsqlParameter<string>("Answer3", record.Answer3));
                                        answerCommand.Parameters.Add(new NpgsqlParameter<int>("Count3", record.Total3));

                                        if (record.Total4.HasValue)
                                        {
                                            answerCommand.Parameters.Add(
                                                new NpgsqlParameter<string>("Answer4", record.Answer4));
                                            answerCommand.Parameters.Add(
                                                new NpgsqlParameter<int>("Count4", record.Total4.Value));
                                        }

                                        if (record.Total5.HasValue)
                                        {
                                            answerCommand.Parameters.Add(
                                                new NpgsqlParameter<string>("Answer5", record.Answer5));
                                            answerCommand.Parameters.Add(
                                                new NpgsqlParameter<int>("Count5", record.Total5.Value));
                                        }

                                        if (record.Total6.HasValue)
                                        {
                                            answerCommand.Parameters.Add(
                                                new NpgsqlParameter<string>("Answer6", record.Answer6));
                                            answerCommand.Parameters.Add(
                                                new NpgsqlParameter<int>("Count6", record.Total6.Value));
                                        }

                                        if (record.Total7.HasValue)
                                        {
                                            answerCommand.Parameters.Add(
                                                new NpgsqlParameter<string>("Answer7", record.Answer7));
                                            answerCommand.Parameters.Add(
                                                new NpgsqlParameter<int>("Count7", record.Total7.Value));
                                        }

                                        var rowsAffected = answerCommand.ExecuteNonQuery();
//                                    Console.ReadLine();
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    public class QuestionRecord
    {
        public string Question { get; set; }

        public string Answer1 { get; set; }

        public int Total1 { get; set; }

        public string Answer2 { get; set; }

        public int Total2 { get; set; }

        public string Answer3 { get; set; }

        public int Total3 { get; set; }

        public string Answer4 { get; set; }

        public int? Total4 { get; set; }

        public string Answer5 { get; set; }

        public int? Total5 { get; set; }

        public string Answer6 { get; set; }

        public int? Total6 { get; set; }

        public string Answer7 { get; set; }

        public int? Total7 { get; set; }
    }

    public class QuestionMap : ClassMap<QuestionRecord>
    {
        public QuestionMap()
        {
            Map(x => x.Question).Name("Question");

            Map(x => x.Answer1).Name("Answer 1");
            Map(x => x.Total1).Name("#1");

            Map(x => x.Answer2).Name("Answer 2");
            Map(x => x.Total2).Name("#2");

            Map(x => x.Answer3).Name("Answer 3");
            Map(x => x.Total3).Name("#3");

            Map(x => x.Answer4).Name("Answer 4");
            Map(x => x.Total4).Name("#4");

            Map(x => x.Answer5).Name("Answer 5");
            Map(x => x.Total5).Name("#5");

            Map(x => x.Answer6).Name("Answer 6");
            Map(x => x.Total6).Name("#6");

            Map(x => x.Answer7).Name("Answer 7");
            Map(x => x.Total7).Name("#7");
        }
    }
}