using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace LightSpeed.Quiz
{
    public class Program
    {
        private static QuizRoot _root;
        private static readonly Random _random = new Random();
        private static int _totalQuestions;
        private static int _questionTotal = -1;
        
        private static readonly List<QuizRound> Rounds = new List<QuizRound>(); 
        
        private static int _round = 1;
        private static string _input;
        
        public static void Main(string[] args)
        {
            _root = MakeObject();

            if (_root == null)
            {
                Console.WriteLine("Enter any key to terminate program.");
                Console.ReadLine();
                return;
            }

            while (true)
            {
                RunQuiz();

                if (Exiting(_input))
                    break;

                _round++;

                Console.WriteLine($"\nPress y or n to take the test again.");
                _input = Console.ReadLine();

                if (Exiting(_input) || _input.ToLower() == "n")
                    break;
            }            
        }

        private static QuizItem GetRandomQuizItem(QuizRound round, out int index)
        {
            index = -1;
            if (_totalQuestions <= round.Answers.Count())
            {
                return null;
            }

            var props = _root.Quiz.GetType().GetProperties();

            var val = _random.Next(0, _totalQuestions);

            var temp = round.Answers.SingleOrDefault(x => x.QuestionIndex == val);

            if (temp == null)
            {
                round.Answers.Add(new Answer {QuestionIndex = val});
            }
            else
            {
                while (round.Answers.FirstOrDefault(x => x.QuestionIndex == val) != null)
                {
                    val = _random.Next(0, _totalQuestions);
                }

                round.Answers.Add(new Answer { QuestionIndex = val });
            }
            
            var propInfo = props[val];
            var item = propInfo.GetValue(_root.Quiz, null) as QuizItem;
            if (item != null)
            {
                _questionTotal = item.Options.Count();
            }

            index = val;
            return item;
        }

        private static void RunQuiz()
        {
            _totalQuestions = _root.Quiz.GetType().GetProperties().Count();

            Console.WriteLine(" ** Please enter a numeric value for each question.\n**" +
                    "Enter quit or exit to stop the quiz at any time.\n\n");

            var round = new QuizRound {Index = _round, Answers = new List<Answer>() };
            
            while (true)
            {
                int index;
                var item = GetRandomQuizItem(round, out index);

                if (item == null)
                    break;
                
                Console.WriteLine($"\n{item.Question}");
                for (var i = 1; i <= item.Options.Count; i++)
                {
                    Console.WriteLine($"{i}. {item.Options[i - 1]}");
                }

                _input = Console.ReadLine();
                if (Exiting(_input))
                    break;

                int value;
                while ((!int.TryParse(_input, out value) || value < 1 || value > item.Options.Count) && !Exiting(_input))
                {
                    Console.WriteLine($"Enter numeric value (1-{item.Options.Count})");
                    _input = Console.ReadLine();

                    if (Exiting(_input))
                        break;
                }

                if (Exiting(_input))
                    break;

                var answer = round.Answers.SingleOrDefault(x => x.QuestionIndex == index);
                if (answer != null)
                {
                    answer.Actual = item.Answer;
                    answer.Chosen = value;
                }              
            }

            var correct = round.Answers.Count(answer => answer.Actual == answer.Chosen);

            var credit = 100/_totalQuestions;
            round.Score = credit*correct;
           
            Console.WriteLine($"\nCurrent score: {round.Score}\n");

            var prevScores = GetPreviousScores();
            
            if (prevScores.Any())
            {
                Console.WriteLine($"Previous scores: {string.Join(", ", prevScores)}");
            }
            
            Rounds.Add(round);
        }

        private static List<float> GetPreviousScores()
        {
            return Rounds.Select(quizRound => quizRound.Score).ToList();
        }

        private static bool Exiting(string input)
        {
            return input.ToLower() == "quit" || input.ToLower() == "exit";
        }

        private static QuizRoot MakeObject()
        {
            string json;

            try
            {
                using (
                var reader = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + "quiz.js"))
                {
                    json = reader.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error encountered reading quiz.js: {ex.Message}");
                return null;
            }

            var obj = JsonConvert.DeserializeObject<QuizRoot>(json);
            return obj;
        }
    }
}
