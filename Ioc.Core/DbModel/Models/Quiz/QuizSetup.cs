using System.ComponentModel.DataAnnotations.Schema;

namespace Ioc.Core.DbModel.Models.Quiz
{
    /*public class QuizSetup
    {
        public Guid QuizSetupId { get; set; }
        public string QuizTitle { get; set; }
        public List<QuestionSetup> Questions { get; set; }
    }

    public class QuestionSetup
    {
        public Guid QuestionSetupId { get; set; }
        public string QuestionText { get; set; }
        public List<AnswerSetup> Answers { get; set; }
    }

    public class AnswerSetup
    {
        public Guid AnswerSetupId { get; set; }
        public string AnswerText { get; set; }
        public bool IsCorrect { get; set; }
    }*/

    public class QuizSetup : PublicBaseEntity
    {
        public string Title { get; set; }
        public List<QuizDescription>? QuizDescription { get; set; }
        public string? Description { get; set; }
    }

    public class QuizDescription : PublicBaseEntity
    {
        public Guid QuizSetupID { get; set; }
        public Guid QuestionSetupID { get; set; }
        [ForeignKey(nameof(QuestionSetupID))]
        public QuestionSetup? QuestionSetup { get; set; }

    }

    public class QuestionSetup : PublicBaseEntity
    {
        public string? Text { get; set; }
        [NotMapped]
        public List<AnswerSetup>? Answers { get; set; }
        public Guid CorrectAnswerId { get; set; }
        public string? Description { get; set; }
    }

    public class AnswerSetup : PublicBaseEntity
    {
        public string? Text { get; set; }
        public string? Description { get; set; }
        public Guid QuestionSetupID { get; set; }
        public bool IsCorrect { get; set; }
    }


}
