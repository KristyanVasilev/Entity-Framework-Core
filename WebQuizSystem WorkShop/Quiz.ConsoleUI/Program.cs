using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Quiz.Data;
using Quiz.Services;
using System;
using System.Collections.Generic;
using System.IO;

namespace Quiz.ConsoleUI
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            var serviceProvider = serviceCollection.BuildServiceProvider();

            var json = File.ReadAllText("EF-Core-Quiz.json");
            var questions = JsonConvert.DeserializeObject<IEnumerable<JsonQuestion>>(json);

            var quizService = serviceProvider.GetRequiredService<IQuizService>();
            var questionService = serviceProvider.GetRequiredService<IQestionService>();
            var answerService = serviceProvider.GetRequiredService<IAnswerService>();

            var quizId = quizService.Add("EF Core Test");
            foreach (var question in questions)
            {
                var questionId = questionService.Add(question.Question, quizId);
                foreach (var answer in question.Answers)
                {
                    answerService.Add(answer.Answer, answer.Correct, answer.Correct ? 1 : 0, questionId);
                }
            }

            //var userAnswerService = serviceProvider.GetRequiredService<IUserAnswerService>();
            //userAnswerService.AddUserAnswer("f93c227b-a54e-4a24-8703-cebec0870837", 2, 2);
  
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            services.AddDbContext<ApplicationDbContext>(options
                => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddControllersWithViews();

            services.AddTransient<IQuizService, QuizService>();
            services.AddTransient<IQestionService, QuestionService>();
            services.AddTransient<IAnswerService, AnswerService>();
            services.AddTransient<IUserAnswerService, UserAnswerService>();
        }
    }
}
