using System.Data.Entity;
using System.Data.Entity.SqlServer;
namespace ContosoUniversity.DAL
{
    public class SchoolConfiguration : DbConfiguration
    {
        public SchoolConfiguration()
        {
            SetExecutionStrategy("System.Data.SqlClient", () => new SqlAzureExecutionStrategy());
        }
    }
}



//pg. 77: Getting Started with Entity Framework 6 Code First Using MVC 5

//To enable connection resiliency; create a class in your assembly that derives from the DbConfiguration class, 
//and in that class set the SQL Database execution strategy, which in EF is another term for retry policy.

//The Entity Framework automatically runs the code it finds in a class that derives from DbConfiguration. 
//Can use the DbConfiguration class to do configuration tasks in code that you would otherwise do in the Web.config file.