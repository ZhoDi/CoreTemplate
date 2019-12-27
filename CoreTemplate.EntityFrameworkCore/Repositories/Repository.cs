using CoreTemplate.Domain;
using CoreTemplate.Domain.Extensions;
using CoreTemplate.Domain.IRepositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CoreTemplate.EntityFrameworkCore.Repositories
{
    public class Repository<TEntity, TPrimaryKey> : IRepository<TEntity, TPrimaryKey> where TEntity : class, IEntity<TPrimaryKey>
    {

        //定义数据访问上下文对象
        protected readonly TempDbContext _dbContext;

        /// <summary>
        /// 通过构造函数注入得到数据上下文对象实例
        /// </summary>
        /// <param name="dbContext"></param>
        public Repository(TempDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Gets DbSet for given entity.
        /// </summary>
        public virtual DbSet<TEntity> Table => _dbContext.Set<TEntity>();

        #region Select
        /// <summary>
        /// 获取集合
        /// </summary>
        /// <returns></returns>
        public IQueryable<TEntity> GetAll()
        {
            return GetAllIncluding();
        }

        /// <summary>
        /// 获取集合
        /// </summary>
        /// <returns></returns>
        public async Task<IQueryable<TEntity>> GetAllAsync()
        {
            return await GetAllIncludingAsync();
        }

        /// <summary>
        /// 获取集合并排序
        /// </summary>
        /// <param name="propertySelectors"></param>
        /// <returns></returns>
        public IQueryable<TEntity> GetAllIncluding(params Expression<Func<TEntity, object>>[] propertySelectors)
        {
            var query = Table.AsQueryable();

            if (!propertySelectors.IsNullOrEmpty())
            {
                foreach (var propertySelector in propertySelectors)
                {
                    query = query.Include(propertySelector);
                }
            }
            return query;
        }

        /// <summary>
        /// 获取集合并排序
        /// </summary>
        /// <param name="propertySelectors"></param>
        /// <returns></returns>
        public Task<IQueryable<TEntity>> GetAllIncludingAsync(params Expression<Func<TEntity, object>>[] propertySelectors)
        {
            var query = Table.AsQueryable();

            if (!propertySelectors.IsNullOrEmpty())
            {
                foreach (var propertySelector in propertySelectors)
                {
                    query = query.Include(propertySelector);
                }
            }

            return Task.FromResult(query);
        }

        /// <summary>
        /// 获取实体集合
        /// </summary>
        /// <returns></returns>
        public List<TEntity> GetAllList()
        {
            return GetAll().ToList();
        }

        /// <summary>
        /// 获取实体集合
        /// </summary>
        /// <returns></returns>
        public async Task<List<TEntity>> GetAllListAsync()
        {
            return await GetAll().ToListAsync();
        }

        /// <summary>
        /// 根据lambda表达式条件获取实体集合
        /// </summary>
        /// <param name="predicate">lambda表达式条件</param>
        /// <returns></returns>
        public List<TEntity> GetAllList(Expression<Func<TEntity, bool>> predicate)
        {
            return GetAll().Where(predicate).ToList();
        }

        /// <summary>
        /// 根据lambda表达式条件获取实体集合
        /// </summary>
        /// <param name="predicate">lambda表达式条件</param>
        /// <returns></returns>
        public async Task<List<TEntity>> GetAllListAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await GetAll().Where(predicate).ToListAsync();
        }

        /// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="id">实体主键</param>
        /// <returns></returns>
        public TEntity FirstOrDefault(TPrimaryKey id)
        {
            return GetAll().FirstOrDefault(CreateEqualityExpressionForId(id));
        }

        /// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="id">实体主键</param>
        /// <returns></returns>
        public async Task<TEntity> FirstOrDefaultAsync(TPrimaryKey id)
        {
            return await GetAll().FirstOrDefaultAsync(CreateEqualityExpressionForId(id));

        }

        /// <summary>
        /// 根据lambda表达式条件获取单个实体
        /// </summary>
        /// <param name="predicate">lambda表达式条件</param>
        /// <returns></returns>
        public TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return GetAll().FirstOrDefault(predicate);
        }

        /// <summary>
        /// 根据lambda表达式条件获取单个实体
        /// </summary>
        /// <param name="predicate">lambda表达式条件</param>
        /// <returns></returns>
        public async Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await GetAll().FirstOrDefaultAsync(predicate);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="whereExpression"></param>
        /// <param name="intPageIndex"></param>
        /// <param name="intPageSize"></param>
        /// <param name="order"></param>
        /// <param name="orderType"></param>
        /// <returns></returns>
        public PageModel<TEntity> GetPageList(int intPageIndex, int intPageSize, Expression<Func<TEntity, bool>> where, Expression<Func<TEntity, object>> order,  string orderType = "asc")
        {
            var result = GetAll().Where(where);
            int totalCount = result.Count();
            if (order != null)
            {
                if (orderType == "asc")
                    result = result.OrderBy(order);
                else
                    result = result.OrderByDescending(order);
            }
            else
            {
                result = result.OrderByDescending(m => m.Id);
            }
            int pageCount = (int)Math.Ceiling((double)(totalCount / intPageSize));
            var _list = result.Skip((intPageIndex - 1) * intPageSize).Take(intPageSize).ToList();

            return new PageModel<TEntity>() { PageCount = pageCount, TotalCount = totalCount, TEntityList = _list, PageIndex = intPageIndex, PageSize = intPageSize };
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="whereExpression"></param>
        /// <param name="intPageIndex"></param>
        /// <param name="intPageSize"></param>
        /// <param name="order"></param>
        /// <param name="orderType"></param>
        /// <returns></returns>
        public async Task<PageModel<TEntity>> GetPageListAsync(int intPageIndex, int intPageSize, Expression<Func<TEntity, bool>> whereExpression,  Expression<Func<TEntity, object>> order, string orderType = "asc")
        {
            var result = GetAll().Where(whereExpression);
            int totalCount = result.Count();
            if (order != null)
            {
                if (orderType == "asc")
                    result = result.OrderBy(order);
                else
                    result = result.OrderByDescending(order);
            }

            int pageCount = (int)Math.Ceiling((double)(totalCount / intPageSize));
            var _list = await result.Skip((intPageIndex - 1) * intPageSize).Take(intPageSize).ToListAsync();

            return new PageModel<TEntity>() { PageCount = pageCount, TotalCount = totalCount, TEntityList = _list, PageIndex = intPageIndex, PageSize = intPageSize };
        }

        /// <summary>
        /// 通过Sql分页查询
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalRecord"></param>
        /// <param name="sql"></param>
        /// <param name="order"></param>
        /// <param name="orderType"></param>
        /// <returns></returns>
        public PageModel<TEntity> FindPageListFromSql(int intPageIndex, int intPageSize, string sql, Expression<Func<TEntity, object>> order, string orderType = "asc")
        {
            var result = GetFromSql(sql);

            var totalCount = result.Count();
            if (order != null)
            {
                if (orderType == "asc")
                    result = result.OrderBy(order);
                else
                    result = result.OrderByDescending(order);
            }
            int pageCount = (int)Math.Ceiling((double)(totalCount / intPageSize));
            var _list =  result.Skip((intPageIndex - 1) * intPageSize).Take(intPageSize).ToList();

            return new PageModel<TEntity>() { PageCount = pageCount, TotalCount = totalCount, PageIndex = intPageIndex, PageSize = intPageSize, TEntityList = _list };
        }

        /// <summary>
        /// 通过Sql查询
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public IQueryable<TEntity> GetFromSql(string sql)
        {
            var _list = GetAll().FromSql(sql);

            return _list;
        }

        #endregion

        #region Insert
        /// <summary>
        /// 新增实体
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="autoSave">是否立即执行保存</param>
        /// <returns></returns>
        public TEntity Insert(TEntity entity)
        {
            return Table.Add(entity).Entity;
        }

        /// <summary>
        /// 新增实体
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="autoSave">是否立即执行保存</param>
        /// <returns></returns>
        public Task<TEntity> InsertAsync(TEntity entity)
        {
            return Task.FromResult(Insert(entity));
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="entitys"></param>
        public void BatchInsert(List<TEntity> entitys)
        {
            Table.AddRange(entitys);
            Save();
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="entitys"></param>
        public async Task BatchInsertAsync(List<TEntity> entitys)
        {
            await Table.AddRangeAsync(entitys);
            await SaveAsync();
        }

        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="autoSave">是否立即执行保存</param>
        public TEntity Update(TEntity entity)
        {
            // 发现并没什么用先，先注释
            //AttachIfNot(entity);
            _dbContext.Entry(entity).State = EntityState.Modified;
            return entity;
        }

        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="autoSave">是否立即执行保存</param>
        public Task<TEntity> UpdateAsync(TEntity entity)
        {
            Update(entity);
            return Task.FromResult(entity);
        }

        /// <summary>
        /// 新增或更新实体
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="autoSave">是否立即执行保存</param>
        public TEntity InsertOrUpdate(TEntity entity)
        {
            return entity.IsTransient()
                ? Insert(entity)
                : Update(entity);
        }

        /// <summary>
        /// 新增或更新实体
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="autoSave">是否立即执行保存</param>
        public async Task<TEntity> InsertOrUpdateAsync(TEntity entity)
        {
            return entity.IsTransient()
                ? await InsertAsync(entity)
                : await UpdateAsync(entity);
        }

        /// <summary>
        /// 实体类不存在,追加到上下文中
        /// </summary>
        /// <param name="entity"></param>
        public void AttachIfNot(TEntity entity)
        {
            if (!Table.Local.Contains(entity))
            {
                Table.Attach(entity);
            }
        }

        #endregion

        #region Delete
        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="id">实体主键</param>
        /// <param name="autoSave">是否立即执行保存</param>
        public void Delete(TPrimaryKey id)
        {
            var entity = Table.Local.FirstOrDefault(ent => EqualityComparer<TPrimaryKey>.Default.Equals(ent.Id, id));
            if (entity == null)
            {
                entity = FirstOrDefault(id);
                if (entity == null)
                {
                    return;
                }
            }

            Delete(entity);
        }
        public Task DeleteAsync(TPrimaryKey id)
        {
            Delete(id);
            return Task.FromResult(0);
        }
        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="entity">要删除的实体</param>
        /// <param name="autoSave">是否立即执行保存</param>
        public void Delete(TEntity entity)
        {
            //AttachIfNot(entity);
            _dbContext.Entry(entity).State = EntityState.Deleted;
        }

        public Task DeleteAsync(TEntity entity)
        {
            Delete(entity);
            return Task.FromResult(0);
        }
        /// <summary>
        /// 根据条件删除实体
        /// </summary>
        /// <param name="where">lambda表达式</param>
        /// <param name="autoSave">是否自动保存</param>
        public void Delete(Expression<Func<TEntity, bool>> predicate)
        {

            DeleteRange(GetAll().Where(predicate));
        }

        public Task DeleteAsync(Expression<Func<TEntity, bool>> predicate)
        {
            Delete(predicate);
            return Task.FromResult(0);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="entities"></param>
        public void DeleteRange(IQueryable<TEntity> entities)
        {
            Table.RemoveRange(entities);
        }

        #endregion


        /// <summary>
        /// 事务性保存
        /// </summary>
        public void Save()
        {
            _dbContext.SaveChanges();
        }

        public async Task SaveAsync()
        {
           await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// 根据主键构建判断表达式
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        protected static Expression<Func<TEntity, bool>> CreateEqualityExpressionForId(TPrimaryKey id)
        {
            ///简单方式
            ////参数
            //var lambdaParam = Expression.Parameter(typeof(TEntity));
            ////比较==
            //var lambdaBody = Expression.Equal(
            //    Expression.PropertyOrField(lambdaParam, "Id"),
            //    Expression.Constant(id, typeof(TPrimaryKey))
            //    );

            var lambdaParam = Expression.Parameter(typeof(TEntity));

            var leftExpression = Expression.PropertyOrField(lambdaParam, "Id");

            var idValue = Convert.ChangeType(id, typeof(TPrimaryKey));

            Expression<Func<object>> closure = () => idValue;
            var rightExpression = Expression.Convert(closure.Body, leftExpression.Type);

            var lambdaBody = Expression.Equal(leftExpression, rightExpression);

            //生成表达式
            return Expression.Lambda<Func<TEntity, bool>>(lambdaBody, lambdaParam);
        }
    }
}
