using Bogus;
using PracticaAPI.Core.Entities;
using PracticaAPI.Data;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace PracticaAPI.Data
{
    public class DataSeeder
    {
        private readonly AppDbContext _context;
        private readonly Random _random = new Random();

        public DataSeeder(AppDbContext context)
        {
            _context = context;
        }

        public void Seed(int numCategories = 0, int numBudgets = 0, int numExpenses = 0)
        {
            var userId = EnsureDummyUser();
            if (numBudgets > 0)
                AddBudgets(numBudgets, userId);
            if (numCategories > 0)
                AddCategories(numCategories);
            if (numExpenses > 0)
                AddExpenses(numExpenses);
        }

        private Guid EnsureDummyUser()
        {
            var user = _context.Users.FirstOrDefault();
            if (user == null)
            {
                user = new User
                {
                    Username = "dummyuser",
                    PasswordHash = HashPassword("dummy1234"),
                    CreatedAt = DateTime.UtcNow,
                    LastLogin = DateTime.UtcNow,
                    IsActive = true
                };
                _context.Users.Add(user);
                _context.SaveChanges();
            }
            return user.Id;
        }

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }

        public void AddBudgets(int count, Guid userId)
        {
            var budgetFaker = new Faker<MonthlyBudget>()
                .RuleFor(b => b.Month, f => f.Date.Between(DateTime.Now.AddYears(-5), DateTime.Now))
                .RuleFor(b => b.UserId, f => userId);
            var budgets = budgetFaker.Generate(count);
            _context.MonthlyBudgets.AddRange(budgets);
            _context.SaveChanges();
        }

        public void AddCategories(int count)
        {
            var budgets = _context.MonthlyBudgets.ToList();
            if (!budgets.Any()) return;
            var existingNames = new HashSet<string>(_context.BudgetCategories.Select(c => c.Name));
            var categoryFaker = new Faker<BudgetCategory>()
                .RuleFor(c => c.Name, f => f.Commerce.Categories(1)[0] + " " + f.UniqueIndex)
                .RuleFor(c => c.Limit, f => f.Random.Decimal(200, 2000))
                .RuleFor(c => c.MonthlyBudgetId, f => budgets[_random.Next(budgets.Count)].Id);
            var categories = new List<BudgetCategory>();
            while (categories.Count < count)
            {
                var cat = categoryFaker.Generate();
                if (!existingNames.Contains(cat.Name))
                {
                    categories.Add(cat);
                    existingNames.Add(cat.Name);
                }
            }
            _context.BudgetCategories.AddRange(categories);
            _context.SaveChanges();
        }

        public void AddExpenses(int count)
        {
            var categories = _context.BudgetCategories.ToList();
            if (!categories.Any()) return;
            var expenseFaker = new Faker<Expense>()
                .RuleFor(e => e.Amount, f => f.Random.Decimal(10, 500))
                .RuleFor(e => e.Date, f => f.Date.Between(DateTime.Now.AddMonths(-6), DateTime.Now))
                .RuleFor(e => e.CategoryId, f => categories[_random.Next(categories.Count)].Id);
            var expenses = new List<Expense>();
            for (int i = 0; i < count; i++)
            {
                var expense = expenseFaker.Generate();
                expenses.Add(expense);
            }
            _context.Expenses.AddRange(expenses);
            _context.SaveChanges();
        }
    }
} 