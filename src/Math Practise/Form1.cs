using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Math_Practise
{
    public partial class Form1 : Form
    {
        List<Question> questions = new List<Question>();
        List<Question.MathSign> AllowedSigns = new List<Question.MathSign> { Question.MathSign.Plus };
        int correctCounter, incorrectCounter;

        public Form1()
        {
            InitializeComponent();
            SetupQuestion();
            SetSummary();
        }

        private void SetupQuestion()
        {
            var question = GetNextQuestion();
            SetQuestion(question);
        }

        private void SetQuestion(Question question)
        {
            lblFirst.Text = question.First.ToString();
            lblSecond.Text = question.Second.ToString();
            switch (question.Sign)
            {
                case Question.MathSign.Plus:
                    lblSign.Text = "+";
                    question.ExpectedAnswer = question.First + question.Second;
                    break;
                case Question.MathSign.Minus:
                    lblSign.Text = "-";
                    question.ExpectedAnswer = question.First - question.Second;
                    break;
                case Question.MathSign.Multiply:
                    lblSign.Text = "x";
                    question.ExpectedAnswer = question.First * question.Second;
                    break;
                case Question.MathSign.Divide:
                    lblSign.Text = "÷";
                    question.ExpectedAnswer = question.First / question.Second;
                    break;
                default:
                    throw new Exception("Operation not supported.");
                    
            }
            txtAnswer.Text = "";
            txtAnswer.Focus();
            lblHeader.Text = "Go Aadit Go";
            lblHeader.ForeColor = Color.DodgerBlue;
            question.ActualAnswers = new List<int>();
            questions.Add(question);
        }

        private Question GetNextQuestion()
        {
            var question = new Question();
            var random = new Random();
            question.Sign = AllowedSigns[0];
            var validQuestion = false;
            var lastQuestion = questions.LastOrDefault();
            while (!validQuestion)
            {
                question.First = random.Next(2, 10);
                question.Second = random.Next(2, 10);
                var greater = Math.Max(question.First, question.Second);
                var lesser = Math.Min(question.First, question.Second);

                if (questions.Count < 85)
                {                    
                    question.First = greater;
                    question.Second = lesser;
                    if (question.First == question.Second)
                    {
                        continue;
                    }
                }                
                if (question.Sign == Question.MathSign.Plus && question.First + question.Second > 10)
                {
                    continue;
                }
                if (question.Sign == Question.MathSign.Minus && question.First < question.Second)
                {
                    continue;
                }
                if (lastQuestion != null && (question.First == lastQuestion.First && question.Second == lastQuestion.Second))
                {
                    continue;
                }

                validQuestion = true;
                
            }
            
            return question;
        }

        private void SetSummary()
        {
            lblCorrect.Text = correctCounter.ToString();
            lblIncorrect.Text = incorrectCounter.ToString();
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            int answer;
            try
            {
                answer = Convert.ToInt32(txtAnswer.Text);                
            }
            catch
            {
                txtAnswer.Text = "";
                return;
            }
            var lastQuestion = questions.LastOrDefault();
            lastQuestion.ActualAnswers.Add(answer);
            if (lastQuestion != null)
            {
                if (answer != lastQuestion.ExpectedAnswer)
                {
                    if (lblHeader.ForeColor != Color.Red)
                    {
                        incorrectCounter++;
                    }                    
                    txtAnswer.Focus();
                    lblHeader.Text = "Wrong Answer";
                    lblHeader.ForeColor = Color.Red;
                }
                else
                {
                    System.Media.SoundPlayer player = new System.Media.SoundPlayer(@"question-correct.wav");
                    player.Play();
                    lblFirst.Text = "";
                    lblSecond.Text = "";
                    txtAnswer.Text = "";
                    Thread.Sleep(500);
                    correctCounter++;
                    SetupQuestion();
                }
                SetSummary();
            }
                
        }
    }

    public class Question
    {
        public enum MathSign
        {
            Plus,
            Minus,
            Multiply,
            Divide
        }
        public int First { get; set; }
        public int Second { get; set; }
        public MathSign Sign { get; set; }
        public double ExpectedAnswer { get; set; }
        public List<int> ActualAnswers { get; set; }
    }

    
}
