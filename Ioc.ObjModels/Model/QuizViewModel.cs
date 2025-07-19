namespace Ioc.ObjModels.Model
{
    public class QuizQuestion
    {
        public int Id { get; set; }
        public string QuestionText { get; set; }
        public List<QuizAnswer> Options { get; set; }
        public bool SelectedAnswer { get; set; }
    }

    public class QuizAnswer
    {
        public Guid Id { get; set; }
        public string AnswerText { get; set; }
        public bool IsSelected { get; set; }
    }

    public class QuizViewModel
    {
        public List<QuizQuestion> Questions { get; set; }
        public int TimeLimitInMinutes { get; set; }
    }
}
