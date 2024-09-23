using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TCGCardCapital.DTOs;
using TCGCardCapital.Models;
using TCGCardCapital.Services.IService;

namespace TCGCardCapital.Services.ServiceImpl
{
    public class CategoryService : ICategoryService
    {
        private readonly TcgcardCapitalContext _context;
        private readonly IMapper _mapper;

        public CategoryService(TcgcardCapitalContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CategoryDTO>> GetCategoriesAsync()
        {
            var categories = await _context.Categories.ToListAsync();
            return _mapper.Map<IEnumerable<CategoryDTO>>(categories);
        }

        public async Task<CategoryDTO> GetCategoryByIdAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null) return null;
            return _mapper.Map<CategoryDTO>(category);
        }

        public async Task<CategoryDTO> CreateCategoryAsync(CategoryDTO categoryDTO)
        {
            var category = _mapper.Map<Category>(categoryDTO);
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return _mapper.Map<CategoryDTO>(category);
        }

        public async Task<bool> UpdateCategoryAsync(int id, CategoryDTO categoryDTO)
        {
            if (id != categoryDTO.CategoryId) return false;

            var category = _mapper.Map<Category>(categoryDTO);
            _context.Entry(category).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Categories.Any(c => c.CategoryId == id))
                    return false;
                throw;
            }
        }

        public async Task<bool> DeleteCategoryAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null) return false;

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
