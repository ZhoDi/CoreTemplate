using CoreTemplate.Domain;
using CoreTemplate.Domain.IRepositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using CoreTemplate.Domain.APIModel;
using CoreTemplate.Domain.Shared.Extensions;

namespace CoreTemplate.EntityFrameworkCore.Repositories
{
    public class Repository<TEntity, TPrimaryKey> : IRepository<TEntity, TPrimaryKey> where TEntity : class, IEntity<TPrimaryKey>
    {

        //定义数据访问上下文对象
        protected readonly TempDbContext DbContext;

        /// <summary>
        /// 通过构造函数注入得到数据上下文对象实例
        /// </summary>
        /// <param name="dbContext"></param>
        public Repository(TempDbContext dbContext)
        {
            DbContext = dbContext;
        }

        /// <summary>
        /// Gets DbSet for given entity.
        /// </summary>
        public virtual DbSet<TEntity> Table => DbContext.Set<TEntity>();

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
                query = propertySelectors.Aggregate(query, (current, propertySelector) => current.Include(propertySelector));
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
                query = propertySelectors.Aggregate(query, (current, propertySelector) => current.Include(propertySelector));
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
            return await (await GetAllAsync()).ToListAsync();
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
            return await (await GetAllAsync()).Where(predicate).ToListAsync();
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
            return await (await GetAllAsync()).FirstOrDefaultAsync(CreateEqualityExpressionForId(id));

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
            return await (await GetAllAsync()).FirstOrDefaultAsync(predicate);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="intPageIndex"></param>
        /// <param name="intPageSize"></param>
        /// <param name="where"></param>
        /// <param name="order"></param>
        /// <param name="orderType"></param>
        /// <returns></returns>
        public PageModel<TEntity> GetPageList(int intPageIndex, int intPageSize, Expression<Func<TEntity, bool>> where, Expression<Func<TEntity, object>> order, string orderType = "asc")
        {
            var result = GetAll().Where(where);
            var totalCount = result.Count();
            if (order != null)
            {
                result = orderType == "asc" ? result.OrderBy(order) : result.OrderByDescending(order);
            }
            else
            {
                result = result.OrderByDescending(m => m.Id);
            }
            var list = result.Skip((intPageIndex - 1) * intPageSize).Take(intPageSize).ToList();

            return new PageModel<TEntity>() { TotalCount = totalCount, Items = list};
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
        public async Task<PageModel<TEntity>> GetPageListAsync(int intPageIndex, int intPageSize, Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, object>> order, string orderType = "asc")
        {
            var result = (await GetAllAsync()).Where(whereExpression);
            var totalCount = result.Count();
            if (order != null)
            {
                if (orderType != null && orderType == "asc")
                    result = result.OrderBy(order);
                else
                    result = result.OrderByDescending(order);
            }

            var list = await result.Skip((intPageIndex - 1) * intPageSize).Take(intPageSize).ToListAsync();

            return new PageModel<TEntity>() {TotalCount = totalCount, Items = list};
        }

        /// <summary>
        /// 通过Sql分页查询
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="sql"></param>
        /// <param name="order"></param>
        /// <param name="orderType"></param>
        /// <returns></returns>
        public PageModel<TEntity> FindPageListFromSql(int pageIndex, int pageSize, string sql, Expression<Func<TEntity, object>> order, string orderType = "asc")
        {
            var result = GetFromSql(sql);

            var totalCount = result.Count();
            if (order != null)
            {
                result = orderType == "asc" ? result.OrderBy(order) : result.OrderByDescending(order);
            }
            var list = result.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

            return new PageModel<TEntity>() {  TotalCount = totalCount, Items = list };
        }

        /// <summary>
        /// 通过Sql查询
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public IQueryable<TEntity> GetFromSql(string sql)
        {
            var list = Table.FromSqlRaw(sql);

            return list;
        }

        #endregion

        #region Insert
        /// <summary>
        /// 新增实体
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="autoSave">是否立即执行保存</param>
        /// <returns></returns>
        public TEntity Insert(TEntity entity, bool autoSave = true)
        {
            var result = Table.Add(entity).Entity;
            if (autoSave)
                Save();
            return result;
        }

        /// <summary>
        /// 新增实体
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="autoSave">是否立即执行保存</param>
        /// <returns></returns>
        public async Task<TEntity> InsertAsync(TEntity entity, bool autoSave = true)
        {
            var result = await InsertAsync(entity);
            if (autoSave)
                await SaveAsync();
            return result;
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="entities"></param>
        public void BatchInsert(List<TEntity> entities)
        {
            Table.AddRange(entities);
            Save();
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="entities"></param>
        public async Task BatchInsertAsync(List<TEntity> entities)
        {
            await Table.AddRangeAsync(entities);
            await SaveAsync();
        }

        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="autoSave">是否立即执行保存</param>
        public TEntity Update(TEntity entity, bool autoSave = true)
        {
            // 发现并没什么用，先注释
            //AttachIfNot(entity);
            DbContext.Entry(entity).State = EntityState.Modified;
            if (autoSave)
                Save();
            return entity;
        }

        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="autoSave">是否立即执行保存</param>
        public async Task<TEntity> UpdateAsync(TEntity entity, bool autoSave = true)
        {
            await UpdateAsync(entity);
            if (autoSave)
                Save();
            return entity;
        }

        /// <summary>
        /// 新增或更新实体
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="autoSave">是否立即执行保存</param>
        public TEntity InsertOrUpdate(TEntity entity, bool autoSave = true)
        {
            var result = entity.IsTransient()
                 ? Insert(entity)
                 : Update(entity);
            if (autoSave)
                Save();
            return result;
        }

        /// <summary>
        /// 新增或更新实体
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="autoSave">是否立即执行保存</param>
        public async Task<TEntity> InsertOrUpdateAsync(TEntity entity, bool autoSave = true)
        {
            var result = entity.IsTransient()
                    ? await InsertAsync(entity)
                    : await UpdateAsync(entity);
            if (autoSave)
                await SaveAsync();
            return result;
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
        /// 根据Id删除实体
        /// </summary>
        /// <param name="id">实体主键</param>
        /// <param name="autoSave">是否立即执行保存</param>
        public void Delete(TPrimaryKey id, bool autoSave = true)
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
            Delete(entity, autoSave);
        }

        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="entity">要删除的实体</param>
        /// <param name="autoSave">是否立即执行保存</param>
        public void Delete(TEntity entity, bool autoSave = true)
        {
            //AttachIfNot(entity);
            DbContext.Entry(entity).State = EntityState.Deleted;
            if (autoSave)
                Save();
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

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="entities"></param>
        public void DeleteRange(IQueryable<TEntity> entities)
        {
            Table.RemoveRange(entities);
            Save();
        }

        #endregion


        /// <summary>
        /// 事务性保存
        /// </summary>
        public void Save()
        {
            DbContext.SaveChanges();
        }

        public async Task SaveAsync()
        {
            await DbContext.SaveChangesAsync();
        }

        /// <summary>
        /// 根据主键构建判断表达式
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        protected static Expression<Func<TEntity, bool>> CreateEqualityExpressionForId(TPrimaryKey id)
        {
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
