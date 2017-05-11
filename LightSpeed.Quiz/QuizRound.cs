using System.Collections.Generic;

namespace LightSpeed.Quiz
{
    public class QuizRound
    {
        public int Index { get; set; }
        public List<Answer> Answers { get; set; }

        public float Score { get; set; }
    }

    public class Answer
    {
        public int QuestionIndex { get; set; }
        public int Actual { get; set; }
        public int Chosen { get; set; }

        public Answer()
        {
            QuestionIndex = -1;
        }
    }

}
