using AutoMapper;
using CoreTemplate.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using CoreTemplate.Domain.Entities.Base;
using CoreTemplate.Domain.Model;

namespace CoreTemplate.Application.IServices
{
    public interface IBaseServices<TEntity, TDto, in TPrimaryKey>
        where TEntity : class, IEntity<TPrimaryKey>
        where TDto : class
    {
        List<TDto> GetAllList();
        Task<List<TDto>> GetAllListAsync();
        List<TDto> GetAllList(Expression<Func<TEntity, bool>> predicate);
        Task<List<TDto>> GetAllListAsync(Expression<Func<TEntity, bool>> predicate);
        PageModel<TDto> GetPageList(int startPage, int pageSize, Expression<Func<TEntity, bool>> where, Expression<Func<TEntity, object>> order, string orderType = "asc");
        Task<PageModel<TDto>> GetPageListAsync(int startPage, int pageSize, Expression<Func<TEntity, bool>> where, Expression<Func<TEntity, object>> order, string orderType = "asc");
        TDto FirstOrDefault(TPrimaryKey id);
        Task<TDto> FirstOrDefaultAsync(TPrimaryKey id);
        TDto FirstOrDefault(Expression<Func<TEntity, bool>> predicate);
        Task<TDto> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);
        TDto Insert(TDto entity);
        Task<TDto> InsertAsync(TDto entity);
        TDto Update(TDto entity);
        Task<TDto> UpdateAsync(TDto entity);
        void Delete(Expression<Func<TEntity, bool>> where);
    }
}
