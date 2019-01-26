﻿using System.Collections.Generic;
using System.Threading.Tasks;
using ApplicationCore.Entities;

namespace ApplicationCore.Interfaces
{
    public interface IExhibitorRepository
    {
        Task<List<Exhibitor>> All();
        Task<IEnumerable<Exhibitor>> AllLight();

        Task<Exhibitor> GetById(int id);

        //IEnumerable<Exhibitor> GetByCategory(Category category);
        Exhibitor Add(Exhibitor Exhibitor);
        Exhibitor Remove(Exhibitor Exhibitor);
        Task<int> SaveChanges();
    }
}