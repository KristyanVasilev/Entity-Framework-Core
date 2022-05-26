using CodeFirst.Models;
using System;
using System.Collections.Generic;

namespace CodeFirst
{
    public class Program
    {
        static void Main(string[] args)
        {
            var db = new ApplicationDbContext();
            db.Database.EnsureCreated();

            db.Categories.Add(new Category
            {
                Title = "Sport",
                News = new List<News>
                {
                    new News
                    {
                        Title = "Kupata e sinq!",
                        Content = "Купата е СИНЯ!!!",
                        Comments = new List<Comment>
                        {
                            new Comment { Author = "Nasko", Content = "Da!"},
                            new Comment { Author = "Muri", Content = "Champions!"}
                        }
                    }
                }
            });

            db.SaveChanges();
        }
    }
}
