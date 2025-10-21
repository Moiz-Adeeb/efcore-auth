namespace efcore.Entities
{
    public class Category
    {
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public string CategoryDescription { get; set; } = string.Empty;
        public Guid UserId { get; set; } 
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public CategoryInputDto ToDto()
        {
            return new CategoryInputDto
            {
                CategoryName = this.CategoryName,
                CategoryDescription = this.CategoryDescription
            };
        }
        public CategoryOutputDto ToOutputDto()
        {
            return new CategoryOutputDto
            {
                CategoryId = this.CategoryId,
                CategoryName = this.CategoryName,
                CategoryDescription = this.CategoryDescription,
            };
        }
    }
}
