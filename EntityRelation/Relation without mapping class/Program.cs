using P01_StudentSystem.Data.Models;
using System;

namespace P01_StudentSystem
{
    public class Program
    {
        static void Main(string[] args)
        {

            var db = new StudentSystemContext();

            //better use migrations
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();
        }
    }
}
