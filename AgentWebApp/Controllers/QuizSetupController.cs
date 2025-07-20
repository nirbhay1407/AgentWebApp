using AutoMapper;
using Ioc.Core;
using Ioc.Core.DbModel.Models.Quiz;
using Ioc.ObjModels.Model;
using Ioc.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AgentWebApp.Controllers
{
    public class QuizSetupController : Controller
    {
        private readonly IQuizSetupService _repository;
        private readonly IQuestionSetupService _repositoryQues;
        private readonly IAnswerSetupService _repositoryAns;
        private readonly IQuizDescService _repositoryQuizDesc;
        private readonly IMapper _mapper;
        private readonly ILogger<QuizSetup> _logger;

        public QuizSetupController(IQuizSetupService repository, IQuestionSetupService repositoryQues, IAnswerSetupService repositoryAns, IQuizDescService repositoryQuizDesc, IMapper mapper, ILogger<QuizSetup> logger)
        {
            _repository = repository;
            _repositoryQues = repositoryQues;
            _repositoryAns = repositoryAns;
            _repositoryQuizDesc = repositoryQuizDesc;
            _mapper = mapper;
            _logger = logger;
        }

        public IActionResult QuizSetup()
        {
            var model = new QuizSetup
            {
                QuizDescription = new List<QuizDescription>
                {
                    new QuizDescription
                    {
                        QuestionSetup = new QuestionSetup
                        {
                            Answers = new List<AnswerSetup>()
                        }
                    }
                }
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult SaveQuiz(QuizSetup model)
        {
            // Save the quiz setup to the database
            return RedirectToAction("QuizSetup");
        }

        public IActionResult AddQuestion()
        {
            var question = new QuestionSetup
            {
                Answers = new List<AnswerSetup>()
            };
            return PartialView("_QuestionSetup", question);
        }

        public IActionResult AddAnswer()
        {
            var answer = new AnswerSetup();
            return PartialView("_AnswerSetup", answer);
        }


        public IActionResult NkQuizView()
        {
            var quizSetup = _repository.GetAllWithIncById(new Guid("FFD4B221-49FA-447A-93D5-7E27E3A101D7"));


            if (quizSetup == null)
                return NotFound();
            else
            {
                quizSetup.QuizDescription = _repositoryQuizDesc.GetAllWithInclude("QuestionSetup").Where(y => y.QuizSetupID == quizSetup.ID).ToList();
                quizSetup.QuizDescription.ForEach(y =>
                {
                    y.QuestionSetup = _repository.GetAllQuestionSetup(y.QuestionSetupID).FirstOrDefault();
                });
            }
            var indexX = 1;
            List<QuizQuestion> questionsList = new List<QuizQuestion>();
            quizSetup.QuizDescription.ForEach(x =>
            {

                QuizQuestion questions = new QuizQuestion();
                var answerList = new List<QuizAnswer>();
                var rightAnsId = new Guid();
                questions.Id = indexX;
                questions.QuestionText = x.QuestionSetup.Text;
                x.QuestionSetup.Answers.ForEach(answer =>
                {
                    var answerObj = new QuizAnswer();
                    answerObj.AnswerText = answer.Text;
                    answerObj.Id = answer.ID;
                    answerList.Add(answerObj);
                    if (answer.IsCorrect)
                        rightAnsId = answer.ID;
                });
                questions.Options = answerList;
                //questions.SelectedAnswer = rightAnsId;
                questionsList.Add(questions);
                indexX++;
            });

            var quizViewModel = new QuizViewModel
            {
                Questions = questionsList,
                TimeLimitInMinutes = 10 // Set your desired time limit
            };
            return View(quizViewModel);

        }

        public ActionResult SubmitQuiz(QuizViewModel quizViewModel)
        {
            // Process the submitted quiz
            // Access the selected answers from quizViewModel.Questions

            // Redirect or return appropriate response
            return View("QuizResult");
        }


        // GET api/quizsetup/{id} 
        [HttpGet]
        public async Task<ActionResult<QuizSetup>> Get()
        {
            //var quizSetup = _repository.GetAllAsync().Result.ToList();
            var quizSetup = _repository.GetAllWithInc();
            quizSetup.ForEach(x =>
            {
                x.QuizDescription = _repositoryQuizDesc.GetAllWithInclude("QuestionSetup").Where(y => y.QuizSetupID == x.ID).ToList();
                x.QuizDescription.ForEach(y =>
                {
                    //y.QuestionSetup = _repositoryQues.GetById(y.QuestionSetupID).Result;
                    y.QuestionSetup = _repository.GetAllQuestionSetup(y.QuestionSetupID).FirstOrDefault();
                    //y.QuestionSetup.Answers = _repositoryAns.get(y.QuestionSetupID).Result;
                });

                //x.q
            });
            if (quizSetup == null)
                return NotFound();

            return Ok(quizSetup);
        }

        // GET api/quizsetup/{id} 
        [HttpGet("get-question")]
        public async Task<ActionResult<QuestionSetup>> GetQuestion()
        {
            //var quizSetup = _repository.GetAllAsync().Result.ToList();
            var questionSetup = await _repositoryQues.GetAllAsync();

            if (questionSetup == null)
                return NotFound();

            return Ok(questionSetup);
        }


        [HttpGet("AllQuestionSetup")]
        public ActionResult<QuestionSetup> GetAllQuestionSetup()
        {
            //var quizSetup = _repository.GetAllAsync().Result.ToList();
            var quizSetup = _repository.GetAllQuestionSetup(Guid.Empty);

            if (quizSetup == null)
                return NotFound();

            return Ok(quizSetup);
        }

        // GET api/quizsetup/{id}
        [HttpGet("GetById/{id}")]
        public async Task<ActionResult<QuizSetup>> Get(Guid id)
        {
            /* var quizSetup = await _repository.GetById(id);

             if (quizSetup == null)
                 return NotFound();

             return quizSetup;*/

            var quizSetup = _repository.GetAllWithIncById(id);


            if (quizSetup == null)
                return NotFound();
            else
            {
                quizSetup.QuizDescription = _repositoryQuizDesc.GetAllWithInclude("QuestionSetup").Where(y => y.QuizSetupID == quizSetup.ID).ToList();
                quizSetup.QuizDescription.ForEach(y =>
                {
                    y.QuestionSetup = _repository.GetAllQuestionSetup(y.QuestionSetupID).FirstOrDefault();
                });
            }

            return Ok(quizSetup);


        }

        // POST api/quizsetup
        [HttpPost]
        public async Task<ActionResult<QuizSetup>> Post(QuizSetup quizSetup)
        {
            //var da = _repository.GetById(quizSetup.ID);
            if (!_repository.CheckExist(quizSetup.ID))
                await _repository.Create(quizSetup);
            else
                await _repository.Update(quizSetup.ID, quizSetup);
            if (quizSetup.QuizDescription?.Count > 0)
            {
                quizSetup.QuizDescription.ForEach(x =>
                {
                    x.QuizSetupID = quizSetup.ID;
                    if (!_repositoryQuizDesc.CheckExist(x.ID))
                        _repositoryQuizDesc.Create(x);
                });
            };
            return CreatedAtAction(nameof(Get), new { id = quizSetup.ID }, quizSetup);
        }


        // POST api/quizsetup
        [HttpPost("create-quiz")]
        public async Task<ActionResult<QuizSetup>> SaveQuestion(QuizSetup quizSetup)
        {
            //var da = _repository.GetById(quizSetup.ID);
            if (!_repository.CheckExist(quizSetup.ID))
                await _repository.Create(quizSetup);
            else
                await _repository.Update(quizSetup.ID, quizSetup);
            if (quizSetup.QuizDescription?.Count > 0)
            {
                quizSetup.QuizDescription.ForEach(x =>
                {
                    x.QuizSetupID = quizSetup.ID;
                    if (!_repositoryQuizDesc.CheckExist(x.ID))
                        _repositoryQuizDesc.Create(x);
                });
            }

            return CreatedAtAction(nameof(Get), new { id = quizSetup.ID }, quizSetup);
        }


        // POST api/quizsetup
        [HttpPost("save-question")]
        public async Task<ActionResult<QuizSetup>> SaveQuestion(QuestionSetup quizSetup)
        {
            if (quizSetup.Answers?.Count > 0)
            {
                quizSetup.Answers.ForEach(x =>
                {
                    x.QuestionSetupID = quizSetup.ID;
                });
                var AllAns = _repositoryAns.GetByQuesId(quizSetup.ID);
                if (_repositoryAns.CheckByQuesId(quizSetup.ID))
                {
                    await _repositoryAns.CreateInRange(quizSetup.Answers);
                }
                else
                {
                    quizSetup.Answers.ForEach(x =>
                    {
                        var extData = _repositoryAns.GetById(x.ID);
                        if (extData != null && AllAns.Count > 4)
                            _repositoryAns.Create(x);
                        else
                            _repositoryAns.Update(x.ID, x);
                    });
                }
                quizSetup.CorrectAnswerId = quizSetup!.Answers!.Where(x => x.IsCorrect).FirstOrDefault().ID;
                if (_repositoryQues.CheckExist(quizSetup.ID))
                    await _repositoryQues.Update(quizSetup.ID, quizSetup);
                else
                    await _repositoryQues.Create(quizSetup);
            }

            return CreatedAtAction(nameof(Get), new { id = quizSetup.ID }, quizSetup);
        }

        /*// PUT api/quizsetup/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, QuizSetup quizSetup)
        {
            if (id != quizSetup.ID)
                return BadRequest();

            await _repository.Update(id, quizSetup);

            return NoContent();
        }*/

        // DELETE api/quizsetup/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            PublicBaseEntity pp;
            var quizSetup = await _repository.GetById(id);

            if (quizSetup == null)
                return NotFound();

            await _repository.Delete(id);
            return NoContent();
        }
    }

}
