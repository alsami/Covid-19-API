// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace Covid19Api.Repositories.Mongo
{
    public class DocumentDbContextOptions
    {
        public string ConnectionString { get; set; }

        public string DatabaseName { get; set; }
    }
}