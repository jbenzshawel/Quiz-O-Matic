using Npgsql;

namespace ApiQuizGenerator.DAL
{
    /// <summary>
    /// Object with PostgreSql Function information with Name property of sql 
    /// function name and  Parameters property of type NpgsqlParameter[]
    /// </summary>
    public class PgSqlFunction
    {
        public string Name { get; set; }

        public NpgsqlParameter[] Parameters { get; set; } 
    }
    
}