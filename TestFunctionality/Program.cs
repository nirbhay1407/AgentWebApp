using Ioc.Data.Data;
using static System.Net.Mime.MediaTypeNames;

Console.WriteLine("Hello, World!");

/*using (var context = new IocDbContext())
{
    context.Database.ExecuteSqlRaw(sqlScript);
}
*/

// Get the current directory where the application is running
var currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
var projectDirectory = Directory.GetParent(currentDirectory)?.Parent?.Parent?.FullName;
// Combine it with the relative path to your SQL files
var relativePath = @"Ioc.Data/_Database/MainDB/ObjectList"; 
var fullPath = Path.Combine(currentDirectory, relativePath); // Get all SQL files in the directory
                                                             
var sqlFiles = Directory.GetFiles(fullPath, "*.sql");


//var sqlFiles = Directory.GetFiles("D:/GIT/MainRepo/AgentWebApp01/Ioc.Data/_Database/MainDB/ObjectList", "*.sql");
var AddList = File.ReadAllText("D:/GIT/MainRepo/AgentWebApp01/Ioc.Data/_Database/MainDB/NeedToAdd.sql");
var deleteList = File.ReadAllText("D:/GIT/MainRepo/AgentWebApp01/Ioc.Data/_Database/MainDB/NeedToDelete.sql");

var sqlScripts = new List<string>();
var sqlAddList = new List<string>();
var sqlDeleteList = new List<string>();
sqlAddList.AddRange(AddList.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries).Select(line => line.Trim()).ToList());
sqlDeleteList.AddRange(deleteList.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries).Select(line => line.Trim()).ToList());
foreach (var file in sqlFiles)
{
    if (!sqlAddList.Contains(Path.GetFileNameWithoutExtension(file)))
        continue;
    var script = File.ReadAllText(file);
    sqlScripts.Add(script);
}