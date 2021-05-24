namespace CoreTemplate.Application.Dto.Base
{
    public abstract class Dto<TPrimaryKey> :IDto<TPrimaryKey>
    {
        public TPrimaryKey Id { get; set; }
    }

    public class Dto : Dto<int>
    {

    }
}
