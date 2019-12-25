using Autofac;
using AutoMapper;
using CoreTemplate.Application.Application;
using CoreTemplate.Application.Dto.User;
using CoreTemplate.Application.IServices;
using CoreTemplate.Domain;
using CoreTemplate.Domain.IRepositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace CoreTemplate.Application.Services
{
    public class BaseServices<TEntity, TDto, TPrimaryKey> : IBaseServices<TEntity, TDto, TPrimaryKey>
        where TEntity : class, IEntity<TPrimaryKey>, new()
    {

        public IRepository<TEntity, TPrimaryKey> _Repository { get; set; }
        public IMapper _Mapper { get; set; }

        protected BaseServices(IRepository<TEntity, TPrimaryKey> _Repository, IMapper _Mapper)
        {
            this._Repository = _Repository;
            this._Mapper = _Mapper;
        }
        public virtual void Delete(System.Linq.Expressions.Expression<Func<TEntity, bool>> where)
        {
            _Repository.DeleteRange(_Repository.GetAll().Where(where));
        }

        public virtual void DeleteRange(System.Linq.IQueryable<TEntity> entities)
        {
            _Repository.DeleteRange(entities);
        }

        public virtual System.Collections.Generic.List<TDto> FindPageList(int startPage, int pageSize, out int rowCount, System.Linq.Expressions.Expression<Func<TEntity, bool>> where, System.Linq.Expressions.Expression<Func<TEntity, object>> order, string orderType = "asc")
        {
            var result = _Repository.FindPageList(startPage, pageSize, out rowCount, where, order).ToList();
            var dtos = _Mapper.Map<List<TDto>>(result);
            return dtos;
        }

        public virtual List<TDto> GetAll()
        {
            var Table = _Repository.GetAll().ToList();

            var dtos = _Mapper.Map<List<TDto>>(Table);

            return dtos;
        }

        public virtual List<TDto> GetAllList(System.Linq.Expressions.Expression<Func<TEntity, bool>> predicate)
        {
            var result = _Repository.GetAllList(predicate);
            var dtos = _Mapper.Map<List<TDto>>(result);
            return dtos;
        }

        public virtual TEntity Insert(TEntity entity)
        {
            var result = _Repository.Insert(entity);
            return result;
        }

        public virtual TEntity Update(TEntity entity)
        {
            var result = _Repository.Update(entity);
            return result;
        }

        public TDto Get(TPrimaryKey id)
        {
            var result = _Repository.Get(id);
            var dtos = _Mapper.Map<TDto>(result);
            return dtos;
        }

        public TDto FirstOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            var result = _Repository.FirstOrDefault(predicate);
            var dtos = _Mapper.Map<TDto>(result);
            return dtos;
        }
    }
}
