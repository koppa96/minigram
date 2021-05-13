using System;
using System.Reflection;
using AutoMapper;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Minigram.Dal;

namespace Minigram.Tests.Application
{
    public class AppDependencyFixture : IDisposable
    {
        private readonly SqliteConnection connection;
        public MinigramDbContext MinigramDbContext { get; }
        public IConfigurationProvider ConfigurationProvider { get; }
        
        public AppDependencyFixture()
        {
            connection = new SqliteConnection("Data Source=:memory:");
            connection.Open();

            MinigramDbContext = new MinigramDbContext(new DbContextOptionsBuilder<MinigramDbContext>()
                .UseSqlite(connection)
                .Options);

            MinigramDbContext.Database.EnsureCreated();
            ConfigurationProvider = new MapperConfiguration(options =>
            {
                options.AddMaps(Assembly.Load("Minigram.Application"));
            });
        }

        public void Dispose()
        {
            MinigramDbContext?.Dispose();
            connection?.Dispose();
        }
    }
}