using AutoMapper;
using CoreTemplate.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace CoreTemplate.Application.IServices
{
    public interface IBaseServices<TEntity, TDto,TPrimaryKey> where TEntity : class,IEntity<TPrimaryKey>
    {
        List<TDto> GetAll();
        List<TDto> GetAllList(Expression<Func<TEntity, bool>> predicate);
        List<TDto> FindPageList(int startPage, int pageSize, out int rowCount, Expression<Func<TEntity, bool>> where, Expression<Func<TEntity, object>> order, string orderType = "asc");

        TDto Get(TPrimaryKey id);
        TDto FirstOrDefault(Expression<Func<TEntity, bool>> predicate);

        TEntity Insert(TEntity entity);
        TEntity Update(TEntity entity);
        void Delete(Expression<Func<TEntity, bool>> where);
        void DeleteRange(IQueryable<TEntity> entities);
    }
}
