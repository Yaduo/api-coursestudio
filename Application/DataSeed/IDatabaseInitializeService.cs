using System;
using System.Threading.Tasks;

namespace CourseStudio.DataSeed.Services
{
    public interface IDatabaseInitializeService
    {
		Task EnsureCreated();
		Task Seed();
    }
}
