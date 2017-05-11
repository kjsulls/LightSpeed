using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightSpeed.Quiz
{
    public class Quiz
    {
        public QuizItem Q1 { get; set; }
        public QuizItem Q2 { get; set; }
        public QuizItem Q3 { get; set; }
        public QuizItem Q4 { get; set; }
        public QuizItem Q5 { get; set; }
    }

    public class QuizItem
    {
        public string Question { get; set; }
        public List<string> Options { get; set; }
        public int Answer { get; set; }
    }

    public class QuizRoot
    {
        public Quiz Quiz { get; set; }
    }


}
