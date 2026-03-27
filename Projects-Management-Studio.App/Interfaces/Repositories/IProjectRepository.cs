using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Projects_Management_Studio.Domain.Entities;

namespace Projects_Management_Studio.App.Interfaces.Repositories
{
    public interface IProjectRepository
    {
        Task AddAsync(Project project);
    }
}