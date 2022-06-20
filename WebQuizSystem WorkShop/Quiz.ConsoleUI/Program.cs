using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quiz.Data;
using Quiz.Services;
using System;
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

            var quizService = serviceProvider.GetRequiredService<IQuizService>();
            //quizService.Add("C# DB");
            var quiz = quizService.GetQuizById(1);
            Console.WriteLine(quiz.Title);

            foreach (var question in quiz.Questions)
            {
                Console.WriteLine(question.Title);

                foreach (var answer in question.Answers)
                {
                    Console.WriteLine(answer.Title);
                }
            }

            //var questionService = serviceProvider.GetRequiredService<IQestionService>();
            //questionService.Add("What is Entity Framework Core?", 1);

            //var answerService = serviceProvider.GetRequiredService<IAnswerService>();
            //answerService.Add("It is a ORM", false, 0, 2);
            //answerService.Add("It is a Mirco ORM", false, 0, 2);

            //var userAnswerService = serviceProvider.GetRequiredService<IUserAnswerService>();
            //userAnswerService.AddUserAnswer("f93c227b-a54e-4a24-8703-cebec0870837", 1, 2, 2);

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
