var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

// Setup HttpClient
builder.Services.AddHttpClient("BackendApi", client =>
{
    client.BaseAddress = new Uri("http://groupprojectbackendv2-dev.ca-central-1.elasticbeanstalk.com/");
    //client.BaseAddress = new Uri("http://groupprojectbackend-dev.ca-central-1.elasticbeanstalk.com/");
    //client.BaseAddress = new Uri("https://localhost:7051/");
});

// Setup Session
builder.Services.AddSession(options =>
{
    options.Cookie.HttpOnly = true;
    options.IdleTimeout = TimeSpan.FromHours(1);
    options.Cookie.IsEssential = true;
});

// Setup Authentication
builder.Services.AddAuthentication("MyCookieAuth").AddCookie("MyCookieAuth", options =>
{
    options.Cookie.Name = "MyCookieAuth";
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";
    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
});

// Setup Authorization
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policy =>
    {
        policy.RequireClaim("Admin", "True");
    });

    options.AddPolicy("Auth", policy =>
    {
        policy.RequireAuthenticatedUser();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseSession();

app.MapRazorPages();

app.Run();
