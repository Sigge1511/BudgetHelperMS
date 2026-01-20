using BudgetHelperClassLibrary.Data;
using BudgetHelperClassLibrary.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetHelperClassLibrary.Repositories
{
    public class CategoryRepo : ICategoryRepo
    {
        private readonly BudgetHelperDbContext _dbcntxt;

        public CategoryRepo(BudgetHelperDbContext dbcntxt)
        {
            _dbcntxt = dbcntxt;
        }

        public async Task<List<Category>> GetAllCategoriesAsync()
        {
            // Här kan det vara smart att använda .Include om du vill ha med listorna direkt
            return await _dbcntxt.Categories.ToListAsync();
        }

        public async Task<Category?> GetCategoryByIdAsync(int id)
        {
            return await _dbcntxt.Categories
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task AddCategoryAsync(Category category)
        {
            await _dbcntxt.Categories.AddAsync(category);
            await _dbcntxt.SaveChangesAsync();
        }

        //public async Task UpdateCategoryAsync(Category category)
        //{
        //    _dbcntxt.Categories.Update(category);
        //    await _dbcntxt.SaveChangesAsync();
        //}

        //public async Task DeleteCategoryAsync(Category category)
        //{
        //    _dbcntxt.Categories.Remove(category);
        //    await _dbcntxt.SaveChangesAsync();
        //}
    }
}

