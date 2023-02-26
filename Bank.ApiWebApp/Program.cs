using Bank.ApiWebApp;

static IHostBuilder CreateHostBuilder(string[] args) => Host
    .CreateDefaultBuilder(args)
    .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>());

CreateHostBuilder(args).Build().Run();
