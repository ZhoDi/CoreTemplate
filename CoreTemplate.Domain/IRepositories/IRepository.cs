using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using CoreTemplate.Domain.Entities.Base;
using CoreTemplate.Domain.Model;

namespace CoreTemplate.Domain.IRepositories
{

    public interface IRepository<TEntity, TKey> where TEntity : class, IEntity<TKey>
    {
        #region Select/Get/Query
        /// <summary>
        /// 获取全部
        /// </summary>
        IQueryable<TEntity> GetAll();
        Task<IQueryable<TEntity>> GetAllAsync();

        /// <summary>
        /// 获取并排序
        /// </summary>
        /// <param name="propertySelectors"></param>
        /// <returns></returns>
        IQueryable<TEntity> GetAllIncluding(params Expression<Func<TEntity, object>>[] propertySelectors);

        /// <summary>
        /// 获取并排序
        /// </summary>
        /// <param name="propertySelectors"></param>
        /// <returns></returns>
        Task<IQueryable<TEntity>> GetAllIncludingAsync(params Expression<Func<TEntity, object>>[] propertySelectors);

        /// <summary>
        /// 获取实体集合
        /// </summary>
        /// <returns></returns>
        List<TEntity> GetAllList();

        /// <summary>
        /// 根据lambda表达式条件获取实体集合<paramref name="predicate"/>.
        /// </summary>
        /// <param name="predicate">lambda表达式条件</param>
        /// <returns></returns>
        List<TEntity> GetAllList(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// 获取实体集合
        /// </summary>
        /// <returns>List of all entities</returns>
        Task<List<TEntity>> GetAllListAsync();

        /// <summary>
        /// 根据lambda表达式条件获取实体集合 <paramref name="predicate"/>.
        /// </summary>
        /// <param name="predicate">lambda表达式条件</param>
        /// <returns>List of all entities</returns>
        Task<List<TEntity>> GetAllListAsync(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="id">实体主键</param>
        /// <returns></returns>
        TEntity FirstOrDefault(TKey id);

        /// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<TEntity> FirstOrDefaultAsync(TKey id);

        /// <summary>
        /// 根据lambda表达式条件获取单个实体
        /// </summary>
        /// <param name="predicate">lambda表达式条件</param>
        /// <returns></returns>
        TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// 根据lambda表达式条件获取单个实体
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <returns></returns>
        PageModel<TEntity> GetPageList(int intPageIndex, int intPageSize, Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, object>> order,   string orderType = "asc");

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <returns></returns>
        Task<PageModel<TEntity>> GetPageListAsync(int intPageIndex, int intPageSize, Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, object>> order, string orderType = "asc");

        /// <summary>
        /// sql分页获取数据
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalRecord"></param>
        /// <param name="sql"></param>
        /// <param name="order"></param>
        /// <param name="orderType"></param>
        /// <returns></returns>
        PageModel<TEntity> FindPageListFromSql(int pageIndex, int pageSize, string sql, Expression<Func<TEntity, object>> order, string orderType = "asc");

        /// <summary>
        /// sql语句
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        IQueryable<TEntity> GetFromSql(string sql);

        #endregion

        #region Insert/Update

        /// <summary>
        /// 新增实体
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="autoSave">是否立即执行保存</param>
        /// <returns></returns>
        TEntity Insert(TEntity entity, bool autoSave = true);

        /// <summary>
        /// 新增实体
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="autoSave"></param>
        /// <returns></returns>
        Task<TEntity> InsertAsync(TEntity entity, bool autoSave = true);

        void BatchInsert(List<TEntity> entities);
        Task BatchInsertAsync(List<TEntity> entities);

        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="autoSave">是否立即执行保存</param>
        TEntity Update(TEntity entity, bool autoSave = true);
        Task<TEntity> UpdateAsync(TEntity entity, bool autoSave = true);


        /// <summary>
        /// 新增或更新实体
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="autoSave">是否立即执行保存</param>
        TEntity InsertOrUpdate(TEntity entity, bool autoSave = true);

        /// <summary>
        /// 新增或更新实体
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="autoSave">是否立即执行保存</param>
        Task<TEntity> InsertOrUpdateAsync(TEntity entity, bool autoSave = true);

        void AttachIfNot(TEntity entity);

        #endregion

        #region Delete
        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="entity">要删除的实体</param>
        /// <param name="autoSave">是否立即执行保存</param>
        void Delete(TEntity entity, bool autoSave = true);

        /// <summary>
        /// 根据Id删除实体
        /// </summary>
        /// <param name="id">实体主键</param>
        /// <param name="autoSave">是否立即执行保存</param>
        void Delete(TKey id, bool autoSave = true);

        /// <summary>
        /// 根据条件删除实体
        /// </summary>
        /// <param name="where">lambda表达式</param>
        void Delete(Expression<Func<TEntity, bool>> where);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="entities"></param>
        void DeleteRange(IQueryable<TEntity> entities);

        #endregion

        void Save();
        Task SaveAsync();
    }
}
