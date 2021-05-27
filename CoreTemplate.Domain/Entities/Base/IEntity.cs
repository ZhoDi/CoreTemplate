namespace CoreTemplate.Domain.Entities.Base
{
    /// <summary>
    /// 定义含有主键Id的实体类接口
    /// </summary>
    public interface IEntity<TPrimaryKey>
    {
        TPrimaryKey Id { get; set; }

        bool IsTransient();
    }

    public interface IEntity : IEntity<int>
    {

    }
}
