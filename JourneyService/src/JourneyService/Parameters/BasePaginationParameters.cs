namespace JourneyService.Parameters
{
    public abstract class BasePaginationParameters
    {
        /// <summary>
        /// Gets or sets the maximum size of the page.
        /// </summary>
        internal virtual int MaxPageSize { get; set; } = 50;

        /// <summary>
        /// Gets or sets the default size of the page.
        /// </summary>
        internal virtual int DefaultPageSize { get; set; } = 10;

        /// <summary>
        /// Gets or sets the page number.
        /// </summary>
        public virtual int PageNumber { get; set; } = 1;

        /// <summary>
        /// Gets or sets the size of the page.
        /// </summary>
        public int PageSize
        {
            get
            {
                return DefaultPageSize;
            }
            set
            {
                DefaultPageSize = value > MaxPageSize ? MaxPageSize : value;
            }
        }
    }
}