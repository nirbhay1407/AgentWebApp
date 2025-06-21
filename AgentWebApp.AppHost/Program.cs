var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.AgentWebApp>("agentwebapp");

builder.Build().Run();
