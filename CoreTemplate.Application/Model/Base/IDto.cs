namespace CoreTemplate.Application.Model.Base
{
    public interface IDto<TPrimaryKey>
    {
        TPrimaryKey Id { get; set; }
    }

    public interface IDto: IDto<int>
    {

    }
}
