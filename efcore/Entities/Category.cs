namespace efcore.Entities
{
    public class Category
    {
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public string CategoryDescription { get; set; } = string.Empty;
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public CategoryInputDto ToInputDto()
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
                UserName = this.User.Username
            };
        }
    }
    public static class CategoryExtensions
    {
        public static List<CategoryOutputDto> ToOutputDtoList(this IEnumerable<Category> categories)
            => categories.Select(c => c.ToOutputDto()).ToList();
    }
}
