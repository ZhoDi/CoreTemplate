using Autofac;
using AutoMapper;
using CoreTemplate.Application.Application;
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
using CoreTemplate.Domain.Entities.Base;
using CoreTemplate.Domain.Model;

namespace CoreTemplate.Application.Services
{
    public class BaseServices<TEntity, TDto, TKey> : IBaseServices<TEntity, TDto, TKey>
        where TEntity : class, IEntity<TKey>, new()
        where TDto : class
    {

        public IRepository<TEntity, TKey> Repository { get; set; }
        public IMapper Mapper { get; set; }

        protected BaseServices(IRepository<TEntity, TKey> repository, IMapper mapper)
        {
            this.Repository = repository;
            this.Mapper = mapper;
        }


        public List<TDto> GetAllList()
        {
            var table = Repository.GetAll().ToList();

            var dtos = Mapper.Map<List<TDto>>(table);

            return dtos;
        }

        public List<TDto> GetAllList(Expression<Func<TEntity, bool>> predicate)
        {
            var result = Repository.GetAllList(predicate);
            var dtos = Mapper.Map<List<TDto>>(result);
            return dtos;
        }

        public PageModel<TDto> GetPageList(int startPage, int pageSize, Expression<Func<TEntity, bool>> where, System.Linq.Expressions.Expression<Func<TEntity, object>> order, string orderType = "asc")
        {
            var result = Repository.GetPageList(startPage, pageSize, where, order);

            var dtos = Mapper.Map<PageModel<TDto>>(result);
            return dtos;
        }

        public TDto Insert(TDto dto)
        {
            var entity = Mapper.Map<TEntity>(dto);
            var result = Repository.Insert(entity);
            var dtos = Mapper.Map<TDto>(result);
            return dtos;
        }

        public TDto Update(TDto dto)
        {
            var entity = Mapper.Map<TEntity>(dto);
            var result = Repository.Update(entity);
            Repository.Save();
            return dto;
        }

        public TDto FirstOrDefault(TKey id)
        {
            var result = Repository.FirstOrDefault(id);
            var dtos = Mapper.Map<TDto>(result);
            return dtos;
        }

        public TDto FirstOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            var result = Repository.FirstOrDefault(predicate);
            var dtos = Mapper.Map<TDto>(result);
            return dtos;
        }

        public void Delete(System.Linq.Expressions.Expression<Func<TEntity, bool>> where)
        {
            Repository.Delete(where);
        }

        public async Task<List<TDto>> GetAllListAsync()
        {
            var Table = await Repository.GetAllAsync();

            var dtos = Mapper.Map<List<TDto>>(Table);

            return dtos;
        }

        public async Task<List<TDto>> GetAllListAsync(Expression<Func<TEntity, bool>> predicate)
        {
            var result = await Repository.GetAllListAsync(predicate);
            var dtos = Mapper.Map<List<TDto>>(result);
            return dtos;
        }

        public async Task<PageModel<TDto>> GetPageListAsync(int startPage, int pageSize, Expression<Func<TEntity, bool>> where, Expression<Func<TEntity, object>> order, string orderType = "asc")
        {
            var result = await Repository.GetPageListAsync(startPage, pageSize, where, order);

            var dtos = Mapper.Map<PageModel<TDto>>(result);
            return dtos;
        }

        public async Task<TDto> FirstOrDefaultAsync(TKey id)
        {
            var result = await Repository.FirstOrDefaultAsync(id);
            var dtos = Mapper.Map<TDto>(result);
            return dtos;
        }

        public async Task<TDto> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            var result = await Repository.FirstOrDefaultAsync(predicate);
            var dtos = Mapper.Map<TDto>(result);
            return dtos;
        }

        public async Task<TDto> InsertAsync(TDto dto)
        {
            var entity = Mapper.Map<TEntity>(dto);
            var result = await Repository.InsertAsync(entity);
            var dtos = Mapper.Map<TDto>(result);
            return dtos;
        }

        public async Task<TDto> UpdateAsync(TDto dto)
        {
            var entity = Mapper.Map<TEntity>(dto);
            await Repository.UpdateAsync(entity);
            return dto;
        }
    }
}
