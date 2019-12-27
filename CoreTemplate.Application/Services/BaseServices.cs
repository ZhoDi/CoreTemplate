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
using System.Threading.Tasks;

namespace CoreTemplate.Application.Services
{
    public class BaseServices<TEntity, TDto, TPrimaryKey> : IBaseServices<TEntity, TDto, TPrimaryKey>
        where TEntity : class, IEntity<TPrimaryKey>, new()
        where TDto : class
    {

        public IRepository<TEntity, TPrimaryKey> _Repository { get; set; }
        public IMapper _Mapper { get; set; }

        protected BaseServices(IRepository<TEntity, TPrimaryKey> _Repository, IMapper _Mapper)
        {
            this._Repository = _Repository;
            this._Mapper = _Mapper;
        }


        public List<TDto> GetAllList()
        {
            var Table = _Repository.GetAll().ToList();

            var dtos = _Mapper.Map<List<TDto>>(Table);

            return dtos;
        }

        public List<TDto> GetAllList(System.Linq.Expressions.Expression<Func<TEntity, bool>> predicate)
        {
            var result = _Repository.GetAllList(predicate);
            var dtos = _Mapper.Map<List<TDto>>(result);
            return dtos;
        }

        public PageModel<TDto> GetPageList(int startPage, int pageSize, System.Linq.Expressions.Expression<Func<TEntity, bool>> where, System.Linq.Expressions.Expression<Func<TEntity, object>> order, string orderType = "asc")
        {
            var result = _Repository.GetPageList(startPage, pageSize, where, order);

            var dtos = _Mapper.Map<PageModel<TDto>>(result);
            return dtos;
        }

        public TDto Insert(TDto dto)
        {
            var entity = _Mapper.Map<TEntity>(dto);
            var result = _Repository.Insert(entity);
            var dtos = _Mapper.Map<TDto>(result);
            return dtos;
        }

        public TDto Update(TDto dto)
        {
            var entity = _Mapper.Map<TEntity>(dto);
            var result = _Repository.Update(entity);
            _Repository.Save();
            return dto;
        }

        public TDto FirstOrDefault(TPrimaryKey id)
        {
            var result = _Repository.FirstOrDefault(id);
            var dtos = _Mapper.Map<TDto>(result);
            return dtos;
        }

        public TDto FirstOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            var result = _Repository.FirstOrDefault(predicate);
            var dtos = _Mapper.Map<TDto>(result);
            return dtos;
        }

        public void Delete(System.Linq.Expressions.Expression<Func<TEntity, bool>> where)
        {
            _Repository.Delete(where);
        }

        public async Task<List<TDto>> GetAllListAsync()
        {
            var Table = await _Repository.GetAllAsync();

            var dtos = _Mapper.Map<List<TDto>>(Table);

            return dtos;
        }

        public async Task<List<TDto>> GetAllListAsync(Expression<Func<TEntity, bool>> predicate)
        {
            var result = await _Repository.GetAllListAsync(predicate);
            var dtos = _Mapper.Map<List<TDto>>(result);
            return dtos;
        }

        public async Task<PageModel<TDto>> GetPageListAsync(int startPage, int pageSize, Expression<Func<TEntity, bool>> where, Expression<Func<TEntity, object>> order, string orderType = "asc")
        {
            var result = await _Repository.GetPageListAsync(startPage, pageSize, where, order);

            var dtos = _Mapper.Map<PageModel<TDto>>(result);
            return dtos;
        }

        public async Task<TDto> FirstOrDefaultAsync(TPrimaryKey id)
        {
            var result = await _Repository.FirstOrDefaultAsync(id);
            var dtos = _Mapper.Map<TDto>(result);
            return dtos;
        }

        public async Task<TDto> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            var result = await _Repository.FirstOrDefaultAsync(predicate);
            var dtos = _Mapper.Map<TDto>(result);
            return dtos;
        }

        public async Task<TDto> InsertAsync(TDto dto)
        {
            var entity = _Mapper.Map<TEntity>(dto);
            var result = await _Repository.InsertAsync(entity);
            var dtos = _Mapper.Map<TDto>(result);
            return dtos;
        }

        public async Task<TDto> UpdateAsync(TDto dto)
        {
            var entity = _Mapper.Map<TEntity>(dto);
            await _Repository.UpdateAsync(entity);
            return dto;
        }

        public async Task DeleteAsync(Expression<Func<TEntity, bool>> where)
        {
            await _Repository.DeleteAsync(where);
        }
    }
}
